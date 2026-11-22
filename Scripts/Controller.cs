using System;
using Godot;
using Tachyogenesis.Data;
using Tachyogenesis.Scripts.UI;
using Tachyogenesis.Types;

namespace Tachyogenesis.Scripts;

public partial class Controller : Node2D
{
	// ----- NODES -----
	public Camera2D Camera { get; private set; }
	public DialogBox Dialog { get; private set; }
	public Hud Hud { get; private set; }
	public Player Player { get; private set; }
	public PauseMenu PauseMenu { get; private set; }
	
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
	public string CurrentMap { get; private set; }
	public ErrorHelper ErrorHelper { get; private set; } = new();
    public KeyHint.@Input InputMethod { get; set; }

    // ----- GAME BOOLS -----
    public bool InCutscene { get; set; }
    public bool InPauseMenu { get; private set; }
    
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

		Player.HealthChanged += _ => Hud.UpdateHealth(Player.Health);
		
		Dialog.Hide();
		ResumeGame(); // call to hide Pause Menu
		
		Player.SetHealth(Player.MaxHealth);
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
        switch (@event)
        {
            case InputEventMouse:
            case InputEventKey:
                InputMethod = KeyHint.@Input.PC;
                break;
            case InputEventJoypadMotion:
            case InputEventJoypadButton:
                InputMethod = Constants.IS_PLAYSTATION 
                    ? KeyHint.@Input.PS 
                    :KeyHint.@Input.Xbox;
                break;
            // Setting up PlayStation controllers requires knowing whether the connected controller *is* a PlayStation
            // controller, so it's better to just ask in the beginning which symbols to use / explicitly set it to
            // PlayStation in the PlayStation version via Constants
        }
        
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
