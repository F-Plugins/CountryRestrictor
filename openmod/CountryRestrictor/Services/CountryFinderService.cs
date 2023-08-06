using CountryRestrictor.API;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenMod.API.Ioc;
using OpenMod.API.Prioritization;
using System.Net.Http;
using System.Threading.Tasks;

namespace CountryRestrictor.Services;

[PluginServiceImplementation(Lifetime = ServiceLifetime.Singleton, Priority = Priority.Lowest)]
internal class CountryFinderService : ICountryFinderService
{
    private readonly ILogger<CountryFinderService> _logger;

    public CountryFinderService(ILogger<CountryFinderService> logger)
    {
        _logger = logger;
    }

    public async Task<(bool IsSucess, string CountryCode)> FindCountryAsync(uint ip)
    {
        try
        {
            _logger.LogDebug("Fetching country from {ip}", ip);
            using var client = new HttpClient();
            var response = await client.GetAsync($"https://ip2c.org/?dec={ip}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var responses = content.Split(';');
            if (int.Parse(responses[0]) == 0 || responses[1] == "XZ") // XZ is special address 
            {
                _logger.LogError("Failed to fetch country code from {ip}", ip);
                return (false, string.Empty);
            }

            var countryCode = responses[1];
            _logger.LogDebug("IP: {ip} is from {countryCode}", ip, countryCode);
            return (true, countryCode);
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "There was an error while fetching the country of {ip}", ip);
            return (false, string.Empty);
        }
    }
}
