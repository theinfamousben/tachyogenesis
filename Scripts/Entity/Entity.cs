using System;
using Godot;
using Tachyogenesis.Types;

namespace Tachyogenesis.Scripts.Entity;

public partial class Entity : CharacterBody2D
{
	public Controller controller;
	public bool Interactable { get; protected init; }
	public float Health { get; private set; }
	public float MaxHealth { get; protected init; }
	public bool Invincible { get; set; }
	
	public AnimatedSprite2D sprite;
	public ShaderMaterial shaderMaterial;
	private bool outlineEnabled = true;
	
	public Entity()
	{
		Invincible = false;
		Interactable = false;
	}

	public override void _Ready()
	{
		Health = MaxHealth;
		
		controller = GetNode<Controller>("/root/Controller");
		if (Interactable) AddToGroup("Interactable");
		sprite = GetNode<AnimatedSprite2D>("Sprite");
		shaderMaterial = (ShaderMaterial)sprite.Material.Duplicate();
		sprite.Material = shaderMaterial;
		
		SetOutline(false);
	}
	
	public virtual void SetHealth(float health)
	{
		Health = Mathf.Clamp(health, 0, MaxHealth );

		if (health <= 0)
		{
			Die();
		}
	}

	public virtual void ChangeHealth(float dHealth) => SetHealth(Health + dHealth);
	
	public virtual void TakeDamage(float dmg) => ChangeHealth(-dmg);

	public virtual void Die() => QueueFree();

	public virtual void Interact() { }
	
	public virtual void SetOutline(bool enabled)
	{
		if (enabled)
		{
			if (HasNode("KeyHint")) return;
			
			var instance = GD.Load<PackedScene>("res://Prefabs/KeyHint.tscn").Instantiate();
			instance.Name = "KeyHint";
			var spriteNode = instance.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			spriteNode.Scale = new Vector2(0.67f, 0.67f);
			spriteNode.Position = spriteNode.Position with { Y = -32f };
			spriteNode.Animation = KeyHint.InputToString[controller.InputMethod]; 
			spriteNode.Frame = KeyHint.GetKey("interact", controller.InputMethod);
			AddChild(instance);
			
		}
		else if (HasNode("KeyHint"))
		{
			GetNode<Node2D>("KeyHint").Free();
		}

		if (outlineEnabled == enabled) return;

		outlineEnabled = enabled;
		try
		{
			shaderMaterial.SetShaderParameter("enabled", enabled);
		}
		catch (NullReferenceException)
		{
			controller.ErrorHelper.ThrowOnce(Errors.OutlineMaterialNotSet, this.ToString()); 
		}
	}
}
