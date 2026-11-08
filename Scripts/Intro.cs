using Godot;
using Tachyogenesis.Data.Cutscenes;

namespace Tachyogenesis.Scripts;

public partial class Intro : Node2D
{
    public DialogBox Dialog { get; private set; }
    
    public override void _Ready()
    {
        Dialog = GetNode<DialogBox>("DialogBox");

        PlayIntroSequence();
    }

    private async void PlayIntroSequence()
    {
        await new IntroSequence().Play(this);
    }

    public void FinishCutscene()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Map/Facility/0.tscn");
    }

}
