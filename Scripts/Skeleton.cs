namespace SideScrollGame;

public partial class Skeleton : AnimatedSprite2D
{
    public override void _Ready()
    {
        Play("walk");
        Frame = GD.RandRange(0, SpriteFrames.GetFrameCount("walk"));
    }

    public override void _PhysicsProcess(double delta)
    {
        Position -= new Vector2(1, 0);
    }
}
