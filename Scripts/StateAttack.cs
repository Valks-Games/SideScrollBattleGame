namespace SideScrollGame;

public class StateAttack : State<Entity>
{
    public StateAttack(Entity entity) : base(entity) { }

    public override void EnterState()
    {
        Entity.AttackTween = Entity.CreateTween();
        var rot = Mathf.Pi * 2;

        Entity.AttackTween.TweenProperty(Entity.AnimatedSprite, "rotation", rot, 0.5);

        rot += Mathf.Pi * 2 * 9;

        Entity.AttackTween.TweenProperty(Entity.AnimatedSprite, "rotation", rot, 0.5);
        Entity.AttackTween.Parallel().TweenProperty(Entity.AnimatedSprite, "position:x", Entity.DetectionRange, 0.5);

        Entity.AttackTween.TweenCallback(Callable.From(() =>
        {
            Entity.Attack();
        }));

        Entity.AttackTween.TweenProperty(Entity.AnimatedSprite, "position", new Vector2(9, -2), 0.1);
        Entity.AttackTween.TweenProperty(Entity.AnimatedSprite, "position", new Vector2(8, 0), 0.3);
        
        rot -= Mathf.Pi * 2 * 2;

        Entity.AttackTween.TweenProperty(Entity.AnimatedSprite, "rotation", rot, 0.8)
            .SetDelay(0.5);
        Entity.AttackTween.Parallel().TweenProperty(Entity.AnimatedSprite, "position", new Vector2(0, 0), 0.8)
            .SetDelay(0.5);

        Entity.AttackTween.TweenCallback(Callable.From(() =>
        {
            // This entity was destroyed (is there a better way of doing this?)
            if (!GodotObject.IsInstanceValid(Entity))
                return;

            Entity.AnimatedSprite.Rotation = 0;
            SwitchState(StateType.Cooldown);
        }));
    }

    public override void Update()
    {
        
    }

    public override void ExitState()
    {
        
    }
}
