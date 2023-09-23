using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KillQuest : Quest
{
    public List<Target> targets = new List<Target>();

    public override void _Update(QuestsController questsController)
    {

    }

    [System.Serializable]
    public class Target
    {
        public Person enemy;
        public Node where;
    }
}