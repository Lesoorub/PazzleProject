using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableElementHelper : MonoBehaviour
{
    TableController table;
    [Header("Set in inspector")]
    public TableElementVisualizer visualizer;
    private void Start()
    {
        table = GetComponentInParent<TableController>();
    }

    public void _StartSelect()
    {
        print(visualizer.targetPosition);
        table.AddToSelected(visualizer);
    }
    public void _ContinueSelection()
    {
        if (!table.isSelectedStarted)
            return;
        table.AddToSelected(visualizer);
    }
}
