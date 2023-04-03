namespace SideScrollGame;

public partial class Base : Sprite2D, IDamageable
{
    [Export] public Team Team { get; set; }
    [Export] public double MaxHealth { get; set; }

    public bool Destroyed => GodotObject.IsInstanceValid(this);

    public double CurHealth
    {
        get => double.Parse(LabelCurHealth.Text);
        set
        {
            if (value <= 0)
            {
                // base destroyed!


                if (LabelMatchResult != null)
                {
                    // match end
                    // skip the rest
                    return;
                }

                switch (Team)
                {
                    case Team.Left:
                        // player lose
                        LabelMatchResult = new Label {
                            Text = "Defeat..."
                        };
                        break;
                    
                    case Team.Right:
                        // enemy lose
                        LabelMatchResult = new Label {
                            Text = "Victory!"
                        };
                        break;
                    
                    default:
                        GD.PrintErr("Team text error");
                        break;
                }

                // adjust label size to big enough
                LabelMatchResult.AddThemeFontSizeOverride("font_size", 120);

                // put the label at center of the screen.
                Main.CanvasLayer.AddChild(LabelMatchResult);
                LabelMatchResult.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.Center);

                // copy the original label posiition
                var originalPos = LabelMatchResult.Position;

                // move the label position to offscreen
                LabelMatchResult.Position = new Vector2(-800, originalPos.Y);

                // tween the offscreen label to the original label posiition
                var tween = LabelMatchResult.CreateTween();
                tween.TweenProperty(LabelMatchResult, "position", originalPos, 1.0f);
                

                // animate base forever
                HBox.Hide();
                SetPhysicsProcess(true);
                return;
            }

            SetPhysicsProcess(true);
            AnimateTimer.StartMs(AnimateTime);
            LabelCurHealth.Text = value + "";
        }
    }

    private GTimer        AnimateTimer      { get; set; }
    private HBoxContainer HBox              { get; set; }
    private Label         LabelMaxHealth    { get; set; }
    private Label         LabelCurHealth    { get; set; }
    private Label         LabelMatchResult  { get; set; }
    private int           AnimateTime       { get; } = 300;
    private int           AnimateAmplitudeX { get; } = 1;
    private int           AnimateAmplitudeY { get; } = 1;

    public override void _Ready()
    {
        SetPhysicsProcess(false);
        AnimateTimer = new GTimer(this, () => SetPhysicsProcess(false), AnimateTime);
        AddToGroup(Team.ToString());

        HBox = GetNode<HBoxContainer>("HBox");

        LabelMaxHealth = HBox.GetNode<Label>("MaxHealth");
        LabelCurHealth = HBox.GetNode<Label>("CurHealth");

        LabelMaxHealth.Text = MaxHealth + "";
        LabelCurHealth.Text = MaxHealth + "";
    }

    public override void _PhysicsProcess(double delta)
    {
        var randX = (float)GD.RandRange(-1d, 1d);
        var randY = (float)GD.RandRange(-1d, 1d);

        Offset = new Vector2(randX * AnimateAmplitudeX, randY * AnimateAmplitudeY);
    }
}
