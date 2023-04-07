namespace SideScrollGame;

[Tool]
public partial class LevelSettings : Resource
{
    [Export] public int Width
    {
        get => _width;
        set => _width = Mathf.Max(value, 500);
    }
    [Export] public int EnemyBaseHealth
    {
        get => _enemyBaseHealth;
        set => _enemyBaseHealth = Mathf.Max(value, 1);
    }

    private int _width;
    private int _enemyBaseHealth;
}
