namespace SideScrollGame;

public static class Attacks
{
    /// <summary>
    /// Roll while staying still, then roll super fast forward
    /// </summary>
    public static void Spinning(Entity entity, Action callback) 
    {
        var tween = entity.AttackTween;

        tween = entity.CreateTween();
        var rot = Mathf.Pi * 2;

        tween.TweenProperty(entity.AnimatedSprite, "rotation", rot, 0.5);

        rot += Mathf.Pi * 2 * 9;

        tween.TweenProperty(entity.AnimatedSprite, "rotation", rot, 0.5);
        tween.Parallel().TweenProperty(
            entity.AnimatedSprite, "position:x", entity.DetectionRange, 0.5);

        tween.TweenCallback(Callable.From(() =>
        {
            entity.Attack();
        }));

        tween.TweenProperty(entity.AnimatedSprite, 
            "position", new Vector2(9, -2), 0.1);
        tween.TweenProperty(entity.AnimatedSprite, 
            "position", new Vector2(8, 0), 0.3);

        rot -= Mathf.Pi * 2 * 2;

        tween.TweenProperty(entity.AnimatedSprite, "rotation", rot, 0.8)
            .SetDelay(0.5);
        tween.Parallel().TweenProperty(entity.AnimatedSprite, 
            "position", new Vector2(0, 0), 0.8)
            .SetDelay(0.5);

        tween.TweenCallback(Callable.From(() =>
        {
            // This entity was destroyed (is there a better way of doing this?)
            if (!GodotObject.IsInstanceValid(entity))
                return;

            entity.AnimatedSprite.Rotation = 0;
            callback();
        }));
    }

    public static void Sword(Entity entity, Action callback) 
    {
        var tween = entity.AttackTween;

        tween = entity.CreateTween();
        var rot = Mathf.Pi * 0.05;

        tween.TweenProperty(entity.AnimatedSprite, "rotation", rot, 0.25);

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

        tween.TweenProperty(entity.AnimatedSprite, "rotation", rot, 0.5)
            .SetDelay(0.25);

        tween.TweenCallback(Callable.From(() =>
        {
            // This entity was destroyed (is there a better way of doing this?)
            if (!GodotObject.IsInstanceValid(entity))
                return;

            entity.AnimatedSprite.Rotation = 0;
            callback();
        }));
    }
}
