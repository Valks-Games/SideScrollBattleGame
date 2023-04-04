namespace SideScrollGame;

public partial class Levels : Node2D
{
    private float V1 { get; set; }
    private float V2 { get; set; }
    private float V3 { get; set; }
    private float V4 { get; set; }

    public override void _Ready()
    {
        var canvasLayer = GetNode<CanvasLayer>("../CanvasLayer");

        var vbox = new VBoxContainer();
        vbox.AddThemeConstantOverride("separation", 0);
        canvasLayer.AddChild(vbox);

        var slider1 = new UISlider(new SliderOptions
        {
            Name = "Debug",
            HSlider = new HSlider {
                MinValue = -1000,
                MaxValue = 1000    
            },
            HideBackPanel = false
        });
        slider1.ValueChanged += v => V1 = v;

        var slider2 = new UISlider(new SliderOptions
        {
            Name = "Debug",
            HSlider = new HSlider
            {
                MinValue = -1000,
                MaxValue = 1000
            },
            HideBackPanel = false
        });
        slider2.ValueChanged += v => V2 = v;

        var slider3 = new UISlider(new SliderOptions
        {
            Name = "Debug",
            HSlider = new HSlider
            {
                MinValue = -1000,
                MaxValue = 1000
            },
            HideBackPanel = false
        });
        slider3.ValueChanged += v => V3 = v;

        var slider4 = new UISlider(new SliderOptions
        {
            Name = "Debug",
            HSlider = new HSlider
            {
                MinValue = -1000,
                MaxValue = 1000
            },
            HideBackPanel = false
        });
        slider4.ValueChanged += v => V4 = v;

        vbox.AddChild(slider1);
        vbox.AddChild(slider2);
        vbox.AddChild(slider3);
        vbox.AddChild(slider4);
    }

    public override void _Process(double delta)
    {
        QueueRedraw();
    }

    public override void _Draw()
    {
        var points = this.GetChildren<Node2D>().Select(x => x.Position).ToArray();

        for (int i = 0; i < points.Count() - 1; i++)
        {
            var pointA = points[i];
            var pointB = points[i + 1];

            var c1 = new Vector2(-299 + V1, 102 + V2);
            var c2 = new Vector2( 243 + V3, 198 + V4);

            BezierCurve.Draw(this, pointA, pointB, c1, c2, Colors.White);
        }
    }
}
