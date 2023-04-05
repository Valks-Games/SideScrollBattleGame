using Godot;
using System;

public partial class LevelGear : Sprite2D
{
	[Signal]
	public delegate void LevelPressedEventHandler(int level);

	private Area2D Area2D {get; set;}
	private Vector2 ScaleDefault = new Vector2(1, 1);
	private Vector2 ScaleEnlarge = new Vector2(2, 2);
	private Color ModulateDefault = new Color(1, 1, 1, 1);
	private Color ModulateClick = new Color(1, 1, 1, 0.3f);
	private Tween TweenGearScale {get; set;}
	private Tween TweenGearModulate {get; set;}
	private bool IsMouseHover = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Area2D = GetNode<Area2D>("Area2D");
		Area2D.MouseEntered += _on_area_2d_mouse_entered;
		Area2D.MouseExited += _on_area_2d_mouse_exited;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void AnimateScaleTween(Vector2 scale)
	{
		TweenGearScale?.Kill();
		TweenGearScale = CreateTween();
		TweenGearScale.TweenProperty(this, "scale", scale, 0.5f)
			.SetTrans(Tween.TransitionType.Quint)
			.SetEase(Tween.EaseType.Out);
	}

	public void AnimateModulateTween()
	{
		// fade in and fade out
		TweenGearModulate?.Kill();
		TweenGearModulate = CreateTween();
		TweenGearModulate.TweenProperty(this, "modulate", ModulateClick, 0.5f)
			.SetTrans(Tween.TransitionType.Quint)
			.SetEase(Tween.EaseType.Out);
		
		TweenGearModulate.TweenCallback(Callable.From(() =>
        {
			TweenGearModulate?.Kill();
			TweenGearModulate = CreateTween();
			TweenGearModulate.TweenProperty(this, "modulate", ModulateDefault, 0.5f)
				.SetTrans(Tween.TransitionType.Quint)
				.SetEase(Tween.EaseType.Out);
        }));
	}

	public void _on_area_2d_mouse_entered()
	{
		IsMouseHover = true;
		AnimateScaleTween(ScaleEnlarge);
	}

	public void _on_area_2d_mouse_exited()
	{
		IsMouseHover = false;
		AnimateScaleTween(ScaleDefault);
	}

	public int GetLevel()
	{
		// regex keep numbers only
		var levelname = Regex.Replace(Name.ToString(), @"[^\d]", "");
		return levelname.ToInt();
	}

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
		{
			if (IsMouseHover && mouseEvent.IsLeftClickPressed())
			{
				AnimateModulateTween();
				EmitSignal(SignalName.LevelPressed, GetLevel());
			}
		}
    }
}
