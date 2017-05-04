using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Akka.Configuration.Hocon;

namespace WebCrawler.CrawlService
{
    public static class CrawlServiceFactory
    {
        internal static Uri GetSelfIpDiscoveryUri(Config config)
        {
            var seedNodes = config.GetStringList("akka.cluster.seed-nodes");
            Uri seedNodeUri;
            Uri.TryCreate(seedNodes[0], UriKind.Absolute, out seedNodeUri);
            var seedNodeHttpUri = new UriBuilder("http", seedNodeUri.Host, 9000, "api/whatsmyip");

            return seedNodeHttpUri.Uri;
        }

        internal static string GetSelfIpAddress(HttpClient client, Uri selfIpDiscoveryUri)
        {
            var response = client.GetAsync(selfIpDiscoveryUri).Result;
            return response.Content.ReadAsStringAsync().Result;
        }

        internal static Config CreateRemoteConfig(string selfIpAddress)
        {
            Uri selfAddressUri;
            Uri.TryCreate(selfIpAddress, UriKind.Absolute, out selfAddressUri);
            var selfIp = selfAddressUri.Host;
            var selfPort = selfAddressUri.Port;

            var remoteConfig =
                ConfigurationFactory.ParseString($@"akka.remote.dot-netty.tcp.public-hostname = ""{selfIp}""
                                                 akka.remote.dot-netty.tcp.port = {selfPort}");

            return remoteConfig;
        }

        internal static Config CreateConfig()
        {
            var section = (AkkaConfigurationSection)ConfigurationManager.GetSection("akka");
            var config = section.AkkaConfig;

            var selfIpDiscoveryUri = GetSelfIpDiscoveryUri(config);
            string selfIpAddress;
            using (var client = new HttpClient())
            {
                selfIpAddress = GetSelfIpAddress(client, selfIpDiscoveryUri);
            }

            var remoteConfig = CreateRemoteConfig(selfIpAddress);
            var finalConfig = remoteConfig.WithFallback(config);
            return finalConfig;
        }

        public static ActorSystem LaunchTrackingService(string systemName)
        {
            var config = CreateConfig();
            return ActorSystem.Create(systemName, config);
        }
    }
}
