using System;
using Godot;
using Tachyogenesis.Scripts.UI;

namespace Tachyogenesis.Scripts;

public partial class Controller : Node2D
{
	public double Elapsed { get; private set; }
	private double _timeScale;
	public double TimeScale
	{
		get => _timeScale;
		set
		{
			_timeScale = Math.Clamp(value, -4D, 4D);
			Engine.TimeScale = (float)Math.Abs(_timeScale);
		}
	}
	public Camera2D Camera { get; private set; }
	public DialogBox Dialog { get; private set; }
	public Hud Hud { get; private set; }
    public Player Player { get; private set; }

	public string CurrentMap { get; private set; }
	public bool InCutscene { get; set; }

	public float ScaledPhysicsDelta(double delta) => (float)(delta * Math.Sign(TimeScale));

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		TimeScale = 1D;
		Elapsed = 0D;
		CurrentMap = $"{GetSceneFilePath().Split("/")[^2].ToLower()}_{GetSceneFilePath().Split("/")[^1].TrimSuffix(".tscn")}";
		Camera = GetNode<Camera2D>("Camera2D");
		Dialog = GetNode<DialogBox>("CanvasLayer/DialogBox");
		Hud = GetNode<Hud>("CanvasLayer/Hud");
        Player = GetNode<Player>("Player");

		Dialog.Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Elapsed += delta * Math.Sign(TimeScale);
	}
}
