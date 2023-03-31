namespace SideScrollGame;

public partial class Base : Sprite2D, IDamageable
{
    [Export] public Team Team { get; set; }
    [Export] public double MaxHealth { get; set; }

    public bool Destroyed => GodotObject.IsInstanceValid(this);

    public double CurHealth
    {
        get => double.Parse(LabelCurHealth.Text);
        set
        {
            if (value <= 0)
            {
                QueueFree();
                return;
            }

            LabelCurHealth.Text = value + "";
        }
    }

    private Label LabelMaxHealth { get; set; }
    private Label LabelCurHealth { get; set; }

    public override void _Ready()
    {
        AddToGroup(Team.ToString());

        var hbox = GetNode<HBoxContainer>("HBox");

        LabelMaxHealth = hbox.GetNode<Label>("MaxHealth");
        LabelCurHealth = hbox.GetNode<Label>("CurHealth");

        LabelMaxHealth.Text = MaxHealth + "";
        CurHealth = MaxHealth;
    }
}
