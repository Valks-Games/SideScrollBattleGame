namespace SideScrollGame;

public partial class CameraController : Camera2D
{
    private float ZoomIncrement      { get; set; } = 0.08f;
    private float MinZoom            { get; set; } = 1.5f;
    private float MaxZoom            { get; set; } = 3.0f;
    private float SmoothFactor       { get; set; } = 0.25f;
    private float HorizontalPanSpeed { get; } = 8;

    private float TargetZoom         { get; set; }

    public override void _Ready()
    {
        // Set the initial target zoom value on game start
        TargetZoom = Zoom.X;
    }

    public override void _PhysicsProcess(double delta)
    {
        HandlePanning();

        // Lerp to the target zoom for a smooth effect
        Zoom = Zoom.Lerp(new Vector2(TargetZoom, TargetZoom), SmoothFactor);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
            InputEventMouseButton(mouseEvent);
    }

    private void HandlePanning()
    {
        var cameraWidth = GetViewportRect().Size.X / Zoom.X;

        if (Input.IsActionPressed("move_left"))
        {
            // Prevent the camera from going too far left
            if (Position.X - (cameraWidth / 2) > LimitLeft)
                Position -= new Vector2(HorizontalPanSpeed, 0);
        }

        if (Input.IsActionPressed("move_right"))
        {
            // Prevent the camera from going too far right
            if (Position.X + (cameraWidth / 2) < LimitRight)
                Position += new Vector2(HorizontalPanSpeed, 0);
        }
    }

    private void InputEventMouseButton(InputEventMouseButton @event)
    {
        HandleZoom(@event);
    }

    private void HandleZoom(InputEventMouseButton @event)
    {
        // Not sure why or if this is required
        if (!@event.IsPressed())
            return;

        // Zoom in
        if (@event.ButtonIndex == MouseButton.WheelUp)
            TargetZoom += ZoomIncrement;

        // Zoom out
        if (@event.ButtonIndex == MouseButton.WheelDown)
            TargetZoom -= ZoomIncrement;

        // Clamp the zoom
        TargetZoom = Mathf.Clamp(TargetZoom, MinZoom, MaxZoom);
    }
}
