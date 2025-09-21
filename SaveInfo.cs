namespace Minecheat;

public class SaveInfo
{
    public required string LevelName { get; init; }
    public required string Path { get; init; }
    public required string Version { get; init; }

    public override string ToString()
    {
        return $"{this.LevelName} (${this.Version}) / {this.Path}";
    }
}
