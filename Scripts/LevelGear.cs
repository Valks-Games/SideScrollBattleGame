namespace SideScrollGame;

public partial class LevelGear : Sprite2D
{
	[Signal] public delegate void LevelPressedEventHandler(int level);

	private Area2D Area       { get; set; }
	private Tween  TweenScale { get; set; }
	private Tween  TweenColor { get; set; }

	public override void _Ready()
	{
		Area = GetNode<Area2D>("Area2D");
		Area.MouseEntered += () => AnimateScaleTween(2);
		Area.MouseExited  += () => AnimateScaleTween(1);
        Area.InputEvent   += (viewport, inputEvent, shapeId) => 
        {
            if (inputEvent is InputEventMouseButton inputEventMouseBtn)
            {
                if (inputEventMouseBtn.IsLeftClickPressed())
                {
                    AnimateScaleTween(1.5f);
                    AnimateColorTween();
                    EmitSignal(SignalName.LevelPressed, GetLevel());
                }

                if (inputEventMouseBtn.IsLeftClickReleased())
                {
                    AnimateScaleTween(2);
                }
            }    
        };
	}

    private void AnimateScaleTween(float scale)
	{
		TweenScale?.Kill();
		TweenScale = CreateTween();
		TweenScale.TweenProperty(this, "scale", Vector2.One * scale, 0.5f)
			.SetTrans(Tween.TransitionType.Quint)
			.SetEase(Tween.EaseType.Out);
	}

    private void AnimateColorTween()
	{
		TweenColor?.Kill();
		TweenColor = CreateTween();
		TweenColor.TweenProperty(this, "modulate", Colors.Green, 0.5f)
			.SetTrans(Tween.TransitionType.Quint)
			.SetEase(Tween.EaseType.Out);
        TweenColor.TweenProperty(this, "modulate", Colors.White, 5f)
            .SetTrans(Tween.TransitionType.Quint)
            .SetEase(Tween.EaseType.Out);
    }

	private int GetLevel() => // Extract level from name
        Regex.Replace(Name.ToString(), @"[^\d]", "").ToInt();
}
