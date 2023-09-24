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
        this.table = this.GetComponentInParent<TableController>();
    }

    public void _StartSelect()
    {
        print(this.visualizer.targetPosition);
        this.table.AddToSelected(this.visualizer);
    }

    public void _ContinueSelection()
    {
        if (!this.table.isSelectedStarted)
            return;
        this.table.AddToSelected(this.visualizer);
    }
}
