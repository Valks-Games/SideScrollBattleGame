namespace SideScrollGame;

public interface IDamageable
{
    public bool   Destroyed { get; }
    public double CurHealth { get; set; }
}
