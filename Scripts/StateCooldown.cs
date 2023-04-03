namespace SideScrollGame;

public class StateCooldown : State<Entity>
{
    private GTimer Timer { get; set; }

    public StateCooldown(Entity entity) : base(entity)
    {
        Timer = new GTimer(
            entity, 
            () =>
            {
                Entity.ValidateDetectedEnemies();
                if (Entity.DetectedEnemies.Count > 0)
                {
                    SwitchState(StateType.Attack);
                    return;
                }

                SwitchState(StateType.Move);
            }, 
            entity.AttackCooldownDuration);
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
