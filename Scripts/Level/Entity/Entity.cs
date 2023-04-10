namespace SideScrollGame;

public partial class Entity : Node2D, IDamageable
{
    [Export] public Team         Team           { get; set; } = Team.Left;
    [Export] public double       MaxHealth      { get; set; } = 100;
    [Export] public float        MoveSpeed      { get; set; } = 1;
    [Export] public Attack       AttackType     { get; set; }
    [Export] public int          AttackPower    { get; set; } = 10;
    [Export] public int          AttackSpeed    { get; set; } = 1000; // in ms
    [Export] public float        DetectionRange { get; set; } = 10;
    [Export] public SpriteFrames SpriteFrames   { get; set; }

    public Dictionary<EntityStateType, EntityState<Entity>> States { get; set; } = new();
    public AnimatedSprite2D   AnimatedSprite      { get; set; }
    public Area2D             DetectionArea       { get; set; }
    public TextureProgressBar HealthBar           { get; set; }
    public Vector2            SpriteSize          { get; set; }
    public EntityStateType    CurrentState        { get; set; }
    public Team               OtherTeam           { get; set; }
    public Tween              AttackTween         { get; set; }
    public bool               Attacking           { get; set; }

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
        States[EntityStateType.Attack] = new EntityStateAttack(this);
        States[EntityStateType.Cooldown] = new EntityStateCooldown(this);
        States[EntityStateType.Move] = new EntityStateMove(this);

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

    /// <summary>
    /// Roll while staying still, then roll super fast forward
    /// </summary>
    public void SpinningAttack()
    {
        AttackTween = CreateTween();
        var rot = Mathf.Pi * 2;

        AttackTween.TweenProperty(AnimatedSprite, "rotation", rot, 0.5);

        rot += Mathf.Pi * 2 * 9;

        AttackTween.TweenProperty(AnimatedSprite, "rotation", rot, 0.5);
        AttackTween.Parallel().TweenProperty(
            AnimatedSprite, "position:x", DetectionRange, 0.5);

        AttackTween.TweenCallback(Callable.From(() =>
        {
            Attack();
        }));

        AttackTween.TweenProperty(AnimatedSprite,
            "position", new Vector2(9, -2), 0.1);
        AttackTween.TweenProperty(AnimatedSprite,
            "position", new Vector2(8, 0), 0.3);

        rot -= Mathf.Pi * 2 * 2;

        AttackTween.TweenProperty(AnimatedSprite, "rotation", rot, 0.8)
            .SetDelay(0.5);
        AttackTween.Parallel().TweenProperty(AnimatedSprite,
            "position", new Vector2(0, 0), 0.8)
            .SetDelay(0.5);

        AttackTween.TweenCallback(Callable.From(() =>
        {
            AnimatedSprite.Rotation = 0;
            States[CurrentState].SwitchState(EntityStateType.Cooldown);
        }));
    }

    /// <summary>
    /// Jump forward then rotate towards the enemy a bit
    /// </summary>
    public void SwordAttack()
    {
        AttackTween = CreateTween();
        var rot = Mathf.Pi * 0.05;

        AttackTween.TweenProperty(AnimatedSprite, "rotation", rot, 0.25);

        AttackTween.TweenProperty(
            AnimatedSprite, "position:y", -5, 0.125);
        AttackTween.Parallel().TweenProperty(
            AnimatedSprite, "position:x", 10, 0.25);

        AttackTween.TweenCallback(Callable.From(() =>
        {
            Attack();
        }));

        AttackTween.TweenProperty(AnimatedSprite,
            "position:y", 5, 0.5)
            .SetDelay(0.125);
        AttackTween.Parallel().TweenProperty(AnimatedSprite,
            "position:x", -10, 0.5)
            .SetDelay(0.25);

        rot -= Mathf.Pi * 0.05;

        AttackTween.Parallel().TweenProperty(AnimatedSprite, "rotation", rot, 0.5)
            .SetDelay(0.25);

        AttackTween.TweenCallback(Callable.From(() =>
        {
            AnimatedSprite.Rotation = 0;
            States[CurrentState].SwitchState(EntityStateType.Cooldown);
        }));
    }
}

public enum Team
{
    Left,
    Right
}
