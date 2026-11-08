using System.Threading.Tasks;
using Godot;
using Tachyogenesis.Scripts;

namespace Tachyogenesis.Data.Cutscenes;

public class IntroSequence : Cutscene
{
    public override async Task Play(Node host)
    {
        if (host is not Intro intro)
        {
            throw new System.ArgumentException($"{nameof(IntroSequence)} requires an {nameof(Intro)} host.", nameof(host));
        }

        var dialog = intro.Dialog;

        await dialog.WaitForSeconds(1f);
        dialog.Show();
        await dialog.WaitForSeconds(0.5f);

        await dialog.Typewriter("[i][color=gray]You drowsily open your eyes.[/color][/i]");
        dialog.StartHintTimer();
        await dialog.WaitForKeypress();
        dialog.MarkFirstKeypressHandled();

        await dialog.Typewriter("[i]... What is this place?[/i]", "???");
        await dialog.WaitForKeypress();

        await dialog.Typewriter(
            "[i][color=gray]The cryogenic hibernation pod from which you emerged let out a tired beep and whirred into standby.[/color][/i]");
        await dialog.WaitForKeypress();

        await dialog.Typewriter("...", "???", 0.33f);
        await dialog.WaitForKeypress();

        await dialog.Typewriter("[i][color=gray]You look around, nervous and confused.[/color][/i]");
        await dialog.WaitForKeypress();

        intro.FinishCutscene();
    }
}