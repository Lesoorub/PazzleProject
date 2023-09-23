using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : ScriptableObject
{
    public Sprite Image;
    public Color32 Color = new Color32(255, 255, 255, 255);

    [Min(0)]
    public int Cooldown = 3;
    public bool CostStep = false;
    public bool NeedTarget = false;

    [Min(0)]
    public int Air;
    [Min(0)]
    public int Fire;
    [Min(0)]
    public int Plant;
    [Min(0)]
    public int Water;

    public int AiPriority = 9;

    public virtual void Execute(FightController table, FightPerson caster, FightPerson enemy, SpellVisualizer sender)
    {
        Debug.Log("execute");
    }
    public virtual int GetAIPriority(FightPerson person)
    {
        return AiPriority;
    }
}
