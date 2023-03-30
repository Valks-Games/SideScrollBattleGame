namespace SideScrollGame;

public partial class CameraController : Camera2D
{
    private float ScrollSpeed { get; } = 8;

    public override void _PhysicsProcess(double delta)
    {
        var cameraWidth = GetViewportRect().Size.X / Zoom.X;

        if (Input.IsActionPressed("move_left"))
        {
            // Prevent the camera from going too far left
            if (Position.X - (cameraWidth / 2) > LimitLeft)
                Position -= new Vector2(ScrollSpeed, 0);
        }

        if (Input.IsActionPressed("move_right"))
        {
            // Prevent the camera from going too far right
            if (Position.X + (cameraWidth / 2) < LimitRight)
                Position += new Vector2(ScrollSpeed, 0);
        }
    }
}
