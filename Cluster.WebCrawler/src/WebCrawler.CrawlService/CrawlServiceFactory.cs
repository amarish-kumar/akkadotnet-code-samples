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
using WebCrawler.Bootstrapper;

namespace WebCrawler.CrawlService
{
    public static class CrawlServiceFactory
    { 
        public static ActorSystem LaunchTrackingService(string systemName)
        {
            var config = LighthouseConfigurationFactory.CreateConfig();
            return ActorSystem.Create(systemName, config);
        }
    }
}
