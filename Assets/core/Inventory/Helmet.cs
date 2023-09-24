using UnityEngine;

[CreateAssetMenu(fileName = "Helmet", menuName = "core/items/Helmet")]
public class Helmet : Armor
{
    public Helmet()
    {
        slotType = SlotType.Helmet;
    }
}
