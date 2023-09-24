using UnityEngine;

[CreateAssetMenu(fileName = "Sword", menuName = "core/items/Sword")]
public class Sword : Item
{
    public float Damage;
    public Sword()
    {
        slotType = SlotType.Sword;
    }
}
