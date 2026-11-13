namespace Tachyogenesis.Scripts.Entity.NPC;

public partial class NPC : Entity
{
    public  NPCType NPCType { get; protected init; }

    public NPC()
    {
        
    }
}
public enum NPCType
{
    None,
    Ally,
    Enemy
}