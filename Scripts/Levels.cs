namespace SideScrollGame;

public partial class Levels : Node2D
{
    private Path2D Path2D { get; set; }
    private PathFollow2D PathFollow2D { get; set; }
    private float[] TweenValues { get; set; }
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

        TweenValues = new float[count];

        // Add a point for each level icon position
        for (int i = 0; i < count; i++)
            Path2D.Curve.AddPoint(LevelIconPositions[i]);

        // Add aditional points to make each line be curved
        var invert = 1;

        for (int i = 0; i < count - 1; i++)
        {
            var A = LevelIconPositions[i];
            var B = LevelIconPositions[i + 1];

            var center = (A + B) / 2;
            var curveDistance = 50; // How far the curve is pushed out
            var offset = ((B - A).Orthogonal().Normalized() * curveDistance * invert);
            var newPos = center + offset;

            // Switch between sides so curves flow more naturally
            invert *= -1;

            Vector4 v;

            // These values were found through trial and error
            // If you see a simpler pattern than this, please tell me lol
            if (B.Y >= A.Y)
                if (B.X >= A.X)
                    // Next point is under and after first point
                    v = new Vector4(-1, -1, 1, 1);
                else
                    // Next point is under and before first point
                    v = new Vector4(1, -1, -1, 1);
            else
                if (B.X <= A.X)
                    // Next point is over and before first point
                    v = new Vector4(1, 1, -1, -1);
                else
                    // Next point is over and after first point
                    v = new Vector4(-1, 1, 1, -1);

            var curveSize = 50; // How big the curve is
            var index = 1 + i * 2;

            // Insert the curved point at the index in the curve
            Path2D.Curve.AddPoint(newPos, 
                new Vector2(v.X, v.Y) * curveSize, 
                new Vector2(v.Z, v.W) * curveSize, index);
        }

        // Calculate all the progress values for Tween animations
        for (int i = 0; i < count; i++)
            TweenValues[i] = Path2D.Curve.GetClosestOffset(LevelIconPositions[i]);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey inputEventKey)
        {
            if (inputEventKey.IsKeyJustPressed(Key.D))
            {
                TIndex = Mathf.Min(TIndex + 1, TweenValues.Count() - 1);

                Tween?.Kill();
                Tween = PathFollow2D.CreateTween();
                Tween.TweenProperty(PathFollow2D, "progress", TweenValues[TIndex], Duration)
                    .SetTrans(TransType)
                    .SetEase(EaseType);
            }

            if (inputEventKey.IsKeyJustPressed(Key.A))
            {
                TIndex = Mathf.Max(TIndex - 1, 0);

                Tween?.Kill();
                Tween = PathFollow2D.CreateTween();
                Tween.TweenProperty(PathFollow2D, "progress", TweenValues[TIndex], Duration)
                    .SetTrans(TransType)
                    .SetEase(EaseType);
            }
        }
    }

    public override void _Draw()
    {
        
    }
}
