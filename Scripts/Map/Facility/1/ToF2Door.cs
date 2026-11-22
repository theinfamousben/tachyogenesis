using Tachyogenesis.Scripts.Entity.StaticInteractableObject;

namespace Tachyogenesis.Scripts.Map.Facility._1;

public partial class ToF2Door : Door
{
	public ToF2Door()
	{
		Locked = false;
	}

	public override void Interact()
	{
		base.Interact();

		GetTree().ChangeSceneToFile("res://Scenes/Map/Facility/2.tscn");
	}
}
