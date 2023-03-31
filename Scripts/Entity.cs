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
        TimerCooldown = new GTimer(this, () => State = State.Find, 1000);

        AnimationPlayer.AnimationFinished += (anim) =>
        {
          if(anim == State.Attack.ToString().ToLower())
          {
            State = State.Cooldown;
            TimerCooldown.Start();
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
        switch (State)
        {
          case State.Moving:
            if (!FoundEnemy)
                Position += MyTeam == Team.Left ?
                    new Vector2(1, 0) : new Vector2(-1, 0);
            else
            {
                State = State.Attack;
            }
            break;
          case State.Attack:
            AnimationPlayer.Play("attack");
            break;
          case State.Find:
            FoundEnemy = false;
            for (int i = 0; i < DetectionArea.GetOverlappingAreas().Count(); i++)
            {
              if(!DetectionArea.GetOverlappingAreas()[i].IsInGroup(MyTeam.ToString()))
              {
                FoundEnemy = true;
                State = State.Moving;
                break;
              }
            }
            break;
          default:
            break;
        }
        
        Update();
    }

    private void OnHit()
    {
        GD.Print("Hit!");
    }

    private AnimatedSprite2D AnimatedSprite { get; set; }
    private AnimationPlayer AnimationPlayer { get; set; }
    private GTimer TimerCooldown { get; set; }
    private Area2D DetectionArea { get; set; }
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

        var detectionPos = spriteSize.X / 2 + detectionWidth / 2;

        DetectionArea = new Area2D();
        var collisionShape = new CollisionShape2D
        {
            Position = new Vector2(detectionPos, 0),
            Shape = new RectangleShape2D
            {
                Size = new Vector2(detectionWidth, detectionHeight)
            }
        };

        DetectionArea.AreaEntered += (otherArea) =>
        {
            if (otherArea.IsInGroup(OtherTeam.ToString()))
            {
                AnimatedSprite.Play(AnimIdleName);
                FoundEnemy = true;
            }
        };

        DetectionArea.AddChild(collisionShape);
        AddChild(DetectionArea);
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
