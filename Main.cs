using Godot;

public partial class Main : Node
{
    [Export]
    public PackedScene MobScene { get; set; }
    private int score;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void GameOver()
    {
        this.GetNode<AudioStreamPlayer>("Music").Stop();
        this.GetNode<AudioStreamPlayer>("DeathSound").Play();


        this.GetNode<Timer>("MobTimer").Stop();
        this.GetNode<Timer>("ScoreTimer").Stop();
        this.GetNode<HUD>("HUD").ShowGameOver();
    }

    public void NewGame()
    {
        this.score = 0;

        var player = this.GetNode<Player>("Player");
        player.Show();
        var startPosition = this.GetNode<Marker2D>("StartPosition");
        player.Start(startPosition.Position);

        this.GetTree().CallGroup("mobs", Node.MethodName.QueueFree);

        this.GetNode<Timer>("StartTimer").Start();
        var hud = this.GetNode<HUD>("HUD");
        hud.UpdateScore(this.score);
        hud.ShowMessage("Get Ready!");
        this.GetNode<AudioStreamPlayer>("Music").Play();
    }

    private void OnScoreTimerTimeout()
    {
        this.score++;
        this.GetNode<HUD>("HUD").UpdateScore(this.score);
    }

    private void OnStartTimerTimeout()
    {
        this.GetNode<Timer>("MobTimer").Start();
        this.GetNode<Timer>("ScoreTimer").Start();
    }

    private void OnMobTimerTimeout()
    {
        Mob mob = this.MobScene.Instantiate<Mob>();

        var mobSpawnLocation = this.GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
        mobSpawnLocation.ProgressRatio = GD.Randf();

        float direction = mobSpawnLocation.Rotation = Mathf.Pi / 2;

        mob.Position = mobSpawnLocation.Position;

        direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
        mob.Rotation = direction;

        var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
        mob.LinearVelocity = velocity.Rotated(direction);

        this.AddChild(mob);
    }
}
