using Godot;

namespace Tachyogenesis.Scripts.UI;

public partial class PauseMenu : Control
{
	private Controller _controller;
	private Label _versionLabel;
	
	//temp
	private Button _settingsButton;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_controller = GetNode<Controller>("/root/Controller");
		_settingsButton = GetNode<Button>("SettingsButton");
		_versionLabel = GetNode<Label>("VersionLabel");

		_versionLabel.Text = $"Version {ProjectSettings.GetSetting("application/config/version", "???")}";
	}


	private void OnContinueButton()
	{
		_controller.ResumeGame();
	}

	private void OnQuitButton()
	{
		GetTree().Quit();
	}

	private void OnSettingsButton()
	{
		_settingsButton.Text = "Under Construction";
	}
}
