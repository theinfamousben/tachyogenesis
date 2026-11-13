namespace Tachyogenesis.Scripts.Entity.StaticInteractableObject;

public partial class Door : StaticInteractableObject
{
	public bool Locked { get; set; } = false;
	public bool IsOpen { get; set; } = false;
	public bool Closable { get; set; } = true;

	public override void Interact()
	{
		if (Locked) return;
	}

	public void Open()
	{
		
	}

	public void Close()
	{
		
	}
}
