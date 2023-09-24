using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class FightAI
{
    public enum AIDifficulty
    {
        Easy, Medium, Hard
    }
    public AIDifficulty difficulty;
    TableElement skull;
    TableElement[] Mana;
    TableElement[,] tablecache;
    int w, h;
    public FightAI(TableElement[,] tablecache, TableElement skull, TableElement[] Mana, AIDifficulty difficulty)
    {
        this.difficulty = difficulty;
        this.skull = skull;
        this.Mana = Mana;
        this.tablecache = tablecache;
        w = tablecache.GetLength(0);
        h = tablecache.GetLength(1);
    }
    public void UpdateTable(TableController table)
    {
        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
                tablecache[x, y] = table.Get(x, y);
    }
    public SpellVisualizer GetNeededSpellToCast(FightPerson f_person, SpellVisualizer[] spells)
    {
        var priorities = new List<(int priority, SpellVisualizer spell)>();

        const int priorityThreashold = 5;

        foreach (var s in spells)
        {
            if (s.hasCooldown()) continue;
            if (!s.hasMana()) continue;
            var priority = s.spell.GetAIPriority(f_person);
            if (priority < priorityThreashold)
                continue;
            priorities.Add((priority, s));
        }
        //Debug.Log(string.Join(", ", priorities.Select(x => $"p={x.priority} name={x.spell.spell.name}")));
        if (priorities.Count == 0) return null;
        var maxPrior = priorities.Max(x => x.priority);
        return priorities.Where(x => x.priority == maxPrior).First().spell;
    }
    public IEnumerable<Vector2Int> GetAIStep()
    {
        var ways = GetBestPaths();

        if (ways.Count == 0)
        {
            return new List<Vector2Int>();
        }

        if (ways.Count == 1)
            return ways.First().Value;

        List<Vector2Int> path;

        switch (difficulty)
        {
            case AIDifficulty.Easy:
                {
                    int i = Random.Range(0, ways.Count);
                    foreach (var w in ways)
                    {
                        if (i == 0)
                            return w.Value;
                        i--;
                    }
                    return ways.First().Value;
                }
            default:
            case AIDifficulty.Medium:
                {
                    var max = ways.Max(x => x.Value.Count);
                    return ways.Where(x => x.Value.Count == max).First().Value;
                }
            case AIDifficulty.Hard:
                {
                    if (ways.TryGetValue(skull, out path))
                        return path;

                    //if (Mana.Any(x => ways.TryGetValue(x, out path)))
                    //    return path;

                    var max = ways.Max(x => x.Value.Count);
                    return ways.Where(x => x.Value.Count == max).First().Value;
                }
        }

    }

    public Dictionary<TableElement, List<Vector2Int>> GetBestPaths()
    {
        var allSteps = GetAllSteps();
        var result = new Dictionary<TableElement, List<Vector2Int>>();

        foreach (var path in allSteps)
        {
            if (path == null) continue;
            var el = path.element;

            if (result.TryGetValue(el, out var bestpath))
            {
                if (path.Count > bestpath.Count)
                    result[el] = path.path;
            }
            else
            {
                result.Add(el, path.path);
            }
        }

        return result;
    }

    public Path[,] GetAllSteps()
    {
        Path[,] result = new Path[w, h];
        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
            {
                var step = GetBestStepInPos(tablecache, x, y);
                if (step.Count < 3)
                    continue;
                result[x, y] = step;
            }
        return result;
    }
    public Path GetBestStepInPos(TableElement[,] tablecache, int x, int y)
    {
        TableElement startElement = tablecache[x, y];

        Path result = new Path(startElement, new List<Vector2Int>());
        if (startElement == null) return result;

        Stack<List<Vector2Int>> stack = new Stack<List<Vector2Int>>(16);
        stack.Push(new List<Vector2Int>() { new Vector2Int(x, y) });

        int iteration = 0;
        const int krit = 100;

        while (stack.Count > 0)
        {
            var path = stack.Pop();
            var last = path.Last();
            int adds = 0;
            for (int dy = -1; dy <= 1; dy++)
                for (int dx = -1; dx <= 1; dx++)
                {
                    if (dy == 0 && dx == 0) continue;
                    int fx = last.x + dx;
                    int fy = last.y + dy;
                    if (fx < 0 || fx >= w ||
                        fy < 0 || fy >= h) continue;
                    var g = tablecache[fx, fy];
                    var p = new Vector2Int(fx, fy);
                    if (g == null) continue;
                    if ((g.freeSelect || g == startElement) && !path.Contains(p))
                    {
                        var newpath = new List<Vector2Int>(path);
                        newpath.Add(p);
                        stack.Push(newpath);
                        adds++;
                    }
                }
            if (adds == 0 && path.Count > result.Count)
            {
                result = new Path(startElement, path);
            }
            if (iteration >= krit)
            {
                if (result == null)
                    result = new Path(startElement, stack.First());
                break;
            }
            iteration++;
        }

        return result;
    }

    public class Path
    {
        public TableElement element;
        public List<Vector2Int> path;
        public int Count => path.Count;

        public Path(TableElement element, List<Vector2Int> path)
        {
            this.element = element;
            this.path = path;
        }
        public Path()
        {
            this.element = null;
            this.path = new List<Vector2Int>();
        }
    }
}