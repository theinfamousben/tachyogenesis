using Godot;

namespace Tachyogenesis.Scripts.Entity.StaticInteractableObject;

public partial class StaticInteractableObject : Entity
{
	
	public StaticInteractableObject()
	{
		Interactable = true;
		Invincible = true;
	}

	public override void _Ready()
	{
		base._Ready();
	}

}
