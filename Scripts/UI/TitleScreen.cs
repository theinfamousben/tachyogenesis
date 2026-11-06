using Godot;

namespace Tachyogenesis.Scripts.UI;

public partial class TitleScreen : Control
{
	private Button continueButton;
	private Button newGameButton;
	private Button loadGameButton;
	private Button settingsButton;
	private Button quitButton;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		continueButton = GetNode<Button>("ContinueGameButton");
		newGameButton = GetNode<Button>("NewGameButton");
		loadGameButton = GetNode<Button>("LoadGameButton");
		settingsButton = GetNode<Button>("SettingsButton");
		quitButton = GetNode<Button>("SettingsButton");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void _OnQuit()
	{
		GetTree().Quit();
	}

	public void _OnNewGame()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Intro.tscn");
	}
}
