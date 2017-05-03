# escape=`

FROM microsoft/windowsservercore

COPY ./bin/Release/ C:/crawlservice/

RUN ["C:\\crawlservice\\WebCrawler.CrawlService.exe","install","--localsystem","--autostart"]