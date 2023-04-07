namespace SideScrollGame;

public interface IDamageable
{
    public Team   Team      { get; }
    public double CurHealth { get; set; }
}
