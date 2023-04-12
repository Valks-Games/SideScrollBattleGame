namespace SideScrollGame;

public class EntityAnimationAttack
{
    public  GTween           Tween          { get; }

    private Entity           Entity         { get; }
    private AnimatedSprite2D AnimatedSprite { get; }

    public EntityAnimationAttack(Entity entity)
    {
        Entity = entity;
        AnimatedSprite = entity.AnimatedSprite;
        Tween = new GTween(AnimatedSprite);
    }

    /// <summary>
    /// Roll while staying still, then roll super fast forward
    /// </summary>
    public void Spinning()
    {
        Tween.Create();
        var rot = Mathf.Pi * 2;

        Tween.Animate("rotation", rot, 0.5);

        rot += Mathf.Pi * 2 * 9;

        Tween.Animate("rotation", rot, 0.5);
        Tween.Animate("position:x", Entity.DetectionRange, 0.5, true);

        Tween.Callback(Entity.Attack);

        Tween.Animate("position", new Vector2(9, -2), 0.1);
        Tween.Animate("position", new Vector2(8, 0), 0.3);

        rot -= Mathf.Pi * 2 * 2;

        Tween.Animate("rotation", rot, 0.8).SetDelay(0.5);
        Tween.Animate("position", new Vector2(0, 0), 0.8, true).SetDelay(0.5);

        Tween.Callback(() =>
        {
            AnimatedSprite.Rotation = 0;
            Entity.CurrentState.SwitchState(Entity.EntityStateCooldown);
        });
    }

    /// <summary>
    /// Jump forward then rotate towards the enemy a bit
    /// </summary>
    public void Sword()
    {
        Tween.Create();
        var rot = Mathf.Pi * 0.05;

        Tween.Animate("rotation", rot, 0.25);

        Tween.Animate("position:y", -5, 0.125);
        Tween.Animate("position:x", 10, 0.25, true);

        Tween.Callback(Entity.Attack);

        Tween.Animate("position:y", 5, 0.5).SetDelay(0.125);
        Tween.Animate("position:x", -10, 0.5, true).SetDelay(0.25);

        rot -= Mathf.Pi * 0.05;

        Tween.Animate("rotation", rot, 0.5, true).SetDelay(0.25);

        Tween.Callback(() =>
        {
            AnimatedSprite.Rotation = 0;
            Entity.CurrentState.SwitchState(Entity.EntityStateCooldown);
        });
    }
}
