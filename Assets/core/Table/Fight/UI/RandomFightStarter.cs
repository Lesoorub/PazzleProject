using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFightStarter : MonoBehaviour
{
    public List<Person> Enemies = new List<Person>();

    public FightController fightController;
    public bool StartFightOnStartScene = false;

    private void Start()
    {
        if (StartFightOnStartScene)
            _StartFight();
    }

    public void _StartFight()
    {
        if (Enemies.Count == 0 || fightController == null) return;

        fightController.CreateFight(Enemies[Random.Range(0, Enemies.Count)]);
    }
}
