namespace SideScrollGame;

public partial class GameMaster : Node
{
    public static Base PlayerBase  { get; set; }
    public static Base EnemyBase   { get; set; }
    public static Node PlayerUnits { get; set; }
    public static Node EnemyUnits  { get; set; }

    private static CancellationTokenSource SpawnUnitsCTS { get; } = new();

    public static void DestroyAllUnits(Team team)
    {
        SpawnUnitsCTS.Cancel();

        if (team == Team.Left)
            PlayerUnits.QueueFreeChildren();
        else
            EnemyUnits.QueueFreeChildren();
    }

    public override async void _Ready()
    {
        PlayerUnits = new Node2D { Name = "Player Units" };
        EnemyUnits  = new Node2D { Name = "Enemy Units" };
        PlayerBase  = GetNode<Base>("Player Base");
        EnemyBase   = GetNode<Base>("Enemy Base");

        PlayerBase.Destroyed += () =>
        {
            DestroyAllUnits(Team.Left);
            SpawnMatchResultLabel(Team.Left);
        };

        EnemyBase.Destroyed += () =>
        {
            DestroyAllUnits(Team.Right);
            SpawnMatchResultLabel(Team.Right);
        };

        AddChild(PlayerUnits);
        AddChild(EnemyUnits);

        await Task.Factory.StartNew(() => SpawnUnits(), SpawnUnitsCTS.Token);
    }

    private void SpawnMatchResultLabel(Team team)
    {
        GLabel LabelMatchResult;

        if (team == Team.Left)
            LabelMatchResult = new GLabel("Defeat...");
        else
            LabelMatchResult = new GLabel("Victory!");

        LabelMatchResult.SetFontSize(120);

        // Add the label to the center of the screen
        Main.CanvasLayer.AddChild(LabelMatchResult);
        LabelMatchResult.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.Center);

        // Keep track of center position
        var originalPos = LabelMatchResult.Position;

        // Set position to be offscreen
        LabelMatchResult.Position = new Vector2(
            (team == Team.Left ? 1 : -1) * GWindow.GetWidth() / 2,
            originalPos.Y);

        // Tween the offscreen label to its original posiition
        // Tween Transitions Cheat Sheet: https://www.reddit.com/r/godot/comments/dgh9vd/transitiontype_cheat_sheet_tween_interpolation_oc/
        var tween = LabelMatchResult.CreateTween();
        tween.TweenProperty(LabelMatchResult, "position", originalPos, 1.0f)
            .SetTrans(Tween.TransitionType.Quint)
            .SetEase(Tween.EaseType.Out);

        tween.TweenCallback(Callable.From(() => SceneManager.SwitchScene(Scene.Map)));
    }

    private async Task SpawnUnits()
    {
        // player units
        for (int i = 0; i < 1; i++)
        {
            await Task.Delay(1, SpawnUnitsCTS.Token);
            Units.Create(Unit.OrangeBall, Team.Left);
        }

        // enemy units
        for (int i = 0; i < 30; i++)
        {
            await Task.Delay(100, SpawnUnitsCTS.Token);
            Units.Create(Unit.Skeleton, Team.Right);
        }
    }
}
