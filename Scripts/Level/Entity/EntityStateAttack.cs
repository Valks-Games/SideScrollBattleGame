namespace SideScrollGame;

public enum Attack
{
    Spinning,
    Sword
}

public class EntityStateAttack : State<Entity>
{
    public EntityStateAttack(Entity entity) : base(entity) { }

    public override void EnterState()
    {
        Entity.Attacking = true;
        Entity.AnimatedSprite.Play("idle");

        switch (Entity.AttackType)
        {
            case Attack.Spinning:
                Entity.AnimationAttack.Spinning();
                break;
            case Attack.Sword:
                Entity.AnimationAttack.Sword();
                break;
        }
    }

    public override void Update()
    {
        
    }

    public override void ExitState()
    {
        Entity.Attacking = false;
    }
}
