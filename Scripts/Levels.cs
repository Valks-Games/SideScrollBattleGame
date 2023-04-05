namespace SideScrollGame;

public partial class Levels : Node2D
{
    private Path2D Path2D { get; set; }
    private PathFollow2D PathFollow2D { get; set; }
    private float[] TValues { get; set; }
    private int TIndex { get; set; }
    private Vector2[] LevelIconPositions { get; set; }
    private Tween Tween { get; set; }
    private Tween.TransitionType TransType { get; } = Tween.TransitionType.Sine;
    private Tween.EaseType EaseType { get; } = Tween.EaseType.Out;
    private double Duration { get; } = 1.5;

    public override void _Ready()
    {
        Path2D = GetNode<Path2D>("../Path2D");
        PathFollow2D = Path2D.GetNode<PathFollow2D>("PathFollow2D");

        var slider = new UISlider(new SliderOptions
        {
            HSlider = new HSlider
            {
                Step = 0.01,
                MaxValue = 1
            }
        });
        slider.ValueChanged += v => PathFollow2D.ProgressRatio = v;

        GetNode<CanvasLayer>("../CanvasLayer").AddChild(slider);

        LevelIconPositions = this.GetChildren<Node2D>().Select(x => x.Position).ToArray();

        var count = LevelIconPositions.Count();

        TValues = new float[count];

        for (int i = 0; i < count; i++)
            TValues[i] = (float)i / (count - 1);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey inputEventKey)
        {
            if (inputEventKey.IsKeyJustPressed(Key.D))
            {
                TIndex = Mathf.Min(TIndex + 1, TValues.Count() - 1);

                Tween?.Kill();
                Tween = PathFollow2D.CreateTween();
                Tween.TweenProperty(PathFollow2D, "progress_ratio", TValues[TIndex], Duration)
                    .SetTrans(TransType)
                    .SetEase(EaseType);
            }

            if (inputEventKey.IsKeyJustPressed(Key.A))
            {
                TIndex = Mathf.Max(TIndex - 1, 0);

                Tween?.Kill();
                Tween = PathFollow2D.CreateTween();
                Tween.TweenProperty(PathFollow2D, "progress_ratio", TValues[TIndex], Duration)
                    .SetTrans(TransType)
                    .SetEase(EaseType);
            }
        }
    }

    public override void _Draw()
    {
        for (int i = 0; i < LevelIconPositions.Count() - 1; i++)
        {
            var pointA = LevelIconPositions[i];
            var pointB = LevelIconPositions[i + 1];

            var c1 = new Vector2(-299, 102);
            var c2 = new Vector2( 243, 198);

            var points = BezierCurve.Draw(this, pointA, pointB, c1, c2, Colors.White);
            foreach (var point in points)
                Path2D.Curve.AddPoint(point);
        }
    }
}
