namespace SideScrollGame;

public static class Units
{
    public static Entity OrangeBall => new Entity 
    {
        Scale = Vector2.One * 2,
        SpriteFrames = GD.Load<SpriteFrames>("res://SpriteFrames/orange_ball.tres"),
        AttackType = Attack.Spinning,
    };

    public static Entity Skeleton => new Entity 
    {
        Scale = Vector2.One * 2,
        SpriteFrames = GD.Load<SpriteFrames>("res://SpriteFrames/skeleton.tres"),
        AttackType = Attack.Sword,
        AttackSpeed = 10,
        MoveSpeed = 1,
        AttackPower = 5
    };
}
