# syntax=docker/dockerfile:1
ARG DOTNET_VERSION=8.0

# 1) Restore with good caching
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS restore
WORKDIR /src

# Copy only sln + project files first for better layer caching
COPY *.sln ./
COPY OktaCapstone.Shared/OktaCapstone.Shared.csproj ./OktaCapstone.Shared/
COPY OktaCapstone.Api/OktaCapstone.Api.csproj ./OktaCapstone.Api/
COPY OktaCapstone.Blazor/OktaCapstone.Blazor.csproj ./OktaCapstone.Blazor/

RUN dotnet restore

# 2) Build & publish
FROM restore AS publish
# Now bring in the rest of the source
COPY OktaCapstone.Shared ./OktaCapstone.Shared
COPY OktaCapstone.Api ./OktaCapstone.Api
COPY OktaCapstone.Blazor ./OktaCapstone.Blazor

# Publish both projects; UseAppHost=false keeps images smaller
RUN dotnet publish ./OktaCapstone.Api/OktaCapstone.Api.csproj -c Release -o /out/api /p:UseAppHost=false
RUN dotnet publish ./OktaCapstone.Blazor/OktaCapstone.Blazor.csproj -c Release -o /out/blazor /p:UseAppHost=false

# 3a) API runtime image
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS api
WORKDIR /app
COPY --from=publish /out/api ./
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "OktaCapstone.Api.dll"]

# 3b) Blazor Server runtime image
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS blazor
WORKDIR /app
COPY --from=publish /out/blazor ./
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "OktaCapstone.Blazor.dll"]


# Dev base (SDK)
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS devbase
WORKDIR /src
ENV ASPNETCORE_ENVIRONMENT=Development
# Needed for mounted volumes on Docker Desktop/WSL/macOS
ENV DOTNET_USE_POLLING_FILE_WATCHER=1
# Improves watch reliability in containers
ENV DOTNET_WATCH_RESTART_ON_RUDE_EDIT=1

# Dev target: API
FROM devbase AS api-dev
COPY . .
WORKDIR ./OktaCapstone.Api
EXPOSE 8080
# dotnet-watch will rebuild/restart or hot-reload as you edit
CMD ["dotnet", "watch", "run", "--no-launch-profile", "--urls", "http://0.0.0.0:8080"]

# Dev target: Blazor Server
FROM devbase AS blazor-dev
COPY . .
WORKDIR ./OktaCapstone.Blazor
EXPOSE 8080
CMD ["dotnet", "watch", "run", "--no-launch-profile", "--urls", "http://0.0.0.0:8080"]
