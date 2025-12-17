Updates an IP Adress over Hetzner API

usage:
docker run --rm --name hetzner-dyndns -e APIKEY=***** -e ZONENAME=example.net -e ARECORDNAMES__0=www -e ARECORDNAMES__1="@" hetzner-dyndns:latest

Or
/App/appsettings.json 
{
  "ApiKey": "YOUR_API_KEY_HERE",
  "ZoneName": "example.net",
  "ARecordNames": [ "@", "www"]
}


Build:
docker build -t hetzner-dyndns:latest .
