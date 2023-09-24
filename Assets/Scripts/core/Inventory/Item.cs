using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(Item), menuName = Paths.ItemsDir + nameof(Item))]
public class Item : ScriptableObject
{
    public Sprite Image;
    public Color Color;
    public string Name;
    public SlotType slotType = SlotType.All;
}
