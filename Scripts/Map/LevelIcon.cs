namespace SideScrollGame;

public partial class LevelIcon : Node2D
{
    [Export] public LevelSettings LevelSettings { get; set; }

	[Signal] public delegate void LevelPressedEventHandler(int level);

	private Area2D    Area       { get; set; }
	private Sprite2D  Icon       { get; set; }
	private GTween    TweenScale { get; set; }
	private GTween    TweenColor { get; set; }
	private Control   Info       { get; set; }
    private Label     LabelLevel { get; set; }
    private int       Level      { get; set; }

	public override void _Ready()
	{
        // Get level from name
        Level = Regex.Replace(Name.ToString(), @"[^\d]", "").ToInt();

        Info = GetNode<Control>("Info");
        Info.Hide();

        LabelLevel = Info.GetNode<Label>("VBox/Label");
		Icon = GetNode<Sprite2D>("Level Gear");

        TweenScale = new GTween(Icon);
        TweenColor = new GTween(Icon);

        Area = Icon.GetNode<Area2D>("Area2D");
		Area.MouseEntered += () => AnimateScale(2);
		Area.MouseExited  += () => AnimateScale(1);
        Area.InputEvent   += (viewport, inputEvent, shapeId) => 
        {
            if (inputEvent is InputEventMouseButton inputEventMouseBtn)
            {
                if (inputEventMouseBtn.IsLeftClickPressed())
                {
                    AnimateScale(1.5f);
                    AnimateColor();
                    EmitSignal(SignalName.LevelPressed, Level);
                }

                if (inputEventMouseBtn.IsLeftClickReleased())
                {
                    AnimateScale(2);
                }
            }    
        };

        Area.AreaEntered += (otherArea) =>
        {
            if (otherArea.Name == "PlayerMapIconArea")
            {
                LabelLevel.Text = "Level " + Level;
                Info.Show();
            }
        };

        Area.AreaExited += (otherArea) =>
        {
            if (otherArea.Name == "PlayerMapIconArea")
            {
                Info.Hide();
            }
        };
	}

    private void AnimateScale(float scale)
	{
		TweenScale.Create();
		TweenScale.Animate("scale", Vector2.One * scale, 0.5f)
			.SetTrans(Tween.TransitionType.Quint)
			.SetEase(Tween.EaseType.Out);
	}

    public void AnimateColor()
	{
		TweenColor.Create();
		TweenColor.Animate("self_modulate", Colors.Green, 0.5f)
			.SetTrans(Tween.TransitionType.Quint)
			.SetEase(Tween.EaseType.Out);
        TweenColor.Animate("self_modulate", Colors.White, 5f)
            .SetTrans(Tween.TransitionType.Quint)
            .SetEase(Tween.EaseType.Out);
    }

    private bool BtnPressed { get; set; }

    private void _on_level_start_pressed()
    {
        if (BtnPressed)
            return;

        BtnPressed = true;
        Global.LevelSettings = LevelSettings;
        SceneManager.SwitchScene("level");
    }
}
