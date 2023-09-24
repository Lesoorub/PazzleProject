using UnityEngine;

[CreateAssetMenu(fileName = "boots", menuName = "core/items/boots")]
public class Boots : Armor
{
    public Boots()
    {
        slotType = SlotType.Boots;
    }
}
