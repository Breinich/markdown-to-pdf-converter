FROM pandoc/latex:latest-ubuntu AS base

USER $APP_UID
WORKDIR /app

RUN wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
RUN rm packages-microsoft-prod.deb

RUN apt-get update && apt-get upgrade -y
RUN apt-get install -y nodejs npm dotnet-sdk-8.0
RUN npm install -g n
RUN n lts
RUN npm install -g @mermaid-js/mermaid-cli

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NotionToPdf.csproj", "./"]
RUN dotnet restore "NotionToPdf.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "NotionToPdf.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "NotionToPdf.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
