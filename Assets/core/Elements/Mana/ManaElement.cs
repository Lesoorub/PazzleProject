using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "mana element", menuName = "core/Elements/mana")]
public class ManaElement : TableElement
{
    public ManaType type = ManaType.air;
    public override void Destroy(TableController table, Vector2Int position)
    {
        var fightController = table as FightController;
        if (fightController == null) return;

        var me = fightController.Me;
        var visualizer = Get(fightController.Me_InfoVisualizer);
        UnityAction @do = () =>
        {
            me.AddMana(type, 1);
        };

        if (visualizer == null)
        {
            @do?.Invoke();
            return;
        }

        GibsController.Create(
            image, color,
            table.GetPosition(position),
            visualizer.transform.position,
            @do
        );
    }
    ManaVisualizer Get(PersonInfoVisualizer personInfoVisualizer)
    {
        switch (type)
        {
            case ManaType.air:
                return personInfoVisualizer.Air;
            case ManaType.fire:
                return personInfoVisualizer.Fire;
            case ManaType.plant:
                return personInfoVisualizer.Plant;
            case ManaType.water:
                return personInfoVisualizer.Water;
        }
        return null;
    }
}

public enum ManaType : byte
{
    air, fire, plant, water
}