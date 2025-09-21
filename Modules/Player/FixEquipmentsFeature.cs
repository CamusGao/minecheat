using fNbt;
using System.IO;

namespace Minecheat.Modules.Player;

public class FixEquipmentsFeature : PlayerBasedFeature
{
    private static readonly string TAG_NAME_DAMAGE = "minecraft:damage";

    public override string FeatureTitle => "修复装备耐久";

    public override string Tooltip => "修复所有装备和工具的耐久度";

    protected override void EditPlayerTag(SaveInfo saveInfo, NbtCompound playerTag, Action<string> appendLog)
    {
        appendLog("正在修复装备");
        var equipmentsTag = playerTag.Get<NbtCompound>("equipment");
        foreach (var itemTag in equipmentsTag?.OfType<NbtCompound>() ?? [])
        {
            this.Fix(itemTag);
        }

        appendLog("正在修复背包物品");
        var inventoryTag = playerTag.Get<NbtList>("Inventory");
        foreach (var itemTag in inventoryTag?.OfType<NbtCompound>() ?? [])
        {
            this.Fix(itemTag);
        }
        appendLog("修复完成");
    }

    private void Fix(NbtCompound itemTag)
    {
        var itemComponentTag = itemTag.Get<NbtCompound>("components");
        if (itemComponentTag == null || itemComponentTag.Names.All(x => x != TAG_NAME_DAMAGE))
        {
            return;
        }

        itemComponentTag.Remove(TAG_NAME_DAMAGE);
    }
}
