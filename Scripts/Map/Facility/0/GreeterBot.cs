using Tachyogenesis.Data.Cutscenes.Facility;
using Tachyogenesis.Scripts.Entity.StaticInteractableObject;

namespace Tachyogenesis.Scripts.Map.Facility._0;

public partial class GreeterBot : CutsceneInitiator
{
	public async override void Interact()
	{
		controller.InCutscene = true;
		await new Facility0BotSequence().Play(this);
		controller.InCutscene = false;
	}

	public void AfterInteract()
	{
		
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		
		
	}
}
