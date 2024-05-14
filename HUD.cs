using Godot;

public partial class HUD : CanvasLayer
{
    [Signal]
    public delegate void StartGameEventHandler();

    public void ShowMessage(string text)
    {
        var message = this.GetNode<Label>("Message");
        message.Text = text;
        message.Show();

        this.GetNode<Timer>("MessageTimer").Start();
    }

    async public void ShowGameOver()
    {
        this.ShowMessage("Game Over!");

        var messageTimer = this.GetNode<Timer>("MessageTimer");
        await this.ToSignal(messageTimer, Timer.SignalName.Timeout);

        var message = this.GetNode<Label>("Message");
        message.Text = "Dodge the Creeps!";
        message.Show();

        await this.ToSignal(this.GetTree().CreateTimer(1.0), SceneTreeTimer.SignalName.Timeout);
        this.GetNode<Button>("StartButton").Show();
    }

    public void UpdateScore(int score)
    {
        this.GetNode<Label>("ScoreLabel").Text = score.ToString();
    }

    private void OnStartButtonPressed()
    {
        this.GetNode<Button>("StartButton").Hide();
        this.EmitSignal(SignalName.StartGame);
    }

    private void OnMessageTimerTimeout()
    {
        this.GetNode<Label>("Message").Hide();
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
