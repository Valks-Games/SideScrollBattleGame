namespace SideScrollGame;

public abstract partial class Entity : Node2D
{
    public abstract Team MyTeam { get; }

    public virtual void Init() { }
    public virtual void Update() { }

    public override void _Ready()
    {
        AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        AnimationPlayer = AnimatedSprite.GetNode<AnimationPlayer>("AnimationPlayer");

        // Set the other team
        OtherTeam = MyTeam == Team.Left ? Team.Right : Team.Left;

        // Play the 'move' animation set at a random starting frame
        if (AnimatedSprite.SpriteFrames.HasAnimation(AnimMoveName))
            AnimatedSprite.PlayRandom(AnimMoveName);

        // Create the Area2D for this sprite. All other areas will try to detect this area
        var spriteSize = AnimatedSprite.GetSize(AnimMoveName);
        CreateBodyArea(spriteSize);
        CreateDetectionArea(spriteSize);

        State = State.Moving;

        Init();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (State == State.Moving)
        {
            if (!FoundEnemy)
                Position += MyTeam == Team.Left ?
                    new Vector2(1, 0) : new Vector2(-1, 0);
            else
            {
                State = State.Attack;
            }
        }
        else if (State == State.Attack)
        {
            AnimationPlayer.Play("attack");
        }
        else if (State == State.Cooldown)
        {

        }
        
        Update();
    }

    private AnimatedSprite2D AnimatedSprite { get; set; }
    private AnimationPlayer AnimationPlayer { get; set; }
    private State State { get; set; }
    private Team OtherTeam { get; set; }
    private string AnimIdleName { get; } = "idle";
    private string AnimMoveName { get; } = "move";
    private bool FoundEnemy { get; set; }

    private void CreateBodyArea(Vector2 spriteSize)
    {
        var area = new Area2D();
        area.AddToGroup(MyTeam.ToString());
        var collisionShape = new CollisionShape2D
        {
            Shape = new RectangleShape2D
            {
                Size = spriteSize
            }
        };

        area.AddChild(collisionShape);
        AddChild(area);
    }

    private void CreateDetectionArea(Vector2 spriteSize)
    {
        var detectionWidth = 10;
        var detectionHeight = 100;

        var detectionPos = 0f;
        var offset = spriteSize.X / 2 + detectionWidth / 2;

        detectionPos = MyTeam == Team.Left ? offset : -offset;

        var area = new Area2D();
        var collisionShape = new CollisionShape2D
        {
            Position = new Vector2(detectionPos, 0),
            Shape = new RectangleShape2D
            {
                Size = new Vector2(detectionWidth, detectionHeight)
            }
        };

        area.AreaEntered += (otherArea) =>
        {
            if (otherArea.IsInGroup(OtherTeam.ToString()))
            {
                AnimatedSprite.Play(AnimIdleName);
                FoundEnemy = true;
            }
        };

        area.AddChild(collisionShape);
        AddChild(area);
    }
}

public enum State
{
    Moving,
    Attack,
    Cooldown
}

public enum Team
{
    Left,
    Right
}
