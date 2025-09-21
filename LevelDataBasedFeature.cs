
using fNbt;
using System.IO;

namespace Minecheat;

public abstract class LevelDataBasedFeature : IFeature
{
    public abstract string FeatureTitle { get; }

    public abstract string Tooltip { get; }

    public void Invoke(SaveInfo saveInfo, Action<string> appendLog)
    {
        var levelDataPath = Path.Combine(saveInfo.Path, "level.dat");

        var levelNbt = new NbtFile();
        appendLog("加载 level.dat");
        levelNbt.LoadFromFile(levelDataPath);

        this.EditLevelDataNbt(saveInfo, levelNbt, appendLog);

        appendLog("保存 level.dat");
        levelNbt.SaveToFile(levelDataPath, levelNbt.FileCompression);
    }

    protected abstract void EditLevelDataNbt(SaveInfo saveInfo, NbtFile levelNbt, Action<string> appendLog);
}
