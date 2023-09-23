using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestsController : MonoBehaviour
{
    public List<Quest> Completed = new List<Quest>();
    public List<Quest> Accepted = new List<Quest>();
    public void _UpdateQuests()
    {
        foreach (var q in Accepted)
        {

        }
    }

    public void AcceptQuest(Quest q)
    {
        Accepted.Add(q);
    }
}
