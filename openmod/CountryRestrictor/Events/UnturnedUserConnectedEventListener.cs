using CountryRestrictor.API;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OpenMod.API.Eventing;
using OpenMod.Core.Eventing;
using OpenMod.Unturned.Users;
using OpenMod.Unturned.Users.Events;
using SDG.Unturned;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CountryRestrictor.Events;

[EventListenerLifetime(ServiceLifetime.Singleton)]
internal class UnturnedUserConnectedEventListener : IEventListener<UnturnedUserConnectedEvent>
{
    private readonly ICountryFinderService _countryFinderService;
    private readonly ILogger<UnturnedUserConnectedEventListener> _logger;
    private readonly IConfiguration _configuration;
    private readonly IStringLocalizer _stringLocalizer;

    public UnturnedUserConnectedEventListener(
        ILogger<UnturnedUserConnectedEventListener> logger,
        ICountryFinderService countryFinderService,
        IConfiguration configurationd,
        IStringLocalizer stringLocalizer)
    {
        _logger = logger;
        _countryFinderService = countryFinderService;
        _configuration = configurationd;
        _stringLocalizer = stringLocalizer;
    }

    public Task HandleEventAsync(object? sender, UnturnedUserConnectedEvent @event)
    {
        UniTask.Run(async () =>
        {
            if (!@event.User.Player.Player.channel.GetOwnerTransportConnection().TryGetIPv4Address(out var ipAddress))
            {
                _logger.LogError("Failed to obtain ip from player {DisplayName} {SteamId}. Letting him join the server...", @event.User.DisplayName, @event.User.SteamId);
                return;
            }
            var (isSucess, countryCode) = await _countryFinderService.FindCountryAsync(ipAddress);
            if (!isSucess)
            {
                _logger.LogError("Failed to obtain country from player {DisplayName} {SteamId} with {ipAddress}", @event.User.DisplayName, @event.User.SteamId, ipAddress);
                return;
            }

            var allowedCountries = _configuration.GetValue<List<string>>("allowed_countries", new());

            if (allowedCountries.Count != 0 && !allowedCountries.Contains(countryCode))
            {
                _logger.LogInformation("Disallowing {DisplayName} {SteamId} from {countryCode} as they country is  the allowed countries list", @event.User.DisplayName, @event.User.SteamId, countryCode);
                await KickUserAsync(@event.User, _stringLocalizer["not_whitelisted_country"]);
                return;
            }

            var notAllowedCountries = _configuration.GetValue<List<string>>("not_allowed_countries", new());

            if (notAllowedCountries.Count != 0 && notAllowedCountries.Contains(countryCode))
            {
                _logger.LogInformation("Disallowing {DisplayName} {SteamId} from {countryCode} as they country is in the not allowed countries list", @event.User.DisplayName, @event.User.SteamId, countryCode);
                await KickUserAsync(@event.User, _stringLocalizer["blacklisted_country"]);
                return;
            }

            _logger.LogInformation("Letting {DisplayName} {SteamId} from {countryCode} enter the server", @event.User.DisplayName, @event.User.SteamId, countryCode);
        });

        return Task.CompletedTask;
    }

    private async UniTask KickUserAsync(UnturnedUser user, string reason)
    {
        await UniTask.SwitchToMainThread();
        Provider.ban(user.SteamId, reason, 3600); // prevent him from joining again almost nobody will bring his pc to another country in less than an hour LMAO
    }
}
