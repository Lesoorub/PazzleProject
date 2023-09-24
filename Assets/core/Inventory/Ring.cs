using UnityEngine;

[CreateAssetMenu(fileName = "Ring", menuName = "core/items/Ring")]
public class Ring : Item
{
    public Ring()
    {
        slotType = SlotType.Ring;
    }
}