using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Quest : MonoBehaviour
{
    public string Name;
    [TextArea(10,10)]
    public string Discription;

    public List<Quest> neededQuests = new List<Quest>();

    public UnityEvent OnAccept = new UnityEvent();
    public UnityEvent OnComplete = new UnityEvent();
    public UnityEvent OnDismiss = new UnityEvent();

    public virtual void _Update(QuestsController questsController) { }
}
