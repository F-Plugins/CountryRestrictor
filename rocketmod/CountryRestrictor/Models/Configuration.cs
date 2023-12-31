﻿using Rocket.API;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CountryRestrictor.Models;

public class Configuration : IRocketPluginConfiguration
{
    [XmlAttribute("CountryCode")]
    public List<string> AllowedCountries { get; set; } = new();
    [XmlAttribute("CountryCode")]
    public List<string> UnAllowedCountries { get; set; } = new();

    public void LoadDefaults()
    {
        AllowedCountries = new List<string>
        {
            "AF", "AX", "AL", "DZ", "AS", "AD", "AO", "AI", "AQ", "AG",
            "AR", "AM", "AW", "AU", "AT", "AZ", "BS", "BH", "BD", "BB",
            "BY", "BE", "BZ", "BJ", "BM", "BT", "BO", "BQ", "BA", "BW",
            "BV", "BR", "IO", "BN", "BG", "BF", "BI", "CV", "KH", "CM",
            "CA", "KY", "CF", "TD", "CL", "CN", "CX", "CC", "CO", "KM",
            "CG", "CD", "CK", "CR", "CI", "HR", "CU", "CW", "CY", "CZ",
            "DK", "DJ", "DM", "DO", "EC", "EG", "SV", "GQ", "ER", "EE",
            "SZ", "ET", "FK", "FO", "FJ", "FI", "FR", "GF", "PF", "TF",
            "GA", "GM", "GE", "DE", "GH", "GI", "GR", "GL", "GD", "GP",
            "GU", "GT", "GG", "GN", "GW", "GY", "HT", "HM", "VA", "HN",
            "HK", "HU", "IS", "IN", "ID", "IR", "IQ", "IE", "IM", "IL",
            "IT", "JM", "JP", "JE", "JO", "KZ", "KE", "KI", "KP", "KR",
            "KW", "KG", "LA", "LV", "LB", "LS", "LR", "LY", "LI", "LT",
            "LU", "MO", "MK", "MG", "MW", "MY", "MV", "ML", "MT", "MH",
            "MQ", "MR", "MU", "YT", "MX", "FM", "MD", "MC", "MN", "ME",
            "MS", "MA", "MZ", "MM", "NA", "NR", "NP", "NL", "NC", "NZ",
            "NI", "NE", "NG", "NU", "NF", "MP", "NO", "OM", "PK", "PW",
            "PS", "PA", "PG", "PY", "PE", "PH", "PN", "PL", "PT", "PR"
        };
        UnAllowedCountries = new List<string>()
        {
            "XZ" // non empty list on serialize country code invalid
        };
    }
}
