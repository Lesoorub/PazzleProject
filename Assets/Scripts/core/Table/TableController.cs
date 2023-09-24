using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ���������� �������� ����.
/// </summary>
public class TableController : MonoBehaviour
{
    /// <summary>
    /// ��������� �������� �������� �� ���������.
    /// </summary>
    public bool AllowDiagonal = true;

    public RectTransform TableParent;
    public GameObject ElementPrefab;
    public List<TableElementWithChance> resources = new List<TableElementWithChance>();
    public UILineRenderer LineRenderer;

    /// <summary>
    /// ������� ��������� ��� ������ ���������.
    /// </summary>
    public UnityEvent<Vector2Int> OnStartSelect = new UnityEvent<Vector2Int>();

    /// <summary>
    /// ���������� ��� ��������� ���������.
    /// </summary>
    public UnityEvent<int> OnSelectComplete = new UnityEvent<int>();
    public Color SelectionColor = Color.green;

    protected TableElementInstance[,] table = new TableElementInstance[TABLE_SIZE_WIDTH, TABLE_SIZE_HEIGHT];
    List<SelectedTableElementInstance> selected = new List<SelectedTableElementInstance>();

    public const int TABLE_SIZE_WIDTH = 6;
    public const int TABLE_SIZE_HEIGHT = 8;
    const int SPACE_BETWEEN_ELEMENTS = 10;
    const int ELEMENT_SIZE = 150;

    /// <summary>
    /// ������ ��������� ���������.
    /// </summary>
    public IEnumerable<TableElement> Selected => 
        this.selected.Select(x => x.instance.element);

    public bool isSelectedStarted => this.selected.Count > 0;
    Vector2Int lastSelected;

    public bool LockInput = false;

    public int Space => (int)(SPACE_BETWEEN_ELEMENTS * this.TableParent.lossyScale.z);
    public int Size => (int)(ELEMENT_SIZE * this.TableParent.lossyScale.z);

    void Start()
    {
        this.InitTable();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            this.TableEndSelection();
        }
    }

    /// <summary>
    /// ��. <see cref="GetPosition(int, int)"/>
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public Vector2 GetPosition(Vector2Int a) =>
        this.GetPosition(a.x, a.y);

    /// <summary>
    /// ������ ������� ������� �������� � ����������� �� ��� �������.
    /// </summary>
    /// <param name="tx">�������������� ������.</param>
    /// <param name="ty">������������ ������.</param>
    /// <returns>������� � ������� ������ ������������� �������.</returns>
    public Vector2 GetPosition(int tx, int ty)
    {
        // ������ �� �������� ������� - ����� ����������.

        const int HW = TABLE_SIZE_WIDTH / 2;
        const int HH = TABLE_SIZE_HEIGHT / 2;
        return 
            (Vector2)this.TableParent.position
            + new Vector2(
                tx * this.Size + tx * this.Space, 
                ty * this.Size + ty * this.Space)
            - new Vector2(
                HW * this.Size + (HW - 1) * this.Space,
                HH * this.Size + (HH - 1) * this.Space)
            + Vector2.one * (this.Size + this.Space) / 2;
    }

    /// <summary>
    /// ������� ��������� ���������� ��������.
    /// </summary>
    /// <returns>��������� �������.</returns>
    /// <exception cref="System.Exception">���� �� ������ ��� ������, ������ ������� �������� ��������� ��������� �������� �����������</exception>
    TableElement GetRandom()
    {
        float sum = this.resources.Sum(x => x.weight);
        float r = Random.value * sum;
        for (int k = 0; k < this.resources.Count; k++)
        {
            r -= this.resources[k].weight;
            if (r <= 0)
                return this.resources[k].element;
        }
        throw new System.Exception("���� �� ������ ��� ������, ������ ������� �������� ��������� ��������� �������� �����������");
    }

    /// <summary>
    /// ������������� ����, ��������� ��������� ����.
    /// </summary>
    public void InitTable()
    {
        for (int y = 0; y < TABLE_SIZE_HEIGHT; y++)
            for (int x = 0; x < TABLE_SIZE_WIDTH; x++)
            {
                if (this.table[x, y] != null)
                {
                    Destroy(this.table[x, y].instance);
                }
                var t = this.Create(this.GetRandom());
                t.instance.transform.position = this.GetPosition(x, y) + this.TableParent.transform.position * Vector2.up;
                t.visualizer.targetPosition = new Vector2Int(x, y);
                this.table[x, y] = t;
            }
    }

    /// <summary>
    /// ������� ��������� �������� ����.
    /// </summary>
    /// <param name="el">������ ��������.</param>
    /// <returns>��������� �������� ����</returns>
    TableElementInstance Create(TableElement el)
    {
        var t = new TableElementInstance();
        t.element = el;
        t.instance = Instantiate(this.ElementPrefab, this.TableParent);
        t.visualizer = t.instance.GetComponent<TableElementVisualizer>();
        t.visualizer.SetElement(el);
        return t;
    }

    /// <summary>
    /// �������� ���������� ���������� � ���������.
    /// </summary>
    /// <param name="visualizer">���������� ���������� �������� ����.</param>
    public void AddToSelected(TableElementVisualizer visualizer)
    {
        if (this.LockInput) 
            return;

        if (this.selected.Count == 0 ||
            (
                this.lastSelected - visualizer.targetPosition
            ).sqrMagnitude <= 
                (
                    this.AllowDiagonal ? 2 : 1
                )
            )
        {
            var t = this.table[visualizer.targetPosition.x, visualizer.targetPosition.y];
            var selectedIndex= this.selected.FindIndex(x => x.instance == t);
            if (selectedIndex == -1)
            {
                if (this.selected.Count == 0 || 
                    (
                        this.selected.Count > 0 && 
                        (
                            t.element.freeSelect || 
                            this.selected[0].instance.element == t.element)
                        )
                    )
                {
                    this.LineRenderer.points.Add(
                        (this.GetPosition(t.visualizer.targetPosition) 
                        - (Vector2)this.TableParent.transform.position) 
                        / this.TableParent.lossyScale);
                    this.LineRenderer.SetVerticesDirty();
                    this.selected.Add(new SelectedTableElementInstance(this.selected.Count, t));
                    visualizer.Selected = true;
                    visualizer.SelectionColor = this.SelectionColor;
                    this.lastSelected = visualizer.targetPosition;

                    if (this.selected.Count == 1)
                    {
                        this.OnStartSelect?.Invoke(visualizer.targetPosition);
                    }
                }
            }
            else if (this.selected.Count > 1)
            {
                var last = this.selected.Last();
                if (last.id - this.selected[selectedIndex].id == 1)
                {
                    this.selected.Remove(last);
                    this.LineRenderer.points.RemoveAt(this.LineRenderer.points.Count - 1);
                    this.LineRenderer.SetVerticesDirty();
                    last.instance.visualizer.Selected = false;
                    this.lastSelected = this.selected.Last().instance.visualizer.targetPosition;
                }
            }
        }
    }

    /// <summary>
    /// ������������ ����� ���������. ������� ���������� �������� ���� ����� ������������������ ������ 2-��.
    /// </summary>
    public void TableEndSelection()
    {
        if (this.LockInput)
            return;

        this.LineRenderer.points.Clear();
        this.LineRenderer.SetVerticesDirty();

        if (this.selected.Count >= 2)
        {
            foreach (var s in this.selected)
            {
                s.instance.visualizer.Selected = false;
                this.RemoveAt(s.instance.visualizer.targetPosition.x, s.instance.visualizer.targetPosition.y);
            }
            this.OnSelectComplete?.Invoke(this.selected.Count);
        }
        else
        {
            foreach (var s in this.selected)
                s.instance.visualizer.Selected = false;
        }

        this.selected.Clear();
        for (int x = 0; x < TABLE_SIZE_WIDTH; x++)
        {
            this.TampRow(x);
            this.TrySpawnNew(x);
        }
    }

    /// <summary>
    /// ������������ ����� ����.
    /// </summary>
    public void EndStep()
    {
        this.OnSelectComplete?.Invoke(this.selected.Count);
        this.selected.Clear();

        for (int x = 0; x < TABLE_SIZE_WIDTH; x++)
        {
            this.TampRow(x);
            this.TrySpawnNew(x);
        }
    }

    /// <summary>
    /// ������� �������� � ������� ������������ ���������� �������.
    /// </summary>
    /// <param name="x">�������������� ������ ��������.</param>
    /// <param name="y">������������ ������ ��������.</param>
    /// <param name="r">������ ��������.</param>
    public void RemoveInRadius(int x, int y, int r)
    {
        for (int dx = -r; dx <= r; dx++)
            for (int dy = -r; dy <= r; dy++)
                this.RemoveAt(x + dx, y + dy);
    }

    /// <summary>
    /// ���������� ������ �������� ���� �� ��������� �������.
    /// </summary>
    /// <param name="x">�������������� ������ ��������.</param>
    /// <param name="y">������������ ������ ��������.</param>
    /// <returns>������ �������� ����.</returns>
    public TableElement Get(int x, int y)
    {
        if (
            x < 0 || 
            y < 0 || 
            x >= TABLE_SIZE_WIDTH || 
            y >= TABLE_SIZE_HEIGHT || 
            this.table[x, y] == null) 
            return null;

        return this.table[x, y].element;
    }

    /// <summary>
    /// ������ ��� �������� ���� �� ��������� �������� �������.
    /// </summary>
    /// <param name="a">������� ������� ��������.</param>
    /// <param name="b">������� ������� ��������.</param>
    public void Swap(Vector2Int a, Vector2Int b)
    {
        if (a.x < 0 || 
            a.y < 0 || 
            a.x >= TABLE_SIZE_WIDTH || 
            a.y >= TABLE_SIZE_HEIGHT) 
            return;

        if (b.x < 0 || 
            b.y < 0 || 
            b.x >= TABLE_SIZE_WIDTH || 
            b.y >= TABLE_SIZE_HEIGHT) 
            return;

        var t = this.table[a.x, a.y];
        this.table[a.x, a.y] = this.table[b.x, b.y];
        this.table[b.x, b.y] = t;

        this.table[a.x, a.y].visualizer.targetPosition = a;
        this.table[b.x, b.y].visualizer.targetPosition = b;
    }

    /// <summary>
    /// ������� ��������� �������� ���� �� ��������� �������.
    /// </summary>
    /// <param name="x">�������������� ������ ��������.</param>
    /// <param name="y">������������ ������ ��������.</param>
    public void RemoveAt(int x, int y)
    {
        if (x < 0 || 
            y < 0 || 
            x >= TABLE_SIZE_WIDTH || 
            y >= TABLE_SIZE_HEIGHT) 
            return;

        var t = this.table[x, y];

        if (t == null) 
            return;

        Destroy(t.instance);
        this.table[x, y] = null;
        var pos = new Vector2Int(x, y);
        t.element.Destroy(this, pos);
        t.element.OnDestroy?.Invoke(this, pos);
    }

    /// <summary>
    /// "������������" �������. ���������� ������� � ������� ������.
    /// </summary>
    /// <param name="x">������ �������.</param>
    public void TampRow(int x)
    {
        for (int k = 0; k < TABLE_SIZE_HEIGHT; k++)
        {
            if (this.table[x, k] != null) 
                continue;
            for (int t = k; t < TABLE_SIZE_HEIGHT; t++)
            {
                if (this.table[x, t] != null)
                {
                    this.table[x, k] = this.table[x, t];
                    this.table[x, k].visualizer.targetPosition = new Vector2Int(x, k);
                    this.table[x, t] = null;
                    break;
                }
            }
        }
    }

    /// <summary>
    /// �������� ������� ����� �������� � �������.
    /// </summary>
    /// <param name="x">������ �������.</param>
    public void TrySpawnNew(int x)
    {
        for (int y = 0; y < TABLE_SIZE_HEIGHT; y++)
        {
            if (this.table[x, y] == null)
            {
                var t = this.Create(this.GetRandom());
                t.instance.transform.position = this.GetPosition(x, y) + this.TableParent.transform.position * Vector2.up;
                t.visualizer.targetPosition = new Vector2Int(x, y);
                this.table[x, y] = t;
            }
        }
    }

    /// <summary>
    /// ��������� �������� ����.
    /// </summary>
    protected class TableElementInstance
    {
        public GameObject instance;
        public TableElement element;
        public TableElementWithChance random;
        public TableElementVisualizer visualizer;
    }

    /// <summary>
    /// ��������� ��������� �������� ����.
    /// </summary>
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

/// <summary>
/// ������������ ������ �������� ���� � ��� ����� ���������.
/// </summary>
[System.Serializable]
public class TableElementWithChance
{
    public TableElement element;
    public float weight = 1;
}