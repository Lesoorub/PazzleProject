using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(Person), menuName = Paths.PlayerDir + nameof(Person))]
public class Person : ScriptableObject
{
    public string Name;

    public Sprite Image;
    public Color Color;

    public float BaseHealth = 50;
    public float BaseDamage = 1;

    public List<Spell> BaseSpells = new List<Spell>();
}
