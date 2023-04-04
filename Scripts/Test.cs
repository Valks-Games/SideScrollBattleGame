using Godot;
using System;

namespace SideScrollGame;

public partial class Test : PathFollow2D
{
    public override void _Ready()
    {
        var tween = CreateTween();
        tween.TweenProperty(this, "progress_ratio", 1.0f, 3.0)
            .SetTrans(Tween.TransitionType.Quint)
            .SetEase(Tween.EaseType.InOut);
    }
}
