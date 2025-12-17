# HetznerApiDynDNS

![License](https://img.shields.io/badge/license-MIT-green)

A small command-line tool and Docker image that updates A records via the **Hetzner DNS API** (DynDNS-style).

It does **not** expose any network ports and is intended for scheduled execution.

[Duckerhub](h[ttps://github.com/mRy0/HetznerApiDynDNS/](https://hub.docker.com/repository/docker/raffi1337/hetzner-dyndns/general))

---

## Features

- Updates one or more A records in a Hetzner DNS zone
- Configuration via environment variables or `appsettings.json`
- Container-friendly (one-shot execution)

---

## Docker Image

- Image name: `hetzner-dyndns`
- Tag: `latest`

---

## Usage

### Configuration via environment variables (recommended)

```bash
docker run \
  -e APIKEY=***** \
  -e ZONENAME=example.net \
  -e ARECORDNAMES__0=www \
  -e ARECORDNAMES__1=@ \
  hetzner-dyndns:latest
```

---

### Configuration via `appsettings.json`

You can also provide configuration via an `appsettings.json` file mounted into `/App`.

Example `appsettings.json`:

```json
{
  "ApiKey": "YOUR_API_KEY_HERE",
  "ZoneName": "example.net",
  "ARecordNames": [ "@", "www" ]
}
```

Run the container:

```bash
docker run \
  -v $(pwd)/appsettings.json:/App/appsettings.json:ro \
  hetzner-dyndns:latest
```

---

## Configuration Reference

| Key | Description |
|-----|-------------|
| `ApiKey` | Hetzner DNS API key |
| `ZoneName` | DNS zone to update |
| `ARecordNames` | List of A record names to update |

---


## Development

Requirements:
- .NET 9 SDK
- Docker (optional)

Build locally:

```bash
dotnet publish HetznerApiDynDNS -c Release
```

---

## Notes

- Environment variables override values from `appsettings.json`.
- Designed for automation and unattended operation.

---

## License

MIT
