﻿namespace SideScrollGame;

public enum Attack
{
    Spinning
}

public class StateAttack : State<Entity>
{
    public StateAttack(Entity entity) : base(entity) { }

    public override void EnterState()
    {
        Entity.Attacking = true;
        Entity.AnimatedSprite.Play("idle");

        switch (Entity.AttackType)
        {
            case Attack.Spinning:
                Attacks.Spinning(Entity, () => SwitchState(StateType.Cooldown));
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
