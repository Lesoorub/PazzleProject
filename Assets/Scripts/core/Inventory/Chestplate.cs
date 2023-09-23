using UnityEngine;

[CreateAssetMenu(fileName = "Chestplate", menuName = "core/items/Chestplate")]
public class Chestplate : Armor
{
    public Chestplate()
    {
        slotType = SlotType.Chestplate;
    }
}
