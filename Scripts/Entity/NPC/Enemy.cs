using Godot;

namespace Tachyogenesis.Scripts.Entity.NPC;

public partial class Enemy : NPC
{
    public Enemy()
    {
        NPCType = NPCType.Enemy;
    }

    public virtual void ScoutPlayer()
    {
        
    }
}