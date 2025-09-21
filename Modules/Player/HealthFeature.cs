using fNbt;

namespace Minecheat.Modules.Player;

public class HealthFeature : PlayerBasedFeature
{
    public override string FeatureTitle => "恢复健康";

    public override string Tooltip => this.FeatureTitle;

    protected override void EditPlayerTag(SaveInfo saveInfo, NbtCompound playerTag, Action<string> appendLog)
    {
        var healthTag = playerTag.GetOrAdd<NbtFloat>("Health", _ => new());
        appendLog($"当前生命值: {healthTag.Value}");
        healthTag.Value = 20.0f;

        var foodLevelTag = playerTag.GetOrAdd<NbtInt>("foodLevel", _ => new());
        appendLog($"当前饱食度: {foodLevelTag.Value}");
        foodLevelTag.Value = 20;

        appendLog($"已经恢复健康");
    }
}
