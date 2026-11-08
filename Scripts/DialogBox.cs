using System.Linq;
using System.Threading.Tasks;
using Godot;
namespace Tachyogenesis.Scripts;

public partial class DialogBox : Control
{
	[Signal] public delegate void KeypressEventHandler();
	private bool typing;
	private bool firstKeypressEvent;
	private RichTextLabel dialogText;
	private RichTextLabel authorText;
	private ColorRect authorContainer;
	private const float FastForwardMultiplier = 4.0f;
	private bool fastForward;

	public override void _Ready()
	{
		dialogText = GetNode<RichTextLabel>("Message");
		authorText = GetNode<RichTextLabel>("AuthorContainer/Author");
		authorContainer = GetNode<ColorRect>("AuthorContainer");

		Hide();
		authorContainer.Hide();
		dialogText.Text = "";
		Keypress += AttemptSkip;
	}

	public async Task WaitForKeypress() => await ToSignal(this, SignalName.Keypress);

	public async void StartHintTimer()
	{
		await WaitForSeconds(5f);
		if (firstKeypressEvent) return;
		dialogText.Text += "\n[font_size=16][i]Press Enter/E to continue.[/i][/font_size]";
	}

	public async Task Typewriter(string message, string author = null, float delta = 0.05f)
	{
		typing = true;
		dialogText.VisibleCharacters = 0;
		dialogText.Text = message;

		if (author == null)
		{
			authorContainer.Hide();
		}
		else
		{
			authorText.Text = author;
			authorContainer.Show();
		}

		for (int i = 0; i < message.Length; i++)
		{
			if (!typing) break;
			dialogText.VisibleCharacters = i + 1;
			float time = new[] {".", ";", "-", "!", "?"}.Contains(message[i].ToString()) 
				? delta * 3 
				: delta ;
			await WaitForSeconds(time / (fastForward ? FastForwardMultiplier : 1));
		}

		dialogText.VisibleCharacters = -1;
		typing = false;
	}

	public void MarkFirstKeypressHandled()
	{
		firstKeypressEvent = true;
	}

	private void AttemptSkip()
	{
		if (typing) typing = false;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("interact"))
		{
			EmitSignal(SignalName.Keypress);
		}

		if (@event.IsActionPressed("exit")) fastForward = true;
		if (@event.IsActionReleased("exit")) fastForward = false;
	}

	public async Task WaitForSeconds(float seconds)
	{
		await ToSignal(GetTree().CreateTimer(seconds), SceneTreeTimer.SignalName.Timeout);
	}
}
