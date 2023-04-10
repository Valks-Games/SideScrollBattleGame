namespace SideScrollGame;

public static class Attacks
{
    /// <summary>
    /// Roll while staying still, then roll super fast forward
    /// </summary>
    public static void Spinning(Entity entity) 
    {
        var tween = entity.AttackTween;
        var sprite = entity.AnimatedSprite;

        tween = entity.CreateTween();
        var rot = Mathf.Pi * 2;

        tween.TweenProperty(sprite, "rotation", rot, 0.5);

        rot += Mathf.Pi * 2 * 9;

        tween.TweenProperty(sprite, "rotation", rot, 0.5);
        tween.Parallel().TweenProperty(
            sprite, "position:x", entity.DetectionRange, 0.5);

        tween.TweenCallback(Callable.From(() =>
        {
            entity.Attack();
        }));

        tween.TweenProperty(sprite, 
            "position", new Vector2(9, -2), 0.1);
        tween.TweenProperty(sprite, 
            "position", new Vector2(8, 0), 0.3);

        rot -= Mathf.Pi * 2 * 2;

        tween.TweenProperty(sprite, "rotation", rot, 0.8)
            .SetDelay(0.5);
        tween.Parallel().TweenProperty(sprite, 
            "position", new Vector2(0, 0), 0.8)
            .SetDelay(0.5);

        tween.TweenCallback(Callable.From(() =>
        {
            sprite.Rotation = 0;
            entity.States[entity.CurrentState].SwitchState(EntityStateType.Cooldown);
        }));
    }

    public static void Sword(Entity entity) 
    {
        var tween = entity.AttackTween;
        var sprite = entity.AnimatedSprite;

        tween = entity.CreateTween();
        var rot = Mathf.Pi * 0.05;

        tween.TweenProperty(sprite, "rotation", rot, 0.25);

        tween.TweenProperty(
            entity.AnimatedSprite, "position:y", -5, 0.125);
        tween.Parallel().TweenProperty(
            entity.AnimatedSprite, "position:x", 10, 0.25);

        tween.TweenCallback(Callable.From(() =>
        {
            entity.Attack();
        }));

        tween.TweenProperty(entity.AnimatedSprite, 
            "position:y", 5, 0.5)
            .SetDelay(0.125);
        tween.Parallel().TweenProperty(entity.AnimatedSprite, 
            "position:x", -10, 0.5)
            .SetDelay(0.25);

        rot -= Mathf.Pi * 0.05;

        tween.Parallel().TweenProperty(sprite, "rotation", rot, 0.5)
            .SetDelay(0.25);

        tween.TweenCallback(Callable.From(() =>
        {
            sprite.Rotation = 0;
            entity.States[entity.CurrentState].SwitchState(EntityStateType.Cooldown);
        }));
    }
}
