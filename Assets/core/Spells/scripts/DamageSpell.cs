using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(DamageSpell), menuName = "core/Spells/" + nameof(DamageSpell))]
public class DamageSpell : Spell
{
    public float BaseDamage = 1;
    public int AIPriority = 9;

    public override void Execute(FightController table, FightPerson caster, FightPerson enemy, SpellVisualizer sender)
    {
        const float start_velocity = 32;

        for (int k = 0; k < BaseDamage; k++)
        {
            GibsController.Create(
                table.Skull.image, table.Skull.color,
                sender.transform.position,
                table.Enemy_InfoVisualizer.PersonImage.transform.position,
                () =>
                {
                    enemy.ApplyDamage(1);
                },
                Random.insideUnitCircle.normalized * start_velocity * table.transform.lossyScale.z
            );
        }
    }
    public override int GetAIPriority(FightPerson person)
    {
        return AIPriority;
    }
}

