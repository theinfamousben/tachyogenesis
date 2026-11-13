using Godot;

namespace Tachyogenesis.Scripts.Entity;

public partial class Entity : Node2D
{
	public Controller controller;
	public bool Interactable { get; protected init; }
	public float Health { get; private set; }
	public float MaxHealth { get; protected init; }
	public bool Invincible { get; set; }
	
	public AnimatedSprite2D sprite;
	public ShaderMaterial shaderMaterial;
	
	public Entity()
	{
		Health = MaxHealth;
		Invincible = false;
		Interactable = false;
	}

	public override void _Ready()
	{
		controller = GetNode<Controller>("/root/Controller");
		if (Interactable) AddToGroup("Interactable");
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		shaderMaterial = (ShaderMaterial)sprite.Material;
		
		SetOutline(false);
	}
	
	public virtual void SetHealth(float health)
	{
		Health = Mathf.Clamp(0, MaxHealth, health);

		if (health <= 0)
		{
			Die();
		}
	}

	public virtual void ChangeHealth(float dHealth) => SetHealth(Health + dHealth);
	
	public virtual void TakeDamage(float dmg) => ChangeHealth(-dmg);

	public virtual void Die() => QueueFree();

	public virtual void Interact() { }
	
	public virtual void SetOutline(bool enabled) => shaderMaterial.SetShaderParameter("enabled", enabled);
}
