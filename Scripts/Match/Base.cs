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
            if (value > 0)
            {
                SetPhysicsProcess(true);
                AnimateTimer.StartMs(AnimateTime);
                LabelCurHealth.Text = value + "";
                return;
            }

            // Base destroyed!
            if (BaseDestroyed)
                return;

            GameMaster.DestroyAllUnits(Team);

            if (Team == Team.Left)
                LabelMatchResult = new GLabel("Defeat...");
            else
                LabelMatchResult = new GLabel("Victory!");

            LabelMatchResult.SetFontSize(120);

            // Add the label to the center of the screen
            Main.CanvasLayer.AddChild(LabelMatchResult);
            LabelMatchResult.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.Center);

            // Keep track of center position
            var originalPos = LabelMatchResult.Position;

            // Set position to be offscreen
            LabelMatchResult.Position = new Vector2(
                (Team == Team.Left ? 1 : -1) * GWindow.GetWidth() / 2, 
                originalPos.Y);

            // Tween the offscreen label to its original posiition
            // Tween Transitions Cheat Sheet: https://www.reddit.com/r/godot/comments/dgh9vd/transitiontype_cheat_sheet_tween_interpolation_oc/
            var tween = LabelMatchResult.CreateTween();
            tween.TweenProperty(LabelMatchResult, "position", originalPos, 1.0f)
                .SetTrans(Tween.TransitionType.Quint)
                .SetEase(Tween.EaseType.Out);

            tween.TweenCallback(Callable.From(() =>
            {
                var blackScreen = new ColorRect {
                    Color = new Color(0, 0, 0, 0)
                };

                Main.CanvasLayer.AddChild(blackScreen);
                blackScreen.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);

                var tween = blackScreen.CreateTween();

                tween.TweenProperty(blackScreen, "color:a", 1, 4)
                    .SetTrans(Tween.TransitionType.Linear)
                    .SetDelay(2.0);

                tween.TweenCallback(Callable.From(() =>
                {
                    GetNode<Global>("/root/Global").SwitchScene(Scene.Map);
                }));
            }));

            // Animate the base forever
            HBox.Hide();
            SetPhysicsProcess(true);
            BaseDestroyed = true;
        }
    }

    private GTimer        AnimateTimer      { get; set; }
    private HBoxContainer HBox              { get; set; }
    private Label         LabelMaxHealth    { get; set; }
    private Label         LabelCurHealth    { get; set; }
    private GLabel        LabelMatchResult  { get; set; }
    private int           AnimateTime       { get; } = 300;
    private int           AnimateAmplitudeX { get; } = 1;
    private int           AnimateAmplitudeY { get; } = 1;
    private bool          BaseDestroyed     { get; set; }

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
