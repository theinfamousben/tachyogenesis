using Godot;

namespace Tachyogenesis.Scripts.Entity;

public partial class Entity : Node2D
{
    public bool Interactable { get; protected init;  }
    public float Health { get; private set; }
    public float MaxHealth { get; protected init; }
    public bool Invincible { get; set; }
    
    public Entity()
    {
        Health = MaxHealth;
        Invincible = false;
        Interactable = false;
    }

    public override void _Ready()
    {
        if (Interactable) AddToGroup("Interactable");
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
}