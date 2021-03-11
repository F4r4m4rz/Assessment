
FROM  mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
ARG BUILD_CONFIG=Debug
ARG BUILDER_VERSION=0.0.1
LABEL maintainer=me@iamfara.com \
    Name=assessment-build-${BUILD_CONFIG} \
    Version=${BUILDER_VERSION}

ENV NUGET_XMLDOC_MODE skip
ENV ASPNETCORE_URLS http://*:7909
WORKDIR /app
ENTRYPOINT ["dotnet", "Assessment.API.dll"]