using Godot;

namespace Tachyogenesis.Scripts.Entity.StaticInteractableObject;

public partial class StaticInteractableObject : Entity
{
    public Controller controller;
    
	public StaticInteractableObject()
	{
		Interactable = true;
		Invincible = true;
	}

    public override void _Ready()
    {
        base._Ready();
        
        controller = GetNode<Controller>("/root/Controller");
    }

}
