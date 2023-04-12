namespace SideScrollGame;

public partial class Level : Node
{
    public static Base PlayerBase  { get; private set; }
    public static Base EnemyBase   { get; private set; }
    public static Node PlayerUnits { get; private set; }
    public static Node EnemyUnits  { get; private set; }
    public static CanvasLayer CanvasLayer { get; set; }
    
    private CancellationTokenSource SpawnUnitsCTS { get; } = new();
    
	public override async void _Ready()
	{
		CanvasLayer = GetNode<CanvasLayer>("CanvasLayer");

        PlayerUnits = new Node2D { Name = "Player Units" };
        EnemyUnits = new Node2D { Name = "Enemy Units" };

        PlayerBase = Prefabs.Base.Instantiate<Base>();
        PlayerBase.MaxHealth = PlayerStats.BaseHealth;
        PlayerBase.Team = Team.Left;
        AddChild(PlayerBase);

        EnemyBase = Prefabs.Base.Instantiate<Base>();
        EnemyBase.MaxHealth = Global.LevelSettings.EnemyBaseHealth;
        EnemyBase.Team = Team.Right;
        EnemyBase.SelfModulate = new Color(0.3f, 0.3f, 0.3f);
        AddChild(EnemyBase);

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

        PlayerBase.Position = new Vector2(-Global.LevelSettings.Width / 2, -64);
        EnemyBase.Position  = new Vector2( Global.LevelSettings.Width / 2, -64);

        var camera = GetNode<CameraController>("Camera2D");
        camera.LimitLeft = -Global.LevelSettings.Width / 2 - 100;
        camera.LimitRight = Global.LevelSettings.Width / 2 + 100;

        await Task.Factory.StartNew(() => SpawnUnits(), SpawnUnitsCTS.Token);
    }

    public void DestroyAllUnits(Team team)
    {
        SpawnUnitsCTS.Cancel();

        if (team == Team.Left)
            PlayerUnits.QueueFreeChildren();
        else
            EnemyUnits.QueueFreeChildren();
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
        CanvasLayer.AddChild(LabelMatchResult);
        LabelMatchResult.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.Center);

        // Keep track of center position
        var originalPos = LabelMatchResult.Position;

        // Set position to be offscreen
        LabelMatchResult.Position = new Vector2(
            (team == Team.Left ? 1 : -1) * GWindow.GetWidth() / 2,
            originalPos.Y);

        // Tween the offscreen label to its original posiition
        // Tween Transitions Cheat Sheet: https://www.reddit.com/r/godot/comments/dgh9vd/transitiontype_cheat_sheet_tween_interpolation_oc/
        var tween = new GTween(LabelMatchResult);
        tween.Create();
        tween.Animate("position", originalPos, 1.0f)
            .SetTrans(Tween.TransitionType.Quint)
            .SetEase(Tween.EaseType.Out);

        tween.Callback(() => SceneManager.SwitchScene("map"));
    }

    private async Task SpawnUnits()
    {
        // player units
        for (int i = 0; i < 1; i++)
        {
            await Task.Delay(1, SpawnUnitsCTS.Token);
            PlayerBase.SpawnUnit(Units.OrangeBall);
        }

        // enemy units
        for (int i = 0; i < 30; i++)
        {
            await Task.Delay(100, SpawnUnitsCTS.Token);
            EnemyBase.SpawnUnit(Units.Skeleton);
        }
    }
}
