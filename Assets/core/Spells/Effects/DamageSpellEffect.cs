using UnityEngine;

[CreateAssetMenu(fileName = nameof(DamageSpellEffect), menuName = "core/Spells/Effects/" + nameof(DamageSpellEffect))]
public class DamageSpellEffect : SpellEffect
{
    public override void Execute(
        FightController table,
        FightPerson caster,
        FightPerson enemy,
        SpellVisualizer sender,
        float mulpilyFactor = 1)
    {
        const float start_velocity = 32;

        for (int k = 0; k < mulpilyFactor; k++)
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
}
