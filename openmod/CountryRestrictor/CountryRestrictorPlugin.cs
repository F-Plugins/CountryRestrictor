using OpenMod.API.Plugins;
using OpenMod.Unturned.Plugins;
using System;

[assembly: PluginMetadata("Feli.CountryRestrictorPlugin", DisplayName = "Country Restrictor Plugin", Author = "Feli", Website = "docs.fplugins.com")]

namespace CountryRestrictor;

public class CountryRestrictorPlugin : OpenModUnturnedPlugin
{
    public CountryRestrictorPlugin(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}