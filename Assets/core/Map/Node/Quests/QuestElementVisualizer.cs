using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestElementVisualizer : MonoBehaviour
{
    public TMP_Text QuestNameText;
    public Button QuestOpenButton;
    public void _Init(Quest quest)
    {
        QuestNameText.text = quest.Name;
    }
}
