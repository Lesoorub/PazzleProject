using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class TableController : MonoBehaviour
{
    public bool AllowDiagonal = true;

    public RectTransform TableParent;
    public GameObject ElementPrefab;
    public List<TableElementWithChance> resources = new List<TableElementWithChance>();
    public UILineRenderer LineRenderer;
    public UnityEvent<Vector2Int> OnStartSelect = new UnityEvent<Vector2Int>();
    public UnityEvent<int> OnSelectComplete = new UnityEvent<int>();

    public const int TableSizeWidth = 6;
    public const int TableSizeHeight = 8;
    protected TableElementInstance[,] table = new TableElementInstance[TableSizeWidth, TableSizeHeight];

    public Color SelectionColor = Color.green;
    List<SelectedTableElementInstance> selected = new List<SelectedTableElementInstance>();
    public IEnumerable<TableElement> Selected => selected.Select(x => x.instance.element);
    public bool isSelectedStarted => selected.Count > 0;
    Vector2Int lastSelected;

    public bool LockInput = false;

    public int space => (int)(10 * TableParent.lossyScale.z);
    public int size => (int)(150 * TableParent.lossyScale.z);

    void Start()
    {
        InitTable();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            TableEndSelection();
        }
    }
    public Vector2 GetPosition(Vector2Int a) => GetPosition(a.x, a.y);

    public Vector2 GetPosition(int tx, int ty)
    {
        int hw = TableSizeWidth / 2;
        int hh = TableSizeHeight / 2;
        return 
            (Vector2)TableParent.position
            + new Vector2(
                tx * size + tx * space, 
                ty * size + ty * space)
            - new Vector2(
                hw * size + (hw - 1) * space,
                hh * size + (hh - 1) * space)
            + Vector2.one * (size + space) / 2;
    }
    TableElement GetRandom()
    {
        float sum = resources.Sum(x => x.weight);
        float r = Random.value * sum;
        for (int k = 0; k < resources.Count; k++)
        {
            r -= resources[k].weight;
            if (r <= 0)
                return resources[k].element;
        }
        throw new System.Exception("Something wrong in random algoritm");
    }

    public void InitTable()
    {
        for (int y = 0; y < TableSizeHeight; y++)
            for (int x = 0; x < TableSizeWidth; x++)
            {
                if (table[x, y] != null)
                {
                    Destroy(table[x, y].instance);
                }
                var t = Create(GetRandom());
                t.instance.transform.position = GetPosition(x, y) + TableParent.transform.position * Vector2.up;
                t.visualizer.targetPosition = new Vector2Int(x, y);
                table[x, y] = t;
            }
    }
    TableElementInstance Create(TableElement el)
    {
        var t = new TableElementInstance();
        t.element = el;
        t.instance = Instantiate(ElementPrefab, TableParent);
        t.visualizer = t.instance.GetComponent<TableElementVisualizer>();
        t.visualizer.SetElement(el);
        return t;
    }

    public void AddToSelected(TableElementVisualizer visualizer)
    {
        if (LockInput) 
            return;
        if (selected.Count == 0 ||
            (lastSelected - visualizer.targetPosition).sqrMagnitude <= (AllowDiagonal ? 2 : 1))
        {
            var t = table[visualizer.targetPosition.x, visualizer.targetPosition.y];
            var selectedIndex= selected.FindIndex(x => x.instance == t);
            if (selectedIndex == -1)
            {
                if (selected.Count == 0 || 
                    (selected.Count > 0 && (t.element.freeSelect || selected[0].instance.element == t.element)))
                {
                    LineRenderer.points.Add((GetPosition(t.visualizer.targetPosition) - (Vector2)TableParent.transform.position) / TableParent.lossyScale);
                    LineRenderer.SetVerticesDirty();
                    selected.Add(new SelectedTableElementInstance(selected.Count, t));
                    visualizer.Selected = true;
                    visualizer.SelectionColor = SelectionColor;
                    lastSelected = visualizer.targetPosition;

                    if (selected.Count == 1)
                    {
                        OnStartSelect?.Invoke(visualizer.targetPosition);
                    }
                }
            }
            else if (selected.Count > 1)
            {
                var last = selected.Last();
                if (last.id - selected[selectedIndex].id == 1)
                {
                    selected.Remove(last);
                    LineRenderer.points.RemoveAt(LineRenderer.points.Count - 1);
                    LineRenderer.SetVerticesDirty();
                    last.instance.visualizer.Selected = false;
                    lastSelected = selected.Last().instance.visualizer.targetPosition;
                }
            }
        }
    }
    public void TableEndSelection()
    {
        if (LockInput)
            return;
        LineRenderer.points.Clear();
        LineRenderer.SetVerticesDirty();
        if (selected.Count < 3)
        {
            foreach (var s in selected)
                s.instance.visualizer.Selected = false;
        }
        else
        {
            foreach (var s in selected)
            {
                s.instance.visualizer.Selected = false;
                RemoveAt(s.instance.visualizer.targetPosition.x, s.instance.visualizer.targetPosition.y);
            }
            OnSelectComplete?.Invoke((int)(selected.Count * selected.Count * 0.25f));
        }
        selected.Clear();
        for (int x = 0; x < TableSizeWidth; x++)
        {
            TampRow(x);
            TrySpawnNew(x);
        }
    }
    public void EndStep()
    {
        OnSelectComplete?.Invoke((int)(selected.Count * selected.Count * 0.25f));
        selected.Clear();
        for (int x = 0; x < TableSizeWidth; x++)
        {
            TampRow(x);
            TrySpawnNew(x);
        }
    }
    public void RemoveInRadius(int x, int y, int r)
    {
        for (int dx = -r; dx <= r; dx++)
            for (int dy = -r; dy <= r; dy++)
                RemoveAt(x + dx, y + dy);
    }
    public TableElement Get(int x, int y)
    {
        if (x < 0 || y < 0 || x >= TableSizeWidth || y >= TableSizeHeight || table[x, y] == null) return null;
        return table[x, y].element;
    }
    public void Swap(Vector2Int a, Vector2Int b)
    {
        if (a.x < 0 || a.y < 0 || a.x >= TableSizeWidth || a.y >= TableSizeHeight) return;
        if (b.x < 0 || b.y < 0 || b.x >= TableSizeWidth || b.y >= TableSizeHeight) return;
        var t = table[a.x, a.y];
        table[a.x, a.y] = table[b.x, b.y];
        table[b.x, b.y] = t;

        table[a.x, a.y].visualizer.targetPosition = a;
        table[b.x, b.y].visualizer.targetPosition = b;
    }
    public void RemoveAt(int x, int y)
    {
        if (x < 0 || y < 0 || x >= TableSizeWidth || y >= TableSizeHeight) return;
        var t = table[x, y];
        if (t == null) return;
        Destroy(t.instance);
        table[x, y] = null;
        var pos = new Vector2Int(x, y);
        t.element.Destroy(this, pos);
        t.element.OnDestroy?.Invoke(this, pos);
    }
    public void TampRow(int x)
    {
        for (int k = 0; k < TableSizeHeight; k++)
        {
            if (table[x, k] != null) continue;
            for (int t = k; t < TableSizeHeight; t++)
            {
                if (table[x, t] != null)
                {
                    table[x, k] = table[x, t];
                    table[x, k].visualizer.targetPosition = new Vector2Int(x, k);
                    table[x, t] = null;
                    break;
                }
            }
        }
    }
    public void TrySpawnNew(int x)
    {
        for (int y = 0; y < TableSizeHeight; y++)
        {
            if (table[x, y] == null)
            {
                var t = Create(GetRandom());
                t.instance.transform.position = GetPosition(x, y) + TableParent.transform.position * Vector2.up;
                t.visualizer.targetPosition = new Vector2Int(x, y);
                table[x, y] = t;
            }
        }
    }

    protected class TableElementInstance
    {
        public GameObject instance;
        public TableElement element;
        public TableElementWithChance random;
        public TableElementVisualizer visualizer;
    }
    private class SelectedTableElementInstance
    {
        public int id;
        public TableElementInstance instance;

        public SelectedTableElementInstance(int id, TableElementInstance instance)
        {
            this.id = id;
            this.instance = instance;
        }
    }
}
[System.Serializable]
public class TableElementWithChance
{
    public TableElement element;
    public float weight = 1;
}