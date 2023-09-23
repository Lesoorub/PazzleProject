using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEffect : ScriptableObject
{
    public virtual void Execute(
        FightController table, 
        FightPerson caster, 
        FightPerson enemy, 
        SpellVisualizer sender, 
        float mulpilyFactor = 1)
    {
        Debug.Log("execute");
    }
}
