namespace Tachyogenesis.Scripts.Entity.NPC;

public partial class NPC : Entity
{

    public  NPCType NPCType { get; private set; }

    public NPC()
    {
        
    }

    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);
        
        DamageAnimation();
    }

    public override void Die()
    {
        DeathAnimation();
        
        base.Die();
    }

    public async void DamageAnimation()
    {
        
    }

    public void DeathAnimation()
    {
        
    }
}
public enum NPCType
{
    Ally,
    Enemy
}