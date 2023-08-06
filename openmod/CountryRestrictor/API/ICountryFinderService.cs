using OpenMod.API.Ioc;
using System.Threading.Tasks;

namespace CountryRestrictor.API;

[Service]
public interface ICountryFinderService
{
    // not a big fan of tuple
    Task<(bool IsSucess, string CountryCode)> FindCountryAsync(uint ip);
}