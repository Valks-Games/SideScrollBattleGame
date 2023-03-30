namespace SideScrollGame;

public abstract partial class Entity : AnimatedSprite2D
{
    public abstract Team MyTeam { get; }

    public virtual void Init() { }
    public virtual void Update() { }

    public override void _Ready()
    {
        // Set the other team
        OtherTeam = MyTeam == Team.Left ? Team.Right : Team.Left;

        // Play the 'move' animation set at a random starting frame
        if (SpriteFrames.HasAnimation(AnimMoveName))
            this.PlayRandom(AnimMoveName);

        // Create the Area2D for this sprite. All other areas will try to detect this area
        var spriteSize = this.GetSize(AnimMoveName);
        CreateBodyArea(spriteSize);
        CreateDetectionArea(spriteSize);

        Init();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!FoundEnemy)
        {
            if (MyTeam == Team.Left)
                Position += new Vector2(1, 0);
            else
                Position -= new Vector2(1, 0);
        }
        
        Update();
    }

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

        if (MyTeam == Team.Left)
            detectionPos += offset;
        else
            detectionPos -= offset;

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
                Play(AnimIdleName);
                FoundEnemy = true;
            }
        };

        area.AddChild(collisionShape);
        AddChild(area);
    }
}

public enum Team
{
    Left,
    Right
}
