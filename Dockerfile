# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /App
COPY . .
RUN dotnet restore
RUN dotnet publish HetznerApiDynDNS -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine
LABEL \
  org.opencontainers.image.title="HetznerApiDynDNSUpdater" \
  org.opencontainers.image.description="Updates a DNS record on Hetzner" \
  org.opencontainers.image.version="1.0.0" \
  org.opencontainers.image.authors="Rafael Schniedermann <rafael.schniedermann@gmx.de>" \
  org.opencontainers.image.source="https://github.com/mRy0/HetznerApiDynDNS" \
  org.opencontainers.image.licenses="MIT"

WORKDIR /App
COPY --from=build /App/out .
ENTRYPOINT ["dotnet", "HetznerApiDynDNS.dll"]
