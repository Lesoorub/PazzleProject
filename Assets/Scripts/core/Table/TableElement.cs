using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = nameof(TableElement), menuName = Paths.ElementsDir + nameof(TableElement))]
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
