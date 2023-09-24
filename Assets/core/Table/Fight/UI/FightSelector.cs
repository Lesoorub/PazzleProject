using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightSelector : MonoBehaviour
{
    public static Person NextFightTarget;
    public Person @default;
    public FightController fightController;

    public void Start()
    {
        if (@default != null)
            fightController?.CreateFight(NextFightTarget ?? @default);
    }

    public void _SetNextFightTarget(Person person)
    {
        NextFightTarget = person;
    }
}
