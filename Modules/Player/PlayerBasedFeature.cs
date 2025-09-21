using fNbt;
using System.IO;

namespace Minecheat.Modules.Player;

public abstract class PlayerBasedFeature : LevelDataBasedFeature
{
    protected abstract void EditPlayerTag(SaveInfo saveInfo, NbtCompound playerTag, Action<string> appendLog);

    protected override void EditLevelDataNbt(SaveInfo saveInfo, NbtFile levelNbt, Action<string> appendLog)
    {
        var playerTag = levelNbt.RootTag.Get<NbtCompound>("Data")
            ?.Get<NbtCompound>("Player")
            ?? throw new InvalidDataException("无法获取玩家数据。");
        this.EditPlayerTag(saveInfo, playerTag, appendLog);
    }
}
