using Godot;

namespace Tachyogenesis.Scripts.UI;

public partial class Hud : Control
{
	private Controller controller;
	private Label healthLabel;
	private Label timeScaleLabel;
	private Label elapsedTimeLabel;
	
	public override void _Ready()
	{
		controller = GetNode<Controller>("/root/Controller");
		healthLabel = GetNode<Label>("Health");
		timeScaleLabel = GetNode<Label>("TimeScale");
		elapsedTimeLabel = GetNode<Label>("ElapsedTime");
	}

	public override void _Process(double delta)
	{
		UpdateHud();
	}

	private void UpdateHud()
	{
		timeScaleLabel.Text = FormatTimeScale(controller.TimeScale);
		elapsedTimeLabel.Text = FormatElapsedTime(controller.Elapsed);
	}

	private static string FormatElapsedTime(double elapsed) => $"Elapsed Time: {elapsed:F2}";

	private static string FormatTimeScale(double scale) => $"Time Scale: {scale:F2}";
}
