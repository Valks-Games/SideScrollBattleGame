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
                QueueFree();
                return;
            }

            SetPhysicsProcess(true);
            AnimateTimer.StartMs(AnimateTime);
            LabelCurHealth.Text = value + "";
        }
    }

    private GTimer AnimateTimer      { get; set; }
    private Label  LabelMaxHealth    { get; set; }
    private Label  LabelCurHealth    { get; set; }
    private int    AnimateTime       { get; } = 300;
    private int    AnimateAmplitudeX { get; } = 1;
    private int    AnimateAmplitudeY { get; } = 1;

    public override void _Ready()
    {
        SetPhysicsProcess(false);
        AnimateTimer = new GTimer(this, () => SetPhysicsProcess(false), AnimateTime);
        AddToGroup(Team.ToString());

        var hbox = GetNode<HBoxContainer>("HBox");

        LabelMaxHealth = hbox.GetNode<Label>("MaxHealth");
        LabelCurHealth = hbox.GetNode<Label>("CurHealth");

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
