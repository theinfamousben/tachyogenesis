using System;
using Godot;

namespace Tachyogenesis.Scripts;

public partial class Controller : Node2D
{
    public Tuple<int, int> CameraBounds { get; set; }
	public float TimeScale { get; set; }
    public Camera2D Camera { get; set; }
    public string currentMap;

    public bool InCutscene;
	
	// Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print(GetSceneFilePath());
        currentMap = $"{GetSceneFilePath().Split("/")[^2].ToLower()}_{GetSceneFilePath().Split("/")[^1].TrimSuffix(".tscn")}";
        GD.Print(currentMap);
        Camera = GetNode<Camera2D>("Camera2D");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
