using fNbt;
using System.IO;

namespace Minecheat.Modules.Player;

public class MoveToRespawnPosFeature : LevelDataBasedFeature
{
    public override string FeatureTitle => "移动到重生点";

    public override string Tooltip => "移动玩家到重生点（上次绑定床的位置）";

    protected override void EditLevelDataNbt(SaveInfo saveInfo, NbtFile levelNbt, Action<string> appendLog)
    {
        var dataTag = levelNbt.RootTag.Get<NbtCompound>("Data")!;
        var playerTag = dataTag.Get<NbtCompound>("Player")
            ?? throw new InvalidDataException("无法获取玩家数据。");
        var dimensionTag = playerTag.Get<NbtString>("Dimension")
            ?? throw new InvalidDataException("无法获取玩家维度信息");

        var posTag = playerTag.Get<NbtList>("Pos")
            ?? throw new InvalidDataException("无法获取玩家位置");
        var xPosTag = posTag.Get<NbtDouble>(0);
        var yPosTag = posTag.Get<NbtDouble>(1);
        var zPosTag = posTag.Get<NbtDouble>(2);

        appendLog($"当前位于【{dimensionTag.Value}】：X: {xPosTag.Value:N1}, Y: {yPosTag.Value:N1}, Z: {zPosTag.Value:N1}");

        dimensionTag.Value = "minecraft:overworld";

        var respawnPosTag = playerTag.Get<NbtCompound>("respawn")
            ?.Get<NbtIntArray>("pos");
        xPosTag.Value = respawnPosTag?[0] ?? dataTag.Get<NbtInt>("SpawnX")?.Value
            ?? throw new InvalidDataException("无法获取重生点");
        yPosTag.Value = respawnPosTag?[1] ?? dataTag.Get<NbtInt>("SpawnY")?.Value
            ?? throw new InvalidDataException("无法获取重生点"); ;
        zPosTag.Value = respawnPosTag?[2] ?? dataTag.Get<NbtInt>("SpawnZ")?.Value
            ?? throw new InvalidDataException("无法获取重生点"); ;

        appendLog($"""
            已经移动到重生点。
            当前位于【{dimensionTag.Value}】：X: {xPosTag.Value:N1}, Y: {yPosTag.Value:N1}, Z: {zPosTag.Value:N1}
            """);
    }
}
