namespace SideScrollGame;

public abstract class State<T> where T : Entity
{
    protected T Entity { get; set; }
    
    public State(T entity) => Entity = entity;

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void Update();

    protected void SwitchState(StateType newState)
    {
        Entity.States[Entity.CurrentState].ExitState();
        Entity.CurrentState = newState;
        Entity.States[Entity.CurrentState].EnterState();
    }
}

public enum StateType
{
    Attack,
    Cooldown,
    Find,
    Move
}
