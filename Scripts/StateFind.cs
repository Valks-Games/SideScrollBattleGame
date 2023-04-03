namespace SideScrollGame;

// unused script - maybe delete?
public class StateFind : State<Entity>
{
    public StateFind(Entity entity) : base(entity) { }

    public override void EnterState()
    {

    }

    public override void Update()
    {
        Entity.FoundEnemy = false;

        // this looks ugly to me
        SwitchState(StateType.Move);

        Entity.ValidateDetectedEnemies();
        if (Entity.DetectedEnemies.Count > 0)
        {
            Entity.FoundEnemy = true;
            SwitchState(StateType.Attack);
        }
    }

    public override void ExitState()
    {

    }
}
