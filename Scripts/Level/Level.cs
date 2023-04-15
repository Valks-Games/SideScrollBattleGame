namespace SideScrollGame;

public partial class Level : Node
{
    public static Base PlayerBase  { get; private set; }
    public static Base EnemyBase   { get; private set; }
    public static Node PlayerUnits { get; private set; }
    public static Node EnemyUnits  { get; private set; }
    public static CanvasLayer CanvasLayer { get; set; }
    
    private CancellationTokenSource SpawnUnitsCTS { get; } = new();

    private Dictionary<int, Func<Task>> Spawners { get; } = new();

    public override async void _Ready()
    {
        Spawners.Add(1, Level1);
        Spawners.Add(2, Level2);
        Spawners.Add(3, Level3);
        Spawners.Add(4, Level4);

        CanvasLayer = GetNode<CanvasLayer>("CanvasLayer");

        var settings = Global.LevelSettings;

        InitUnitParents();
        InitPlayerBase(settings);
        InitEnemyBase(settings);
        InitCamera(settings);

        // spawn unit(s) for the player for testing
        for (int i = 0; i < 1; i++)
        {
            await Task.Delay(1, SpawnUnitsCTS.Token);
            PlayerBase.SpawnUnit(Units.OrangeBall);
        }

        try
        {
            await Task.Factory.StartNew(Spawners[settings.Level], SpawnUnitsCTS.Token);
        } 
        catch (KeyNotFoundException)
        {
            Logger.LogWarning("Enemy spawner logic has not yet been implemented " +
                $"for level {settings.Level}");
        }
    }

    public void DestroyAllUnits(Team team)
    {
        SpawnUnitsCTS.Cancel();

        if (team == Team.Left)
            PlayerUnits.QueueFreeChildren();
        else
            EnemyUnits.QueueFreeChildren();
    }

    private void InitCamera(LevelSettings settings)
    {
        var camera = GetNode<CameraController>("Camera2D");
        camera.LimitLeft = -settings.Width / 2 - 100;
        camera.LimitRight = settings.Width / 2 + 100;
    }

    private void InitUnitParents()
    {
        PlayerUnits = new Node2D { Name = "Player Units" };
        EnemyUnits  = new Node2D { Name = "Enemy Units" };

        AddChild(PlayerUnits);
        AddChild(EnemyUnits);
    }

    private void InitPlayerBase(LevelSettings settings)
    {
        PlayerBase = Prefabs.Base.Instantiate<Base>();
        PlayerBase.MaxHealth = PlayerStats.BaseHealth;
        PlayerBase.Team = Team.Left;
        PlayerBase.Position = new Vector2(-settings.Width / 2, -64);
        PlayerBase.Destroyed += () =>
        {
            DestroyAllUnits(Team.Left);
            SpawnMatchResultLabel(Team.Left);
        };
        
        AddChild(PlayerBase);
    }

    private void InitEnemyBase(LevelSettings settings)
    {
        EnemyBase = Prefabs.Base.Instantiate<Base>();
        EnemyBase.MaxHealth = settings.EnemyBaseHealth;
        EnemyBase.Team = Team.Right;
        EnemyBase.SelfModulate = new Color(0.3f, 0.3f, 0.3f);
        EnemyBase.Position = new Vector2(settings.Width / 2, -64);
        EnemyBase.Destroyed += () =>
        {
            DestroyAllUnits(Team.Right);
            SpawnMatchResultLabel(Team.Right);
        };

        AddChild(EnemyBase);
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

    private async Task Level1()
    {
        await Delay(1000);

        EnemyBase.SpawnUnit(Units.Skeleton);

        while (!SpawnUnitsCTS.IsCancellationRequested)
        {
            await Delay(1000);

            for (int i = 0; i < 3; i++)
            {
                await Delay(500);

                EnemyBase.SpawnUnit(Units.Skeleton);
            }
        }
    }

    private async Task Level2()
    {
        while (!SpawnUnitsCTS.IsCancellationRequested)
        {
            await Delay(10000);

            for (int i = 0; i < 10; i++)
            {
                await Delay(100);
                EnemyBase.SpawnUnit(Units.Skeleton);
            }
        }
    }

    private async Task Level3()
    {
        while (!SpawnUnitsCTS.IsCancellationRequested)
        {
            for (int i = 0; i < 5; i++)
            {
                await Delay(100);
                EnemyBase.SpawnUnit(Units.Skeleton);
            }

            await Delay(1000);
        }
    }

    private async Task Level4()
    {
        for (int i = 0; i < 100; i++)
        {
            await Delay(100);
            EnemyBase.SpawnUnit(Units.Skeleton);
        }
    }

    private async Task Delay(int delay) => await Task.Delay(delay, SpawnUnitsCTS.Token);
}
