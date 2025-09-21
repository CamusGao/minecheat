using fNbt;

namespace Minecheat.Modules.World;

public class CheatModeFeature : LevelDataBasedFeature
{
    public override string FeatureTitle => "打开作弊";

    public override string Tooltip => this.FeatureTitle;

    protected override void EditLevelDataNbt(SaveInfo saveInfo, NbtFile levelNbt, Action<string> appendLog)
    {
        levelNbt.RootTag.Get<NbtCompound>("Data")!
            .Get<NbtByte>("allowCommands")!.Value = 1;
        appendLog("已打开作弊（允许命令）");
    }
}
