namespace SideScrollGame;

public static class Units
{
    public static void Create(Unit unitType, Team team)
    {
        Entity entity = null;

        switch (unitType)
        {
            case Unit.OrangeBall:
                entity = OrangeBall();
                break;
            case Unit.Skeleton:
                entity = Skeleton();
                break;
            default:
                throw new ArgumentException("This unit type has not been defined");
        }

        entity.Team = team;

        var spawnOffsetFromBase = 90;
        var maxRandomY = 25;
        var randomY = GD.RandRange(0, maxRandomY);
        
        Vector2 basePosition;
        Node unitsParent;
        int offset;

        if (entity.Team == Team.Left)
        {
            basePosition = Level.PlayerBase.Position;
            unitsParent = Level.PlayerUnits;
            offset = spawnOffsetFromBase;
        }
        else
        {
            basePosition = Level.EnemyBase.Position;
            unitsParent = Level.EnemyUnits;
            offset = -spawnOffsetFromBase;
        }

        entity.Position = basePosition + new Vector2(offset, randomY);
        unitsParent.AddChild(entity);
    }

    private static Entity OrangeBall() => new Entity 
    {
        Scale = Vector2.One * 2,
        SpriteFrames = GD.Load<SpriteFrames>("res://SpriteFrames/orange_ball.tres"),
        AttackType = Attack.Spinning,
    };

    private static Entity Skeleton() => new Entity 
    {
        Scale = Vector2.One * 2,
        SpriteFrames = GD.Load<SpriteFrames>("res://SpriteFrames/skeleton.tres"),
        AttackType = Attack.Sword,
        AttackSpeed = 10,
        MoveSpeed = 1,
        AttackPower = 9999
    };
}

public enum Unit
{
    OrangeBall,
    Skeleton
}
