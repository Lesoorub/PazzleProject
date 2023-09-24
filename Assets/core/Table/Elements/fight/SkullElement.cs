using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skull", menuName = "core/Elements/Skull")]
public class SkullElement : TableElement
{
    public override void Destroy(TableController table, Vector2Int position)
    {
        var fightController = table as FightController;
        if (fightController == null) return;

        var enemy = fightController.Enemy;
        var damage = fightController.Me.person.BaseDamage;

        GibsController.Create(
            image, color, 
            table.GetPosition(position), 
            fightController.Enemy_InfoVisualizer.PersonImage.transform.position, 
            () =>
            {
                enemy.ApplyDamage(damage);
            }
        );
    }
}
