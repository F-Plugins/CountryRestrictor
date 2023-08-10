using Rocket.Core.Logging;
using SDG.Unturned;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace CountryRestrictor.Services;

internal static class CountryFinderService
{
    public static async Task<(bool IsSucess, string CountryCode)> FindCountryAsync(uint ip)
    {
        try
        {
            Logger.Log($"Fetching country from {ip}");
            var request = CreateRequest(ip);
            var response = await request.GetResponseAsync();
            var content = await ReadContentAsync(response.GetResponseStream());

            var responses = content.Split(';');
            if (int.Parse(responses[0]) == 0 || responses[1] == "XZ") // XZ is special address 
            {
                Logger.LogError($"Failed to fetch country code from {ip}");
                return (false, string.Empty);
            }

            var countryCode = responses[1];
            Logger.Log($"IP: {ip} is from {countryCode}");
            return (true, countryCode);
        }
        catch (Exception exception)
        {
            Logger.LogException(exception, $"There was an error while fetching the country of {ip}");
            return (false, string.Empty);
        }
    }

    private static WebRequest CreateRequest(uint ip)
    {
        return WebRequest.Create($"https://ip2c.org/?dec={ip}");
    }

    private static async Task<string> ReadContentAsync(Stream stream)
    {
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
}
