namespace SideScrollGame;

// About Scene Switching: https://docs.godotengine.org/en/latest/tutorials/scripting/singletons_autoload.html
public partial class Global : Node
{
    private static Global    Instance     { get; set; }
    private static Node      CurrentScene { get; set; }
    private static SceneTree Tree         { get; set; }

    public override void _Ready()
    {
        Instance = this;
        Tree = GetTree();
        var root = Tree.Root;
        CurrentScene = root.GetChild(root.GetChildCount() - 1);
    }

    public void SwitchScene(Scene scene)
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
    }
}

public enum Scene
{
    Main,
    Map
}
