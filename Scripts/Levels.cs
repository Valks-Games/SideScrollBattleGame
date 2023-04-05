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
        // Retrieve Path2D and PathFollow2D nodes
        Path2D = GetNode<Path2D>("../Path2D");
        PathFollow2D = Path2D.GetNode<PathFollow2D>("PathFollow2D");

        // Retrieve all level icon (gear) positions
        LevelIconPositions = this.GetChildren<Node2D>().Select(x => x.Position).ToArray();

        var count = LevelIconPositions.Count();

        TValues = new float[count];

        // Add a point for each level icon position
        for (int i = 0; i < count; i++)
            Path2D.Curve.AddPoint(LevelIconPositions[i]);

        // Add aditional point to make the first line be curved
        var center = (LevelIconPositions[0] + LevelIconPositions[1]) / 2;
        var offset = center + ((LevelIconPositions[1] - LevelIconPositions[0]).Orthogonal().Normalized() * 50);

        Path2D.Curve.AddPoint(offset,
            new Vector2(-50, -50), new Vector2(50, 50), 1);

        // Calculate all the progress values for Tween animations
        for (int i = 0; i < count; i++)
        {
            TValues[i] = Path2D.Curve.GetClosestOffset(LevelIconPositions[i]);
        }
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
                Tween.TweenProperty(PathFollow2D, "progress", TValues[TIndex], Duration)
                    .SetTrans(TransType)
                    .SetEase(EaseType);
            }

            if (inputEventKey.IsKeyJustPressed(Key.A))
            {
                TIndex = Mathf.Max(TIndex - 1, 0);

                Tween?.Kill();
                Tween = PathFollow2D.CreateTween();
                Tween.TweenProperty(PathFollow2D, "progress", TValues[TIndex], Duration)
                    .SetTrans(TransType)
                    .SetEase(EaseType);
            }
        }
    }

    public override void _Draw()
    {
        
    }
}
