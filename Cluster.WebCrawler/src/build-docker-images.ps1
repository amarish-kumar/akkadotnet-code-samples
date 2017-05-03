docker build -f .\WebCrawler.Service\Dockerfile .\WebCrawler.Service -t nickc/trackingservice:latest
docker build -f .\WebCrawler.CrawlService\Dockerfile .\WebCrawler.CrawlService -t nickc/crawlservice:latest
docker build -f .\WebCrawler.Web\Dockerfile .\WebCrawler.Web -t nickc/webcrawlerweb:latest