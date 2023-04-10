namespace SideScrollGame;

public partial class Base : Sprite2D, IDamageable
{
    [Export] public Team Team { get; set; }
    [Export] public double MaxHealth { get; set; }

    [Signal] public delegate void DestroyedEventHandler();

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

            EmitSignal(SignalName.Destroyed);

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
    private int           AnimateTime       { get; } = 300;
    private int           AnimateAmplitudeX { get; } = 1;
    private int           AnimateAmplitudeY { get; } = 1;
    private bool          BaseDestroyed     { get; set; }

    public override void _Ready()
    {
        SetPhysicsProcess(false);
        AnimateTimer = new GTimer(this, () =>
        {
            if (!BaseDestroyed)
                SetPhysicsProcess(false);
        }, AnimateTime);
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

    public void SpawnUnit(Entity entity)
    {
        entity.Team = Team;

        var spawnOffsetFromBase = 90;
        var maxRandomY = 25;
        var randomY = GD.RandRange(0, maxRandomY);

        var offset = entity.Team == Team.Left ? 
            spawnOffsetFromBase : -spawnOffsetFromBase;

        entity.Position = new Vector2(offset, randomY);
        AddChild(entity);
    }
}
