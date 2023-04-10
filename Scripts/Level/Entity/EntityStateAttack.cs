namespace SideScrollGame;

public enum Attack
{
    Spinning,
    Sword
}

public class EntityStateAttack : EntityState<Entity>
{
    public EntityStateAttack(Entity entity) : base(entity) { }

    public override void EnterState()
    {
        Entity.Attacking = true;
        Entity.AnimatedSprite.Play("idle");

        switch (Entity.AttackType)
        {
            case Attack.Spinning:
                Entity.SpinningAttack();
                break;
            case Attack.Sword:
                Entity.SwordAttack();
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
