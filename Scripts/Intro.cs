using System;
using System.Collections;
using System.Net.Mime;
using System.Threading.Tasks;
using Godot;

namespace Tachyogenesis.Scripts;

public partial class Intro : Node2D
{
    public NinePatchRect DialogBox;
    public Label DialogText;
    
    public override void _Ready()
    {
        // --- INITIALIZATION ---
        DialogBox = GetNode<NinePatchRect>("DialogBox");
        DialogText = GetNode<Label>("DialogBox/Label");
        
        DialogBox.Hide();
        DialogText.Text = "";
        
        _ = IntroSequence();
    }
    
	public async Task IntroSequence()
	{
		await WaitForSeconds(2f);
        DialogBox.Show();
	}
    
	public async Task WaitForSeconds(float seconds)
    {
        await ToSignal(GetTree().CreateTimer(seconds), SceneTreeTimer.SignalName.Timeout);
    }

    public async Task Typewriter(string msg, float delta)
    {
        for (int i = 0; i < msg.Length; i++)
        {
            await Type(msg, i);

            await WaitForSeconds(delta);
        }   
    }

    public async Task Type(string msg, int i)
    {
        DialogText.Text = msg.Substring(0, i + 1);
    }
}
