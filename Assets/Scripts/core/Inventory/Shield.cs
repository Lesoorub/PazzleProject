using UnityEngine;

[CreateAssetMenu(fileName = "Shield", menuName = "core/items/Shield")]
public class Shield : Item
{
    public float BlockDamage;
    public Shield()
    {
        slotType = SlotType.Shield;
    }
}
