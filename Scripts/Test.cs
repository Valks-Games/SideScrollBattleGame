using static Godot.Animation;
using static Godot.Tween;

namespace SideScrollGame;

public partial class Test : Node
{
    private GPath Path { get; set; }

    public override void _Ready()
    {
        var points = GetNode("Levels").GetChildren<Node2D>().Select(x => x.Position).ToArray();
        Path = new GPath(this, points);
        Path.AddCurves();

        var sprite = GetNode<Sprite2D>("Sprite2D");
        RemoveChild(sprite);

        Path.AddSprite(sprite);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey inputEventKey)
        {
            if (inputEventKey.IsKeyJustPressed(Key.D))
                Path.AnimateNext();

            if (inputEventKey.IsKeyJustPressed(Key.A))
                Path.AnimatePrev();
        }
    }
}
