using System.Linq;
using UnityEngine;

[CreateAssetMenu(
    fileName = nameof(HealingSpellEffect), 
    menuName = "core/Spells/Effects/" + nameof(HealingSpellEffect))]
public class HealingSpellEffect : SpellEffect
{
    public Sprite HealingGibImage;
    public Color32 HealingGibColor;

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
                HealingGibImage, HealingGibColor,
                sender.transform.position,
                table.Me_InfoVisualizer.PersonImage.transform.position,
                () =>
                {
                    caster.ApplyDamage(-1);
                },
                Random.insideUnitCircle.normalized * start_velocity * table.transform.lossyScale.z
            );
        }
    }
}