
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
ENV BUILD_CONFIG Debug
LABEL maintainer=me@iamfara.com \
    Name=assessment-${BUILD_CONFIG} \
    Version=0.0.1
ARG URL_PORT
WORKDIR /app
ENV NUGET_XMLDOC_MODE skip
ENV ASPNETCORE_URLS http://*:${URL_PORT}
ENTRYPOINT [ "dotnet", "Assessment.API.dll" ]