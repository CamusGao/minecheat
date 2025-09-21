namespace Minecheat;

public interface IFeatureModule
{
    string ModuleTitle { get; }

    IReadOnlyList<IFeature> Features { get; }
}
