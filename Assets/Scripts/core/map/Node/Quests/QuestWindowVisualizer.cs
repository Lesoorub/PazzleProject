using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestWindowVisualizer : MonoBehaviour
{
    public QuestsController questsController;
    public TMP_Text DiscriptionText;
    Quest showedQuest;
    public void _Show(Quest quest)
    {
        showedQuest = quest;
        DiscriptionText.text = quest.Discription;
    }

    public void _AcceptQuest()
    {
        if (!questsController.Accepted.Contains(showedQuest))
        {
            questsController.AcceptQuest(showedQuest);
        }
    }
}
