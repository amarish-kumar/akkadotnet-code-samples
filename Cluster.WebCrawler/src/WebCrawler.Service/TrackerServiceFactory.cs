using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Akka.Configuration;
using Akka.Configuration.Hocon;

namespace WebCrawler.TrackingService
{
    public static class TrackerServiceFactory
    {
        public static Uri GetSelfIpDiscoveryUri(Config config)
        {
            var seedNodes = config.GetStringList("akka.cluster.seed-nodes");
            Uri seedNodeUri;
            Uri.TryCreate(seedNodes[0], UriKind.Absolute, out seedNodeUri);
            var seedNodeHttpUri = new UriBuilder("http", seedNodeUri.Host, 9000, "api/whatsmyip");

            return seedNodeHttpUri.Uri;
        }

        public static string GetSelfIpAddress(HttpClient client, Uri selfIpDiscoveryUri)
        {
            var response = client.GetAsync(selfIpDiscoveryUri).Result;
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
