using System;
using System.Threading.Tasks;
using Godot;

namespace Tachyogenesis.Scripts;

public partial class Intro : Node2D
{
    [Signal] public delegate void KeypressEventHandler();
    private bool typing;
    private bool firstKeypressEvent;
    private Control dialogBox;
    private RichTextLabel dialogText;
    private RichTextLabel authorText;
    private ColorRect authorContainer;
    private float fastForwardMultiplier = 4.0f;
    private bool fastForward = false;
    
    public override void _Ready()
    {
        Keypress += AttemptSkipDialogue;
        
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
        
        await WaitForSeconds(1f);
        dialogBox.Show();
        await WaitForSeconds(0.5f);
        
        await Typewriter("[i][color=gray]You drowsily open your eyes.[/color][/i]");
        StartHintTimer();
        await WaitForKeypress();
        firstKeypressEvent = true;
        
        await Typewriter("[i]... What is this place?[/i]", "[Glitch text]");
        await WaitForKeypress();

        await Typewriter("[i][color=gray]The cryogenic hibernation pod from which you emerged let out a tired beep and whirred into standby.[/color][/i]");
        await WaitForKeypress();

        await Typewriter("...", "[Glitch Text]", 0.33f);
        await WaitForKeypress();

        await Typewriter("[i][color=gray]You look around, nervous and confused.[/color][/i]");
        await WaitForKeypress();

        GetTree().ChangeSceneToFile("res://Scenes/Map/Facility/0.tscn");
    }

    public async Task WaitForKeypress() => await ToSignal(this, SignalName.Keypress);

    public async void StartHintTimer()
    {
        await WaitForSeconds(5f);
        if (firstKeypressEvent) return;
        dialogText.Text += "\n[font_size=16][i]Press Enter/E to continue.[/i][/font_size]";
    }
    
	public async Task WaitForSeconds(float seconds) => await ToSignal(GetTree().CreateTimer(seconds), SceneTreeTimer.SignalName.Timeout);

    public void AttemptSkipDialogue()
    {
        if (typing) typing = false;
    }
    
    public async Task Typewriter(string msg, string author = null, float delta = 0.1f)
    {
        typing = true;
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
            if (!typing) break;
            dialogText.VisibleCharacters = i + 1;

            await WaitForSeconds(delta / (fastForward ? fastForwardMultiplier : 1));
        }

        dialogText.VisibleCharacters = -1;
        typing = false;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("interact"))
        {
            EmitSignal(SignalName.Keypress);
        }

        if (@event.IsActionPressed("exit"))
        {
            fastForward = true;
        }

        if (@event.IsActionReleased("exit"))
        {
            fastForward = false;
        }
    }
}
