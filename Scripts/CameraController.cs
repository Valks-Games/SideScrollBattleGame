namespace SideScrollGame;

public partial class CameraController : Camera2D
{
    private float ScrollSpeed { get; } = 15;

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionPressed("move_left"))
        {
            Position -= new Vector2(ScrollSpeed, 0);
        }

        if (Input.IsActionPressed("move_right"))
        {
            Position += new Vector2(ScrollSpeed, 0);
        }
    }
}
