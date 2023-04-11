namespace SideScrollGame;

public class EntityStateCooldown : State<Entity>
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
        Timer.StartMs();
    }

    public override void Update()
    {

    }

    public override void ExitState()
    {

    }
}
