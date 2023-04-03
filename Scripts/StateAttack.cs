namespace SideScrollGame;

public class StateAttack : State<Entity>
{
    public StateAttack(Entity entity) : base(entity) { }

    public override void EnterState()
    {
        Attacks.Spinning(Entity, () => SwitchState(StateType.Cooldown));
    }

    public override void Update()
    {
        
    }

    public override void ExitState()
    {
        
    }
}
