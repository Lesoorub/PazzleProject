using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "new element", menuName = "core/Elements/Element", order = 1)]
public class TableElement : ScriptableObject
{
    public Sprite image;
    public Color32 color;
    public bool freeSelect = false;

    public UnityEvent<TableController, Vector2Int> OnDestroy = new UnityEvent<TableController, Vector2Int>();

    public virtual void Destroy(TableController table, Vector2Int position)
    {

    }
}
