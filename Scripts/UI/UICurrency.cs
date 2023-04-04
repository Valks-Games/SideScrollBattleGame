namespace SideScrollGame;

public partial class UICurrency : Node
{
    public static int Gold
    {
        get
        {
            return int.Parse(LabelGold.Text);
        }
        set
        {
            LabelGold.Text = Mathf.Min(value, MaxGold) + "";
        }
    }

    private static Label LabelGold { get; set; }
    private static int MaxGold { get; set; } = 50;

    private GTimer IncrementGoldTimer { get; set; }

    public override void _Ready()
    {
        LabelGold = GetNode<Label>("Gold");
        IncrementGoldTimer = new GTimer(this, () => Gold++, 100);
        IncrementGoldTimer.Loop = true;
        IncrementGoldTimer.StartMs();
    }
}
