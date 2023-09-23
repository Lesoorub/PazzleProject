using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ChainSpell), menuName = "core/Spells/" + nameof(ChainSpell))]
public class ChainSpell : Spell
{
    public override void Execute(
        FightController table,
        FightPerson caster,
        FightPerson enemy,
        SpellVisualizer sender)
    {
        table.AddSpellCastEvent((pos) =>
        {
            var el = table.Get(pos.x, pos.y);
            Stack<Vector2Int> stack = new Stack<Vector2Int>();
            stack.Push(pos);

            while (stack.Count > 0)
            {
                var t = stack.Pop();

                table.RemoveAt(t.x, t.y);
                for (int x = -1; x <= 1; x++)
                    for (int y = -1; y <= 1; y++)
                    {
                        if (x == 0 && y == 0) continue;
                        if (table.Get(t.x + x, t.y + y) != el) continue;
                        stack.Push(new Vector2Int(t.x + x, t.y + y));
                    }
            }

            table.EndStep();
        });
    }
}
