namespace SideScrollGame;

public partial class Entity : Node2D, IDamageable, IStateMachine<Entity>
{
    [Export] public Team         Team           { get; set; } = Team.Left;
    [Export] public double       MaxHealth      { get; set; } = 100;
    [Export] public float        MoveSpeed      { get; set; } = 1;
    [Export] public Attack       AttackType     { get; set; }
    [Export] public int          AttackPower    { get; set; } = 10;
    [Export] public int          AttackSpeed    { get; set; } = 1000; // in ms
    [Export] public float        DetectionRange { get; set; } = 10;
    [Export] public SpriteFrames SpriteFrames   { get; set; }

    // Implement IStateMachine
    public Dictionary<object, State<Entity>> States { get; set; } = new();
    public object CurrentState { get; set; }

    public EntityAnimationAttack AnimationAttack { get; set; }
    public AnimatedSprite2D      AnimatedSprite  { get; set; }
    public Area2D                DetectionArea   { get; set; }
    public TextureProgressBar    HealthBar       { get; set; }
    public Vector2               SpriteSize      { get; set; }
    public Team                  OtherTeam       { get; set; }
    public Tween                 AttackTween     { get; set; }
    public bool                  Attacking       { get; set; }

    public double CurHealth 
    { 
        get => HealthBar.Value;
        set 
        {
            if (value <= 0)
            {
                AttackTween?.Kill();
                QueueFree();
                return;
            }

            HealthBar.Value = value;
        }
    }

    public override void _Ready()
    {
        // Populate states
        States[EntityStateType.Attack]   = new EntityStateAttack(this);
        States[EntityStateType.Cooldown] = new EntityStateCooldown(this);
        States[EntityStateType.Move]     = new EntityStateMove(this);

        // Set current state
        CurrentState = EntityStateType.Move;

        AddToGroup(Team.ToString());

        AnimatedSprite = new AnimatedSprite2D
        {
            SpriteFrames = SpriteFrames
        };
        AddChild(AnimatedSprite);

        if (Team == Team.Left)
        {
            OtherTeam = Team.Right;
        }
        else
        {
            OtherTeam = Team.Left;

            // Flip the root node to face the left side
            Scale = new Vector2(Scale.X * -1, Scale.Y);
        }

        // Assuming every entity will have an animation called "move"
        // Play the 'move' animation set at a random starting frame
        if (AnimatedSprite.SpriteFrames.HasAnimation("move"))
            AnimatedSprite.InstantPlay("move");

        // Create the Area2D for this sprite. All other areas will try to detect this area
        SpriteSize = AnimatedSprite.GetSize("move");
        CreateBodyArea();
        CreateDetectionArea();
        CreateHealthBar();

        AnimationAttack = new EntityAnimationAttack(this);

        // Enter current state
        States[CurrentState].EnterState();
    }

    public override void _PhysicsProcess(double delta)
    {
        States[CurrentState].Update();
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("view_health"))
            HealthBar.Show();

        if (Input.IsActionJustReleased("view_health"))
            HealthBar.Hide();
    }

    public int GetEnemyCount()
    {
        var count = 0;

        foreach (var area in DetectionArea.GetOverlappingAreas())
            if (area.GetParent() is IDamageable entity && entity.Team == OtherTeam)
                count++;

        return count;
    }

    public void Attack()
    {
        foreach (var area in DetectionArea.GetOverlappingAreas())
            if (area.GetParent() is IDamageable entity && entity.Team == OtherTeam)
            {
                entity.CurHealth -= AttackPower;
                break; // Single Attack
            }
    }

    private void CreateBodyArea()
    {
        var area = new Area2D();
        var collisionShape = new CollisionShape2D
        {
            Shape = new RectangleShape2D
            {
                Size = SpriteSize
            }
        };

        area.AddChild(collisionShape);
        AddChild(area);
    }

    private void CreateDetectionArea()
    {
        var detectionHeight = 100;

        var detectionPos = SpriteSize.X / 2 + DetectionRange / 2;

        DetectionArea = new Area2D();
        var collisionShape = new CollisionShape2D
        {
            Position = new Vector2(detectionPos, 0),
            Shape = new RectangleShape2D
            {
                Size = new Vector2(DetectionRange, detectionHeight)
            }
        };

        DetectionArea.AreaEntered += (otherArea) =>
        {
            if (otherArea.GetParent() is IDamageable entity && entity.Team == OtherTeam)
            {
                if (!Attacking)
                    States[CurrentState].SwitchState(EntityStateType.Attack);
            }
        };

        DetectionArea.AddChild(collisionShape);
        AddChild(DetectionArea);
    }

    private void CreateHealthBar()
    {
        HealthBar = Prefabs.HealthBar.Instantiate<TextureProgressBar>();
        HealthBar.MaxValue = MaxHealth;
        HealthBar.Value = MaxHealth;
        HealthBar.Position = new Vector2(-HealthBar.Size.X / 2, -SpriteSize.Y / 2 - 3);
        HealthBar.Hide();
        AddChild(HealthBar);
    }
}

public enum EntityStateType
{
    Attack,
    Cooldown,
    Move
}

public enum Team
{
    Left,
    Right
}
