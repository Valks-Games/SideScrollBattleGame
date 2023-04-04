using System.Drawing;

namespace SideScrollGame;

public partial class Levels : Node2D
{
    private float P2x { get; set; }
    private float P2y { get; set; }
    private float P3x { get; set; }
    private float P3y { get; set; }
    private float T { get; set; }

    public override void _Process(double delta)
    {
        QueueRedraw();
    }

    public override void _Draw()
    {
        var center = GWindow.GetCenter();
        var pointA = center - new Vector2(200, 0);
        var pointB = center + new Vector2(200, -75);

        var c1 = new Vector2(P2x + T, P2y);
        var c2 = new Vector2(P3x - T, P3y);

        BezierCurve.Draw(this, pointA, pointB, c1, c2, Colors.White);
    }

    private void _on_test_value_changed(float v)
    {
        T = v;
        GD.Print(v);
    }

    private void _on_p_1x_value_changed(float v)
    {
        P2x = v;
        GD.Print(v);
    }

    private void _on_p_1y_value_changed(float v)
    {
        P2y = v;
        GD.Print(v);
    }

    private void _on_p_2x_value_changed(float v)
    {
        P3x = v;
        GD.Print(v);
    }

    private void _on_p_2y_value_changed(float v)
    {
        P3y = v;
        GD.Print(v);
    }
}
