namespace Tachyogenesis.Scripts.Entity.NPC;

public partial class NPC : Entity
{
    protected NPCType NPCType { get; init; }

    protected NPC()
    {
        
    }
}
public enum NPCType
{
    None,
    Ally,
    Enemy
}