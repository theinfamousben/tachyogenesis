namespace Tachyogenesis.Scripts.Entity.NPC;

public partial class NPC : Entity
{

    public  NPCType NPCType { get; private set; }

    public NPC()
    {
        
    }
}
public enum NPCType
{
    Ally,
    Enemy
}