namespace SideScrollGame;

public partial class GameMaster : Node
{
    public static Node2D PlayerBase  { get; set; }
    public static Node2D EnemyBase   { get; set; }
    public static Node   PlayerUnits { get; set; }
    public static Node   EnemyUnits  { get; set; }

    public override async void _Ready()
    {
        PlayerUnits = GetNode("Player Units");
        EnemyUnits  = GetNode("Enemy Units");
        PlayerBase  = GetNode<Node2D>("Player Base");
        EnemyBase   = GetNode<Node2D>("Enemy Base");

        // player units
        for (int i = 0; i < 2; i++)
        {
            await Task.Delay(500);
            Units.Create(Unit.OrangeBall, Team.Left);
        }

        // enemy units
        for (int i = 0; i < 5; i++)
        {
            await Task.Delay(500);
            Units.Create(Unit.Skeleton, Team.Right);
        }
    }
}
