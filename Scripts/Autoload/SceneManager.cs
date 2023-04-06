namespace SideScrollGame;

// About Scene Switching: https://docs.godotengine.org/en/latest/tutorials/scripting/singletons_autoload.html
public partial class SceneManager : Node
{
    private static SceneManager Instance     { get; set; }
    private static Node         CurrentScene { get; set; }
    private static SceneTree    Tree         { get; set; }

    public override void _Ready()
    {
        Instance = this;
        Tree = GetTree();
        var root = Tree.Root;
        CurrentScene = root.GetChild(root.GetChildCount() - 1);
    }

    public static void SwitchScene(Scene scene)
    {
        // Wait for engine to be ready to switch scene
        Instance.CallDeferred(nameof(DeferredSwitchScene), Variant.From(scene));
    }

    private void DeferredSwitchScene(Variant variantScene)
    {
        var scene = variantScene.As<Scene>().ToString().ToLower();

        // Safe to remove scene now
        CurrentScene.Free();

        // Load a new scene.
        var nextScene = (PackedScene)GD.Load($"res://Scenes/{scene}.tscn");

        // Instance the new scene.
        CurrentScene = nextScene.Instantiate();

        // Add it to the active scene, as child of root.
        Tree.Root.AddChild(CurrentScene);

        // Optionally, to make it compatible with the SceneTree.change_scene_to_file() API.
        Tree.CurrentScene = CurrentScene;

        FadeFromBlack(1);
    }

    private void FadeFromBlack(double duration)
    {
        // Add canvas layer to scene if does not exist
        var canvasLayer = CurrentScene.GetNodeOrNull<CanvasLayer>("CanvasLayer");

        if (canvasLayer == null)
        {
            canvasLayer = new CanvasLayer();
            CurrentScene.AddChild(canvasLayer);
        }

        // Setup color rect
        var colorRect = new ColorRect
        {
            Color = Colors.Black,
            MouseFilter = Control.MouseFilterEnum.Ignore
        };

        // Make the color rect cover the entire screen
        colorRect.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
        canvasLayer.AddChild(colorRect);

        // Animate color rect
        var tween = colorRect.CreateTween();
        tween.TweenProperty(colorRect, "color", new Color(0, 0, 0, 0), duration);
        tween.TweenCallback(Callable.From(() => colorRect.QueueFree()));
    }
}

public enum Scene
{
    Map
}
