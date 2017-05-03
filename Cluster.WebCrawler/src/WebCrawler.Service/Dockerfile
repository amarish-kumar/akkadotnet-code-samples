# escape=`

FROM microsoft/windowsservercore

COPY ./bin/Release/ C:/trackingservice/

RUN ["C:\\trackingservice\\WebCrawler.TrackingService.exe","install","--localsystem","--autostart"]