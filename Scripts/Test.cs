using static Godot.Animation;
using static Godot.Tween;

namespace SideScrollGame;

public partial class Test : Node
{
    private Tween Tween { get; set; }
    private int TIndex { get; set; }
    private Tween.TransitionType TransType { get; } = Tween.TransitionType.Sine;
    private Tween.EaseType EaseType { get; } = Tween.EaseType.Out;
    private double Duration { get; } = 1.5;

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
            {
                TIndex = Mathf.Min(TIndex + 1, Path.TweenValues.Count() - 1);

                Tween?.Kill();
                Tween = Path.PathFollow.CreateTween();
                Tween.TweenProperty(Path.PathFollow, "progress", Path.TweenValues[TIndex], Duration)
                    .SetTrans(TransType)
                    .SetEase(EaseType);
            }

            if (inputEventKey.IsKeyJustPressed(Key.A))
            {
                TIndex = Mathf.Max(TIndex - 1, 0);

                Tween?.Kill();
                Tween = Path.PathFollow.CreateTween();
                Tween.TweenProperty(Path.PathFollow, "progress", Path.TweenValues[TIndex], Duration)
                    .SetTrans(TransType)
                    .SetEase(EaseType);
            }
        }
    }
}
