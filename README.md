# CountryRestrictor

## Features

- Allow only certain countries to enter your server
- Allow every country to enter your server with some exceptions

---

## Configuration

The configuration is divided in 2 groups Allowed Countries and Not Allowed Countries depending on your needs you will leave 1 of those empty.

To allow every country and restrict only some do it like this:

In this example every country except of `Afghanistan` and `Algeria` are allowed to join the server.

**OpenMod**

```yaml
not_allowed_countries: # This is just an example
  - AF # Afghanistan
  - DZ # Algeria
allowed_countries: []
```

**RocketMod**

```xml
<?xml version="1.0" encoding="utf-8"?>
<Configuration xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <AllowedCountries />
  <UnAllowedCountries>
    <CountryCode>AF</CountryCode>
    <CountryCode>DZ</CountryCode>
  </UnAllowedCountries>
</Configuration>
```

---

To only allow some countries do it like this:

In this example on `Argentina` and `United States` are allowed to join the server.

**OpenMod**

```yaml
not_allowed_countries: []
allowed_countries:
  - AR # Argentina
  - US # United States
```

**RocketMod**

```xml
<?xml version="1.0" encoding="utf-8"?>
<Configuration xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <AllowedCountries>
    <CountryCode>AR</CountryCode>
    <CountryCode>US</CountryCode>
  </AllowedCountries>
  <UnAllowedCountries />
</Configuration>
```

---

## Translations

**OpenMod**

```yaml
blacklisted_country: "The country your are playing from is not allowed in this server"
not_whitelisted_country: "The country your are playing from is not allowed in this server"
# Different translations depending if the ip country was in allowed_countries or not_allowed_countries
```

**RocketMod**

```xml
<?xml version="1.0" encoding="utf-8"?>
<Translations xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Translation Id="blacklisted_country" Value="The country your are playing from is not allowed in this server" />
  <Translation Id="not_whitelisted_country" Value="The country your are playing from is not allowed in this server" />
</Translations>
```

---

## Installation

**OpenMod**

Execute `openmod install Feli.CountryRestrictor`.

**RocketMod**

Download the latest release from [UnturnedStore](https://unturnedstore.com/products/1541)
