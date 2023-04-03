namespace SideScrollGame;

public class StateMove : State<Entity>
{
    public StateMove(Entity entity) : base(entity) { }

    public override void EnterState()
    {
        Entity.AnimatedSprite.InstantPlay("move", 1);
    }

    public override void Update()
    {
        Entity.Position += Entity.Team == Team.Left ?
            new Vector2(Entity.MoveSpeed, 0) : new Vector2(-Entity.MoveSpeed, 0);
    }

    public override void ExitState()
    {

    }
}
