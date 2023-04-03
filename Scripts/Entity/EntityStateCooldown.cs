namespace SideScrollGame;

public class EntityStateCooldown : EntityState<Entity>
{
    private GTimer Timer { get; set; }

    public EntityStateCooldown(Entity entity) : base(entity)
    {
        Timer = new GTimer(
            entity, 
            () =>
            {
                if (Entity.GetEnemyCount() > 0)
                {
                    SwitchState(EntityStateType.Attack);
                    return;
                }

                SwitchState(EntityStateType.Move);
            }, 
            entity.AttackSpeed);
    }

    public override void EnterState()
    {
        Timer.StartMs(1);
    }

    public override void Update()
    {

    }

    public override void ExitState()
    {

    }
}
