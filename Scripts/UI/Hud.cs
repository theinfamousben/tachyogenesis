using Godot;

namespace Tachyogenesis.Scripts.UI;

public partial class Hud : Control
{
	private Controller controller;
	private RichTextLabel healthLabel;
	private Label timeScaleLabel;
	private Label elapsedTimeLabel;
	
	public override void _Ready()
	{
		controller = GetNode<Controller>("/root/Controller");
		healthLabel = GetNode<RichTextLabel>("Health");
		timeScaleLabel = GetNode<Label>("TimeScale");
		elapsedTimeLabel = GetNode<Label>("ElapsedTime");
	}

	public override void _Process(double delta)
	{
		UpdateHud();
	}

	// HUD Stuff that needs to get updated every tick
	private void UpdateHud()
	{
		timeScaleLabel.Text = FormatTimeScale(controller.TimeScale);
		elapsedTimeLabel.Text = FormatElapsedTime(controller.Elapsed);
	}

	public void Init(float playerHealth)
	{
		UpdateHud();
		UpdateHealth(playerHealth);
	}

	private static string FormatElapsedTime(double elapsed) => $"Elapsed Time: {elapsed:F2}";

	private static string FormatTimeScale(double scale) => $"Time Scale: {scale:F2}";

	private string FormatHealth(float health)
	{
		int h = (int)health;
		string part = h < 100
			? h < 10 
				?  h == 0 
					? "[color=gray]000[/color]"
					: $"[color=gray]00[/color]{h}"
				: $"[color=gray]0[/color]{h}"
			: h.ToString();

		return $"Health: {part}";
	}

	public void UpdateHealth(float health) => healthLabel.Text = FormatHealth(health);
}
