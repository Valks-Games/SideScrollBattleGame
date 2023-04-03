namespace SideScrollGame;

public class StateMove : State<Entity>
{
    public StateMove(Entity entity) : base(entity) { }

    public override void EnterState()
    {

    }

    public override void Update()
    {
        Entity.FoundEnemy = false;

        // this is being done every frame -- very ugly
        Entity.ValidateDetectedEnemies();
        if (Entity.DetectedEnemies.Count > 0)
            Entity.FoundEnemy = true;

        if (!Entity.FoundEnemy)
        {
            Entity.AnimatedSprite.InstantPlay("move");

            Entity.Position += Entity.Team == Team.Left ?
                new Vector2(Entity.MoveSpeed, 0) : new Vector2(-Entity.MoveSpeed, 0);
        }
        else
        {
            SwitchState(StateType.Attack);
        }
    }

    public override void ExitState()
    {

    }
}
