using Tachyogenesis.Scripts.Entity.StaticInteractableObject;

namespace Tachyogenesis.Scripts.Map.Facility._0;

public partial class ToF1Door : Door
{
	public ToF1Door()
	{
		Locked = false;
	}

	public override void Interact()
	{
		base.Interact();

		GetTree().ChangeSceneToFile("res://Scenes/Map/Facility/1.tscn");
	}
}
