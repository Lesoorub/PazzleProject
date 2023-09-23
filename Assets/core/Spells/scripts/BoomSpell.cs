using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(BoomSpell), menuName = "core/Spells/" + nameof(BoomSpell))]
public class BoomSpell : Spell
{
    public int Radius = 1;
    public int AIPriority = 8;

    public override void Execute(FightController table, FightPerson caster, FightPerson enemy, SpellVisualizer sender)
    {
        table.AddSpellCastEvent((pos) =>
        {
            table.RemoveInRadius(pos.x, pos.y, Radius);
            table.EndStep();
        });
    }
    public override int GetAIPriority(FightPerson person)
    {
        return AIPriority;
    }
}
