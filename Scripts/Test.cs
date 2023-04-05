namespace SideScrollGame;

public partial class Test : Node2D
{
    private GPath Path { get; set; }

    public override void _Ready()
    {
        var points = GetNode("Levels").GetChildren<Node2D>().Select(x => x.Position).ToArray();
        Path = new GPath(points, Colors.White, 2, 8);
        Path.AddCurves();

        var sprite = GetNode<Sprite2D>("Sprite2D");
        RemoveChild(sprite);

        Path.AddSprite(sprite);

        AddChild(Path);

        // connect custom signals at the gears
        var gears = GetNode("Levels").GetChildren<Node2D>();
        foreach(LevelGear node in gears)
        {
            node.LevelPressed += HandleGearLevelPressed;
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey inputEventKey)
        {
            if (inputEventKey.IsKeyJustPressed(Key.D))
                Path.AnimateForwards();

            if (inputEventKey.IsKeyJustPressed(Key.A))
                Path.AnimateBackwards();
        }
    }

    public void HandleGearLevelPressed(int level)
    {
        Path.AnimateSpecific(level - 1);
    }
}
