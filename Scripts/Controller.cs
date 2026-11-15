using System;
using Godot;
using Tachyogenesis.Scripts.UI;

namespace Tachyogenesis.Scripts;

public partial class Controller : Node2D
{
    // ----- NODES -----
    public Camera2D Camera { get; private set; }
    public DialogBox Dialog { get; private set; }
    public Hud Hud { get; private set; }
    public Player Player { get; private set; }
    public UI.PauseMenu PauseMenu { get; private set; }
    
    // ----- RANDOM VARS -----
	public double Elapsed { get; private set; }
	private double _timeScale;
	public double TimeScale
	{
		get => _timeScale;
		set
		{
			_timeScale = Math.Clamp(value, -4D, 4D);
		}
	}
    public bool InPauseMenu { get; private set; }
	public string CurrentMap { get; private set; }
	public bool InCutscene { get; set; }
    public ErrorHelper ErrorHelper { get; private set; } = new();

	public override void _Ready()
    {
		TimeScale = 1D;
		Elapsed = 0D;
		CurrentMap = $"{GetSceneFilePath().Split("/")[^2].ToLower()}_{GetSceneFilePath().Split("/")[^1].TrimSuffix(".tscn")}";
        
        // SET UP NODES
        PauseMenu = GetNode<UI.PauseMenu>("CanvasLayer/PauseMenu");
		Camera = GetNode<Camera2D>("Camera2D");
		Dialog = GetNode<DialogBox>("CanvasLayer/DialogBox");
		Hud = GetNode<Hud>("CanvasLayer/Hud");
        Player = GetNode<Player>("Player");

		Dialog.Hide();
        ResumeGame(); // call to hide Pause Menu
	}

	public override void _Process(double delta)
	{
		Elapsed += delta * Math.Sign(TimeScale);
	}

    public void PauseGame()
    {
        InPauseMenu = true;
        PauseMenu.Show();
        Engine.SetTimeScale(0);
    }

    public void ResumeGame()
    {
        InPauseMenu = false;
        PauseMenu.Hide();
        Engine.SetTimeScale(1);
    }
    
    // UI actions that are global to the scene should be handled here!!!
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("quit"))
        {
            if (InPauseMenu)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
}
