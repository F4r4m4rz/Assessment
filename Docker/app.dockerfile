
FROM  mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
ARG BUILD_CONFIG=Debug
ARG BUILDER_VERSION=0.0.1
LABEL maintainer=me@iamfara.com \
    Name=assessment-build-${BUILD_CONFIG} \
    Version=${BUILDER_VERSION}
ARG URL_PORT
ENV NUGET_XMLDOC_MODE skip
ENV ASPNETCORE_URLS http://*:${URL_PORT}
WORKDIR /app
COPY ./publish .
ENTRYPOINT ["dotnet", "Assessment.API.dll"]