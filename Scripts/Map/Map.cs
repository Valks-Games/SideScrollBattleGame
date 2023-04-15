namespace SideScrollGame;

public partial class Map : Node2D
{
    private GPath Path { get; set; }
    private LevelIcon[] LevelIcons { get; set; }

    public override void _Ready()
    {
        LevelIcons = GetNode("Levels").GetChildren<LevelIcon>();
        var points = LevelIcons.Select(x => x.Position).ToArray();
        Path = new GPath(points, Colors.Black, 2, 8);
        Path.AddCurves();

        var sprite = GetNode<Sprite2D>("Sprite2D");
        RemoveChild(sprite);

        Path.AddSprite(sprite);

        AddChild(Path);

        foreach (var gearNode in LevelIcons)
            gearNode.LevelPressed += level => Path.AnimateTo(level - 1);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey inputEventKey)
        {
            if (inputEventKey.IsKeyJustPressed(Key.D))
            {
                var index = Path.AnimateForwards();
                LevelIcons[index].AnimateColor();
            }

            if (inputEventKey.IsKeyJustPressed(Key.A))
            {
                var index = Path.AnimateBackwards();
                LevelIcons[index].AnimateColor();
            }
        }
    }
}
