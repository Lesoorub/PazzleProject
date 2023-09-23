using UnityEngine;

[CreateAssetMenu(fileName = nameof(BoomSpellEffect), menuName = "core/Spells/Effects/" + nameof(BoomSpellEffect))]
public class BoomSpellEffect : SpellEffect
{
    public int Radius = 1;
    public override void Execute(
        FightController table,
        FightPerson caster,
        FightPerson enemy,
        SpellVisualizer sender,
        float mulpilyFactor = 1)
    {
        table.AddSpellCastEvent((pos) =>
        {
            table.RemoveInRadius(pos.x, pos.y, Mathf.FloorToInt(Radius * mulpilyFactor));
            table.EndStep();
        });
    }
}