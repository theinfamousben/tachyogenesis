using System.Threading.Tasks;
using Godot;

namespace Tachyogenesis.Scripts;

public partial class Intro : Node2D
{
    [Signal] public delegate void KeypressEventHandler();
    private bool firstKeypressEvent;
    private Control dialogBox;
    private RichTextLabel dialogText;
    private RichTextLabel authorText;
    private ColorRect authorContainer;
    
    public override void _Ready()
    {
        // --- INITIALIZATION ---
        dialogBox = GetNode<Control>("DialogBox");
        dialogText = GetNode<RichTextLabel>("DialogBox/Message");
        authorText = GetNode<RichTextLabel>("DialogBox/AuthorContainer/Author");
        authorContainer = GetNode<ColorRect>("DialogBox/AuthorContainer");
        
        dialogBox.Hide();
        authorContainer.Hide();
        dialogText.Text = "";

        IntroSequence();
    }

    public async void IntroSequence()
    {
        firstKeypressEvent = false;
        
        await WaitForSeconds(2f);
        dialogBox.Show();
        await WaitForSeconds(0.5f);
        
        await Typewriter("[i][color=gray]You drowsily open your eyes.[/color][/i]");
        Hint();
        
        await ToSignal(this, SignalName.Keypress);
        firstKeypressEvent = true;
        
        await Typewriter("[i]... What is this place?[/i]", "[Glitch text]");
    }

    public async void Hint()
    {
        await WaitForSeconds(10f);
        if (firstKeypressEvent) return;
        dialogText.Text += "\n[font_size=16][i]Press Enter/E to continue.[/i][/font_size]";
    }
    
	public async Task WaitForSeconds(float seconds) => await ToSignal(GetTree().CreateTimer(seconds), SceneTreeTimer.SignalName.Timeout);

    public async Task Typewriter(string msg, string author = null, float delta = 0.1f)
    {
        dialogText.VisibleCharacters = 0;
        dialogText.Text = msg;
        
        if (author == null)
        {
            authorContainer.Hide();
        }
        else
        {
            authorText.Text = author;
            authorContainer.Show();
        }
        
        for (int i = 0; i < msg.Length; i++)
        {
            dialogText.VisibleCharacters = i + 1;

            await WaitForSeconds(delta);
        }

        dialogText.VisibleCharacters = -1;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("interact"))
        {
            EmitSignal(SignalName.Keypress);
        }
    }
}
