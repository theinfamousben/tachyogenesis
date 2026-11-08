using System.Threading.Tasks;
using Godot;
using Tachyogenesis.Scripts;
using Tachyogenesis.Scripts.Entity.StaticInteractableObject;

namespace Tachyogenesis.Data.Cutscenes.Facility;

public class Facility0BotSequence : Cutscene
{
    public override async Task Play(Node host)
    {
        if (host is not CutsceneInitiator cutscene)
            throw new System.ArgumentException("TerminalSequence requires a CutsceneInitiator host.");

        var dialog = cutscene.controller.Dialog;
        const string bot = "Bot";
        string athena = "???";
        
        dialog.Show();

        await dialog.Typewriter("Hello...?", athena);
        await dialog.WaitForKeypress();

        await dialog.Typewriter("[i][color=gray]The bot let out a beep and opened its mechanical eyes.[/color][/i]");
        await dialog.WaitForKeypress();

        await dialog.Typewriter("Woah! What are [i]you[/i] doing here?", bot);
        await dialog.WaitForKeypress();

        await dialog.Typewriter("I- uh...", athena);
        await dialog.WaitForKeypress();

        await dialog.Typewriter("Ah, no matter. I can't let myself get disturbed by everyone around me.", bot);
        await dialog.WaitForKeypress();

        await dialog.Typewriter("Uhm... I'm sorry to disturb you?", athena);
        await dialog.WaitForKeypress();

        await dialog.Typewriter("Apology accepted. Now what do you want?", bot);
        await dialog.WaitForKeypress();

        await dialog.Typewriter("What.. are you?", athena);
        await dialog.WaitForKeypress();

        await dialog.Typewriter("I'm Solstice, a friendly bot to help researchers with their work. What are you?", bot);
        await dialog.WaitForKeypress();

        await dialog.Typewriter("I'm...", athena);
        await dialog.WaitForKeypress();

        await dialog.Typewriter(
            "[i][color=gray]You try to think of your name, but your mind draws a blank.[/color][/i]");
        await dialog.WaitForKeypress();

        await dialog.Typewriter("I don't know...", bot);
        await dialog.WaitForKeypress();

        await dialog.Typewriter(
            "Biological patterns indicate you of homo-sapien class, but I have been told on December 4th, 2024 that this information is unhelpful.",
            bot);
        await dialog.WaitForKeypress();

        await dialog.Typewriter("You seem to be coming from one of the Temporal Deceleration Chambers. Should I check the logs to deduce your identity?", bot);
        await dialog.WaitForKeypress();

        await dialog.Typewriter("Uhh- Yes, please...", athena);
        await dialog.WaitForKeypress();

        await dialog.Typewriter("Okay, checking. This might take a while.", bot);
        await dialog.WaitForKeypress();

        await dialog.Typewriter(
            "[i][color=gray]The bot seemed to be completely indifferent to his surroundings - but you figure that's what he looks like when he's busy.[/color][/i]");
        await dialog.WaitForKeypress();

        await dialog.Typewriter(
            "Found one release from a Temporal Deceleration Chamber in the last 194 years. Assuming you fit that criteria, as all other possible matches are likely to be deceased at this time.",
            bot);
        await dialog.WaitForKeypress();

        await dialog.Typewriter("Your name appears to be Athena Clayton; daughter of Dr. Alexander Clayton, Chief Researcher in the Temporal Manipulation Division.", bot);
        await dialog.WaitForKeypress();

        await dialog.Typewriter(
            "[i][color=gray]You frown as you hear your name. You know it is yours, you've been called that your entire life. Why don't you remember?[/color][/i]");
        await dialog.WaitForKeypress();
        
        athena = "Athena";
        await dialog.Typewriter("...Athena?", athena);
        await dialog.WaitForKeypress();

        await dialog.Typewriter("Correct. Can I assist you in anything else?", bot);
        await dialog.WaitForKeypress();

        await dialog.Typewriter(
            "I don't know anything about myself - Why don't I know anything about myself!?",
            athena);
        await dialog.WaitForKeypress();

        await dialog.Typewriter(
            "I am sensing that your are feeling uneasy. May I suggest calming down in order to prevent psychological damage to your body?",
            bot);
        await dialog.WaitForKeypress();

        await dialog.Typewriter(
            "It seems your long stay in the Temporal Deceleration Chamber has left you with retrograde amnesia. I am certain it will wear off in time, but I don't know how long that will be.",
            bot);
        await dialog.WaitForKeypress();

        await dialog.Typewriter("[i][color=gray]Ending cutscene early as the rest is unfinished.[/color][/i]");
        await dialog.WaitForKeypress();
        
        dialog.Hide();
        cutscene.QueueFree();
    }
}