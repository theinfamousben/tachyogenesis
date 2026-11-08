using Godot;

namespace Tachyogenesis.Scripts.Entity;

public partial class Entity : Node2D
{
    public bool Interactable { get; }
    public float Health { get; private set; }
    public float MaxHealth { get; private set; }
    public bool Invincible { get; }
    
    public Entity()
    {
        Health = MaxHealth;
        Invincible = false;
        Interactable = false;
    }

    public override void _Ready()
    {
        if (Interactable) AddToGroup("Interactable");
        
        OnReady();
    }
    
    // Use this start method for children, as _Ready() can only be overridden.
    public virtual void OnReady() { }
    
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
}