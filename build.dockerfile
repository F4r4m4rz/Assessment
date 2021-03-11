FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /src
COPY Assessment.sln ./
COPY Assessment.Data/*.csproj ./Assessment.Data/
COPY Assessment.API/*.csproj ./Assessment.API/

RUN dotnet restore
COPY . .
WORKDIR /src/Assessment.Data
RUN dotnet build -c Release -o /app

WORKDIR /src/Assessment.API
RUN dotnet build -c Release -o /app

WORKDIR /src
RUN dotnet build -c Release -o /app/Assessment.sln


WORKDIR /src
RUN dotnet publish -c Release -o /app
