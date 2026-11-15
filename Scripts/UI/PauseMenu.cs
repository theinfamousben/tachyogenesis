using Godot;

namespace Tachyogenesis.Scripts.UI;

public partial class PauseMenu : Control
{
	private Controller _controller;
	
	//temp
	private Button _settingsButton;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_controller = GetNode<Controller>("/root/Controller");
		_settingsButton = GetNode<Button>("SettingsButton");
	}


	public void OnContinueButton()
	{
		_controller.ResumeGame();
	}

	public void OnQuitButton()
	{
		GetTree().Quit();
	}

	public void OnSettingsButton()
	{
		_settingsButton.Text = "Under Construction";
	}
}
