namespace SideScrollGame;

public partial class Test : Node2D
{
    private GPath Path { get; set; }

    public override void _Ready()
    {
        var points = GetNode("Levels").GetChildren<Node2D>().Select(x => x.Position).ToArray();
        Path = new GPath(points, Colors.White);
        Path.AddCurves();

        var sprite = GetNode<Sprite2D>("Sprite2D");
        RemoveChild(sprite);

        Path.AddSprite(sprite);

        AddChild(Path);
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
