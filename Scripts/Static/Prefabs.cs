namespace SideScrollGame;

public static class Prefabs
{
    public static PackedScene HealthBar { get; } = LoadPrefab("health_bar");
    public static PackedScene Base      { get; } = LoadPrefab("base");

    private static PackedScene LoadPrefab(string path) =>
        GD.Load<PackedScene>($"res://Scenes/Prefabs/{path}.tscn");
}
