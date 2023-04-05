namespace SideScrollGame;

public partial class Test : Node2D
{
    private GPath Path { get; set; }

    public override void _Ready()
    {
        var gearNodes = GetNode("Levels").GetChildren<LevelGear>();
        var points = gearNodes.Select(x => x.Position).ToArray();
        Path = new GPath(points, Colors.White, 2, 8);
        Path.AddCurves();

        var sprite = GetNode<Sprite2D>("Sprite2D");
        RemoveChild(sprite);

        Path.AddSprite(sprite);

        AddChild(Path);

        foreach (var gearNode in gearNodes)
            gearNode.LevelPressed += (level) => Path.AnimateTo(level - 1);
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
}
