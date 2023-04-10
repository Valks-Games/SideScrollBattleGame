namespace SideScrollGame;

public class EntityAnimationAttack
{
    private Entity           Entity         { get; }
    private AnimatedSprite2D AnimatedSprite { get; }
    private Tween            Tween          { get; set; }

    public EntityAnimationAttack(Entity entity)
    {
        Entity = entity;
        AnimatedSprite = entity.AnimatedSprite;
        Tween = entity.AttackTween;
    }

    /// <summary>
    /// Roll while staying still, then roll super fast forward
    /// </summary>
    public void Spinning()
    {
        Tween = Entity.CreateTween();
        var rot = Mathf.Pi * 2;

        Animate("rotation", rot, 0.5);

        rot += Mathf.Pi * 2 * 9;

        Animate("rotation", rot, 0.5);
        Animate("position:x", Entity.DetectionRange, 0.5, true);

        AnimateCallback(Entity.Attack);

        Animate("position", new Vector2(9, -2), 0.1);
        Animate("position", new Vector2(8, 0), 0.3);

        rot -= Mathf.Pi * 2 * 2;

        Animate("rotation", rot, 0.8).SetDelay(0.5);
        Animate("position", new Vector2(0, 0), 0.8, true).SetDelay(0.5);

        AnimateCallback(() =>
        {
            AnimatedSprite.Rotation = 0;
            Entity.States[Entity.CurrentState].SwitchState(EntityStateType.Cooldown);
        });
    }

    /// <summary>
    /// Jump forward then rotate towards the enemy a bit
    /// </summary>
    public void Sword()
    {
        Tween = Entity.CreateTween();
        var rot = Mathf.Pi * 0.05;

        Animate("rotation", rot, 0.25);

        Animate("position:y", -5, 0.125);
        Animate("position:x", 10, 0.25, true);

        AnimateCallback(Entity.Attack);

        Animate("position:y", 5, 0.5).SetDelay(0.125);
        Animate("position:x", -10, 0.5, true).SetDelay(0.25);

        rot -= Mathf.Pi * 0.05;

        Animate("rotation", rot, 0.5, true).SetDelay(0.25);

        AnimateCallback(() =>
        {
            AnimatedSprite.Rotation = 0;
            Entity.States[Entity.CurrentState].SwitchState(EntityStateType.Cooldown);
        });
    }

    private PropertyTweener Animate(string prop, Variant finalValue, double duration, bool parallel = false)
    {
        // For some reason .SetParallel(bool) is not the same as .Parallel() so that
        // is why .Parallel() is used here. Try changing to .SetParallel(bool) to see
        // what I mean
        if (parallel)
            return Tween.Parallel().TweenProperty(AnimatedSprite, prop, finalValue, duration);
        else
            return Tween.TweenProperty(AnimatedSprite, prop, finalValue, duration);
    }

    private void AnimateCallback(Action callback) =>
        Tween.TweenCallback(Callable.From(callback));
}
