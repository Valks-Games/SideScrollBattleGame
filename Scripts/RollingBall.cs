namespace SideScrollGame;

public partial class RollingBall : AnimatedSprite2D
{
    public override void _Ready()
    {
        Play("roll");
        Frame = GD.RandRange(0, SpriteFrames.GetFrameCount("roll"));
    }

    public override void _PhysicsProcess(double delta)
    {
        Position += new Vector2(1, 0);  
    }
}
