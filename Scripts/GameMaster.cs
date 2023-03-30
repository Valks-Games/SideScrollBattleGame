namespace SideScrollGame;

public static class Prefabs
{
    public static PackedScene Skeleton { get; } = LoadPrefab("skeleton");
    public static PackedScene OrangeBall { get; } = LoadPrefab("orange_ball");

    private static PackedScene LoadPrefab(string path) =>
        GD.Load<PackedScene>($"res://Scenes/Prefabs/{path}.tscn");
}

public partial class GameMaster : Node
{
    private Node   PlayerUnits { get; set; }
    private Node   EnemyUnits  { get; set; }
    private Node2D PlayerBase  { get; set; }
    private Node2D EnemyBase   { get; set; }

    public override void _Ready()
    {
        PlayerUnits = GetNode("Player Units");
        EnemyUnits  = GetNode("Enemy Units");
        PlayerBase  = GetNode<Node2D>("Player Base");
        EnemyBase   = GetNode<Node2D>("Enemy Base");

        var spawnOffsetFromBase = 90;
        var maxRandomY = 50;

        // player units
        for (int i = 0; i < 10; i++)
        {
            var unit = Prefabs.OrangeBall.Instantiate<RollingBall>();

            var randomY = GD.RandRange(0, maxRandomY);

            unit.Position = PlayerBase.Position + 
                new Vector2(spawnOffsetFromBase, randomY);

            PlayerUnits.AddChild(unit);
        }

        // enemy units
        for (int i = 0; i < 10; i++)
        {
            var unit = Prefabs.Skeleton.Instantiate<Skeleton>();

            var randomY = GD.RandRange(0, maxRandomY);

            unit.Position = EnemyBase.Position + 
                new Vector2(-spawnOffsetFromBase, randomY);

            EnemyUnits.AddChild(unit);
        }
    }
}
