namespace SideScrollGame;

public partial class Entity : Node2D
{
    [Export] public double MaxHealth              { get; set; } = 100;
    [Export] public float  MoveSpeed              { get; set; } = 1;
    [Export] public int    AttackPower            { get; set; } = 10;
    [Export] public float  DetectionRange         { get; set; } = 10;
    [Export] public int    AttackCooldownDuration { get; set; } = 1000; // in ms
    [Export] public string AnimationAttackType    { get; set; } = "attack";

    public Team MyTeam { get; set; } = Team.Left;
    public double CurHealth 
    { 
        get => HealthBar.Value;
        set 
        {
            if (value <= 0)
            {
                QueueFree();
                return;
            }

            HealthBar.Value = value;
        }
    }

    public override void _Ready()
    {
        AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        AnimationPlayer = AnimatedSprite.GetNode<AnimationPlayer>("AnimationPlayer");
        TimerAttackCooldown = new GTimer(
            this, 
            () => State = State.Find, 
            AttackCooldownDuration);

        AnimationPlayer.AnimationFinished += (anim) =>
        {
            if (anim == AnimationAttackType)
            {
                State = State.Cooldown;
                TimerAttackCooldown.Start();
            }
        };

        if (MyTeam == Team.Left)
        {
            OtherTeam = Team.Right;

            // All sprites face the right side by default
        }
        else
        {
            OtherTeam = Team.Left;

            // Flip the root node to face the left side
            Scale = new Vector2(Scale.X * -1, Scale.Y);
        }

        // Play the 'move' animation set at a random starting frame
        if (AnimatedSprite.SpriteFrames.HasAnimation("move"))
            AnimatedSprite.PlayRandom("move");

        // Create the Area2D for this sprite. All other areas will try to detect this area
        SpriteSize = AnimatedSprite.GetSize("move");
        CreateBodyArea();
        CreateDetectionArea();
        CreateHealthBar();

        State = State.Moving;
    }

    public override void _PhysicsProcess(double delta)
    {
        switch (State)
        {
            case State.Moving:
                if (!FoundEnemy)
                {
                    AnimatedSprite.Play("move");

                    Position += MyTeam == Team.Left ?
                        new Vector2(MoveSpeed, 0) : new Vector2(-MoveSpeed, 0);
                }
                else
                {
                    State = State.Attack;
                }
                break;
            case State.Attack:
                AnimationPlayer.Play(AnimationAttackType);
                break;
            case State.Find:
                FoundEnemy = false;

                // this looks ugly to me
                State = State.Moving;

                if (DetectedEnemies.Count > 0)
                {
                    FoundEnemy = true;
                    State = State.Attack;
                }
                break;
            default:
                break;
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("view_health"))
            HealthBar.Show();

        if (Input.IsActionJustReleased("view_health"))
            HealthBar.Hide();
    }

    private void OnHit()
    {
        foreach (var entity in DetectedEnemies)
        {
            entity.CurHealth -= AttackPower;
            break;
        }
    }

    private AnimatedSprite2D   AnimatedSprite      { get; set; }
    private AnimationPlayer    AnimationPlayer     { get; set; }
    private GTimer             TimerAttackCooldown { get; set; }
    private Area2D             DetectionArea       { get; set; }
    private TextureProgressBar HealthBar           { get; set; }
    private Vector2            SpriteSize          { get; set; }
    private State              State               { get; set; }
    private Team               OtherTeam           { get; set; }
    private bool               FoundEnemy          { get; set; }
    private List<Entity>       DetectedEnemies     { get; set; } = new();

    private void CreateBodyArea()
    {
        var area = new Area2D();
        area.AddToGroup(MyTeam.ToString());
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
            if (otherArea.IsInGroup(OtherTeam.ToString()))
            {
                DetectedEnemies.Add(otherArea.GetParent<Entity>());
                AnimatedSprite.Play("idle");
                FoundEnemy = true;
            }
        };

        DetectionArea.AreaExited += (otherArea) =>
        {
            if (otherArea.IsInGroup(OtherTeam.ToString()))
            {
                DetectedEnemies.Remove(otherArea.GetParent<Entity>());
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

public enum State
{
    Moving,
    Attack,
    Cooldown,
    Find
}

public enum Team
{
    Left,
    Right
}
