using fNbt;
using System.IO;

namespace Minecheat.Modules.Player;

public class Add300XpLevelFeature : PlayerBasedFeature
{
    public override string FeatureTitle => "增加300经验等级";

    public override string Tooltip => "附魔！";

    protected override void EditPlayerTag(SaveInfo saveInfo, NbtCompound playerTag, Action<string> appendLog)
    {
        var levelTag = playerTag.Get<NbtInt>("XpLevel")
            ?? throw new InvalidDataException("无法获取玩家经验等级");

        appendLog($"当前经验等级: {levelTag.Value}");
        levelTag.Value += 300;
        appendLog($"修改后经验等级: {levelTag.Value}");
    }
}
