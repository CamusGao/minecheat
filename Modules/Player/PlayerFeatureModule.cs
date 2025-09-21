namespace Minecheat.Modules.Player;

public class PlayerFeatureModule : IFeatureModule
{
    public string ModuleTitle => "玩家";

    public IReadOnlyList<IFeature> Features => this.features.AsReadOnly();

    private readonly IFeature[] features =
    [
        new Add300XpLevelFeature(),
        new BeAdventureModeFeature(),
        new BeCreativeModeFeature(),
        new BeSpectatorModeFeature(),
        new BeSurvivalModeFeature(),
        new FireResistanceFeature(),
        new FixEquipmentsFeature(),
        new HealthFeature(),
        new MoveToRespawnPosFeature(),
        new ReplenishInventoryFeature(),
    ];
}
