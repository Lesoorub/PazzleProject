using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class TavernVisualizer : MonoBehaviour
{
    public QuestsController QuestsController;
    public QuestWindowVisualizer QuestWindow;
    public GameObject QuestPrefab;
    public Transform QuestPrefabParent;
    public void _Open(Tavern tavern)
    {
        gameObject.SetActive(true);
        for (int k = 0; k < QuestPrefabParent.childCount; k++)
            Destroy(QuestPrefabParent.GetChild(k));
        foreach (var q in tavern.quests
            .Where(x => 
            QuestsController.Completed.Contains(x) || 
            QuestsController.Accepted.Contains(x)))
        {
            var obj = Instantiate(QuestPrefab, QuestPrefabParent);
            var visualizer = obj.GetComponent<QuestElementVisualizer>();
            visualizer._Init(q);
            visualizer.QuestOpenButton.onClick.AddListener(() => OpenQuestWindow(q));
        }
    }

    void OpenQuestWindow(Quest q)
    {
        QuestWindow.gameObject.SetActive(true);
        QuestWindow._Show(q);
    }
}
