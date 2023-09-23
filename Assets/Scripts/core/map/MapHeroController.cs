using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapHeroController : MonoBehaviour
{
    [Header("Settings")]
    public float threashold = 1;
    public float Speed = 10;
    public GameObject MapObject;
    public UnityEvent<Node> OnMovementStaring = new UnityEvent<Node>();
    public UnityEvent<Node> OnMovementEnding = new UnityEvent<Node>();
    [Header("Info")]
    [Disable]
    [Tooltip("Sets automaticly by nearest node")]
    public Node currentNode;

    Coroutine currentDoing;

    void Start()
    {
        Load();
        OnMovementEnding?.AddListener((x) => Save());
        float min = float.PositiveInfinity;
        foreach (var node in MapObject.GetComponentsInChildren<Node>())
        {
            node.OnClick?.AddListener(() => GoToNode(node));

            float dist = Vector2.Distance(node.transform.position, transform.position);
            if(dist < min)
            {
                currentNode = node;
                min = dist;
            }
        }
        if (currentNode != null)
        {
            transform.position = currentNode.transform.position;
            OnMovementEnding?.Invoke(currentNode);
        }
    }

    public void GoToNode(Node node)
    {
        if (currentDoing != null) return;

        if (currentNode == null || currentNode.connectedWith.Contains(node))
        {
            currentDoing = StartCoroutine(Go(node));
            OnMovementStaring?.Invoke(node);
            return;
        }
        //Pathfinging algoritm start
        List<Node> GetPath(Node start, Node end)
        {
            Stack<List<Node>> stack = new Stack<List<Node>>();
            stack.Push(new List<Node>() { start });
            int iterations = 0;
            int maxiterations = 1000;
            while (stack.Count > 0)
            {
                var el = stack.Pop();
                var lastnode = el.Last();
                foreach (var n in lastnode.connectedWith)
                {
                    if (!n.gameObject.activeSelf) continue;
                    if (el.Contains(n)) continue;
                    if (n == end)
                    {
                        el.Add(n);
                        return el;
                    }
                    var newel = new List<Node>(el);
                    newel.Add(n);
                    stack.Push(newel);
                }
                iterations++;
                if (iterations > maxiterations)
                    return new List<Node>();
            }
            return new List<Node>();
        }

        StartCoroutine(GoByPath(GetPath(currentNode, node)));
    }

    IEnumerator GoByPath(List<Node> path)
    {
        print(string.Join(", ", path.Select(x => x.name)));
        if (path.Count <= 1) yield break;
        var end = path.Last();
        int index = 0;
        while (currentNode != end)
        {
            if (currentNode == path[index])
                index++;
            GoToNode(path[index]);
            yield return new WaitWhile(() => currentDoing != null);
            index++;
        }
    }

    public IEnumerator Go(Node targetNode)
    {
        var t = transform;

        while (Vector2.Distance(t.position, targetNode.transform.position) > threashold)
        {
            t.position = Vector2.MoveTowards(t.position, targetNode.transform.position, Time.deltaTime * Speed * t.lossyScale.z);
            yield return new WaitForEndOfFrame();
        }

        currentNode = targetNode;
        currentDoing = null;
        OnMovementEnding?.Invoke(currentNode);
    }

    public void _InteractWithCurrentNode()
    {
        Save();
        currentNode?.OnInteract?.Invoke();
    }

    public const string save_player_position_PATH = "map/player/position";
    public const string save_camera_position_PATH = "map/camera/position";

    public void Save()
    {
        if (currentDoing == null)
            PlayerPrefsHelper.SetVector2(save_player_position_PATH, transform.localPosition);
        PlayerPrefsHelper.SetVector2(save_camera_position_PATH, transform.parent.localPosition);
    }

    public void Load()
    {
        if (PlayerPrefsHelper.HasVector2(save_player_position_PATH))
            transform.localPosition = PlayerPrefsHelper.GetVector2(save_player_position_PATH);
        if (PlayerPrefsHelper.HasVector2(save_camera_position_PATH))
            transform.parent.localPosition = PlayerPrefsHelper.GetVector2(save_camera_position_PATH);
    }
}
