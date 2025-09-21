namespace Minecheat.Modules.World;

public class WorldFeatureModule : IFeatureModule
{
    public string ModuleTitle => "世界";

    public IReadOnlyList<IFeature> Features => this.features.AsReadOnly();

    private readonly IFeature[] features =
    [
        new CheatModeFeature(),
    ];
}
