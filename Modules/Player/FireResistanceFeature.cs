using fNbt;
using System.IO;

namespace Minecheat.Modules.Player;

public class FireResistanceFeature : PlayerBasedFeature
{
    public override string FeatureTitle => "火焰抗性";

    public override string Tooltip => "天干物燥，小心火烛~";

    private static readonly TimeSpan effectDuration = TimeSpan.FromHours(1);

    protected override void EditPlayerTag(SaveInfo saveInfo, NbtCompound playerTag, Action<string> appendLog)
    {

        appendLog("正在应用火焰抗性效果...");

        var activeEffectsTag = playerTag.GetOrAdd<NbtList>(
            "active_effects", _ => new NbtList("active_effects", NbtTagType.Compound));

        var effectTag = activeEffectsTag
            .OfType<NbtCompound>()
            .Where(x => x.Get<NbtString>("id")?.Value == "fire_resistance")
            .FirstOrDefault();

        if (effectTag != null)
        {
            var durationTag = effectTag["duration"] as NbtInt
                    ?? throw new InvalidDataException("无法获取火焰抗性效果的持续时间。");
            appendLog($"已有火焰抗性效果，剩余时间：{GameTickTimeConverter.ConvertTicksToTimeSpan(durationTag.Value)}");

            durationTag.Value += GameTickTimeConverter.ConvertTimeSpanToTicks(effectDuration);
            appendLog($"新火焰抗性效果剩余时间：{GameTickTimeConverter.ConvertTicksToTimeSpan(durationTag.Value)}");
        }
        else
        {
            var fireResistanceEffectTag = new NbtCompound
            {
                new NbtString("id", "fire_resistance"), // Fire Resistance effect ID
                new NbtInt("duration", GameTickTimeConverter.ConvertTimeSpanToTicks(System.TimeSpan.FromHours(1))), // 5 minutes
                new NbtByte("show_icon", 1),
            };
            activeEffectsTag.Add(fireResistanceEffectTag);
            appendLog($"已应用火焰抗性效果，剩余时间：{effectDuration}");
        }
    }
}
