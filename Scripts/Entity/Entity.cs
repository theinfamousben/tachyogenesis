using Godot;

namespace Tachyogenesis.Scripts.Entity;

public partial class Entity : Node2D
{
    public Entity()
    {
        Health = MaxHealth;
    }
    
    public float Health { get; private set; }
    public float MaxHealth { get; private set; }

    public virtual void SetHealth(float health)
    {
        Health = Mathf.Clamp(0, MaxHealth, health);

        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void ChangeHealth(float dHealth) => SetHealth(Health + dHealth);

    public virtual void Die() => QueueFree();
}