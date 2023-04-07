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
                Attacks.Spinning(Entity, () => SwitchState(EntityStateType.Cooldown));
                break;
            case Attack.Sword:
                Attacks.Sword(Entity, () => SwitchState(EntityStateType.Cooldown));
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
