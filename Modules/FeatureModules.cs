using Minecheat.Modules.Player;
using Minecheat.Modules.World;

namespace Minecheat.Modules;

public static class FeatureModules
{
    public static readonly IReadOnlyList<IFeatureModule> Modules =
        [
            new PlayerFeatureModule(),
            new WorldFeatureModule(),
        ];
}
