namespace SideScrollGame;

public abstract class EntityState<T> where T : Entity
{
    protected T Entity { get; set; }
    
    public EntityState(T entity) => Entity = entity;

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void Update();

    public void SwitchState(EntityStateType newState)
    {
        Entity.States[Entity.CurrentState].ExitState();
        Entity.CurrentState = newState;
        Entity.States[Entity.CurrentState].EnterState();
    }
}

public enum EntityStateType
{
    Attack,
    Cooldown,
    Move
}
