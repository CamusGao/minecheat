namespace Minecheat;

public interface IFeature
{
    string FeatureTitle { get; }

    string Tooltip { get; }

    void Invoke(SaveInfo saveInfo, Action<string> appendLog);
}
