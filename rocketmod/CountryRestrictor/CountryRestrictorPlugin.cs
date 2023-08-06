using CountryRestrictor.Models;
using CountryRestrictor.Services;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Core.Utils;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logger = Rocket.Core.Logging.Logger;

namespace CountryRestrictor;

public class CountryRestrictorPlugin : RocketPlugin<Configuration>
{
    public override TranslationList DefaultTranslations => new()
    {
        {"blacklisted_country", "The country your are playing from is not allowed in this server" },
        {"not_whitelisted_country", "The country your are playing from is not allowed in this server" }
    };

    protected override void Load()
    {
        Logger.Log("Plugin brought to you by Feli from FPlugins. Discord with more plugins: discord.fplugins.com");

        U.Events.OnPlayerConnected += OnPlayerConnected;
    }

    protected override void Unload()
    {
        U.Events.OnPlayerConnected -= OnPlayerConnected;
    }

    private void OnPlayerConnected(UnturnedPlayer player)
    {
        Task.Run(async () =>
        {
            try
            {
                await OnPlayerConnectedAsync(player);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "There was an error during async player connected event");
            }
        });
    }

    private async Task OnPlayerConnectedAsync(UnturnedPlayer player)
    {
        if (!player.Player.channel.GetOwnerTransportConnection().TryGetIPv4Address(out var ipAddress))
        {
            Logger.LogError($"Failed to obtain ip from player {player.DisplayName} {player.CSteamID}. Letting him join the server...");
            return;
        }

        var (isSucess, countryCode) = await CountryFinderService.FindCountryAsync(ipAddress);
        if (!isSucess)
        {
            Logger.LogError($"Failed to obtain country from player {player.DisplayName} {player.CSteamID} with {ipAddress}");
            return;
        }
        var allowedCountries = Configuration.Instance.AllowedCountries;

        if (allowedCountries.Count != 0 && !allowedCountries.Contains(countryCode))
        {
            Logger.Log($"Disallowing {player.DisplayName} {player.CSteamID} from {countryCode} as they country is  the allowed countries list");
            KickUser(player, Translate("not_whitelisted_country"));
            return;
        }

        var notAllowedCountries = Configuration.Instance.UnAllowedCountries;

        if (notAllowedCountries.Count != 0 && notAllowedCountries.Contains(countryCode))
        {
            Logger.Log($"Disallowing {player.DisplayName} {player.CSteamID} from {countryCode} as they country is in the not allowed countries list");
            KickUser(player, Translate("blacklisted_country"));
            return;
        }

        Logger.Log($"Letting {player.DisplayName} {player.CSteamID} from {countryCode} enter the server");
    }

    private void KickUser(UnturnedPlayer user, string reason)
    {
        TaskDispatcher.QueueOnMainThread(() => Provider.ban(user.CSteamID, reason, 3600)); // prevent him from joining again almost nobody will bring his pc to another country in less than an hour LMAO
    }
}