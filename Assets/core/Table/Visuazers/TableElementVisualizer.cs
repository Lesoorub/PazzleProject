using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableElementVisualizer : MonoBehaviour
{
    TableController table;

    public Vector2Int targetPosition;
    public Image sprite;
    public UIOutline outline;
    public float Speed = 1;
    private void Start()
    {
        table = GetComponentInParent<TableController>();
        //outline.width = Mathf.Ceil((1080f / Screen.height) / transform.lossyScale.z);
    }
    public bool Selected
    {
        get => outline.gameObject.activeSelf;
        set => outline.gameObject.SetActive(value);
    }
    public Color32 SelectionColor
    {
        get => outline.color;
        set => outline.color = value;
    }
    public void SetElement(TableElement el)
    {
        sprite.sprite = el.image;
        sprite.color = el.color;
    }

    private void Update()
    {
        var newpos = table.GetPosition(targetPosition.x, targetPosition.y);
        transform.position = Vector2.Lerp(transform.position, newpos, Time.deltaTime * Speed);
    }
    public void _StartSelect()
    {
        table.AddToSelected(this);
    }
    public void _ContinueSelection()
    {
        if (!table.isSelectedStarted)
            return;
        table.AddToSelected(this);
    }
}
