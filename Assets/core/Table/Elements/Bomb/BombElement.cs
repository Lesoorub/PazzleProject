using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "bomb", menuName = "core/Elements/Bomb", order = 1)]
public class BombElement : TableElement
{
    public override void Destroy(TableController table, Vector2Int position)
    {
        table.RemoveInRadius(position.x, position.y, 1);
    }
}
