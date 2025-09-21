using fNbt;

namespace Minecheat.Modules.Player;

public abstract class BeGameModeFeature : PlayerBasedFeature
{
    protected abstract GameType GameMode { get; }

    protected override void EditPlayerTag(SaveInfo saveInfo, NbtCompound playerTag, Action<string> appendLog)
    {
        var gameTypeTag = playerTag.Get<NbtInt>("playerGameType")!;
        appendLog($"当前游戏模式: {Enum.GetName((GameType)gameTypeTag.Value)}");
        gameTypeTag.Value = (int)this.GameMode;
        appendLog($"修改后游戏模式: {Enum.GetName((GameType)gameTypeTag.Value)}");
    }
}

public class BeSurvivalModeFeature : BeGameModeFeature
{
    public override string FeatureTitle => "切换至生存模式";

    public override string Tooltip => "当前玩家切换至生存模式";

    protected override GameType GameMode => GameType.Survival;
}

public class BeCreativeModeFeature : BeGameModeFeature
{
    public override string FeatureTitle => "切换至创造模式";
    public override string Tooltip => "当前玩家切换至创造模式";
    protected override GameType GameMode => GameType.Creative;
}

public class BeAdventureModeFeature : BeGameModeFeature
{
    public override string FeatureTitle => "切换至冒险模式";
    public override string Tooltip => "当前玩家切换至冒险模式";
    protected override GameType GameMode => GameType.Adventure;
}
public class BeSpectatorModeFeature : BeGameModeFeature
{
    public override string FeatureTitle => "切换至旁观者模式";
    public override string Tooltip => "当前玩家切换至旁观者模式";
    protected override GameType GameMode => GameType.Spectator;
}
