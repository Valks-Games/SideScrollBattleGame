namespace SideScrollGame;

public class StateAttack : State<Entity>
{
    public StateAttack(Entity entity) : base(entity) { }

    public override void EnterState()
    {
        var tween = Entity.GetTree().CreateTween();
        var rot = Mathf.Pi * 2;

        tween.TweenProperty(Entity.AnimatedSprite, "rotation", rot, 0.5);

        rot += Mathf.Pi * 2 * 9;

        tween.TweenProperty(Entity.AnimatedSprite, "rotation", rot, 0.5);
        tween.Parallel().TweenProperty(Entity.AnimatedSprite, "position:x", Entity.DetectionRange, 0.5);

        tween.TweenProperty(Entity.AnimatedSprite, "position", new Vector2(9, -2), 0.1);
        tween.TweenProperty(Entity.AnimatedSprite, "position", new Vector2(8, 0), 0.3);
        
        rot -= Mathf.Pi * 2 * 2;

        tween.TweenProperty(Entity.AnimatedSprite, "rotation", rot, 0.8)
            .SetDelay(0.5);
        tween.Parallel().TweenProperty(Entity.AnimatedSprite, "position", new Vector2(0, 0), 0.8)
            .SetDelay(0.5);
    }

    public override void Update()
    {
        
    }

    public override void ExitState()
    {
        
    }
}
