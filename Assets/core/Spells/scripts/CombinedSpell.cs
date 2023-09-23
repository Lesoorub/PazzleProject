using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(CombinedSpell), menuName = "core/Spells/" + nameof(CombinedSpell))]
public class CombinedSpell : Spell
{
    public List<SpellWithFactor> contains = new List<SpellWithFactor>();

    public override void Execute(FightController table, FightPerson caster, FightPerson enemy, SpellVisualizer sender)
    {
        foreach (var pair in contains)
            pair.spellEffect.Execute(table, caster, enemy, sender, pair.Multiply);
    }

    [System.Serializable]
    public class SpellWithFactor
    {
        public SpellEffect spellEffect;
        public float Multiply = 1;
    }
}
