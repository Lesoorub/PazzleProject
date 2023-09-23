using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteAlways]
public class Node : MonoBehaviour
{
    public bool alwaysVisible = false;
    [Space]
    public UITransformBasedLineRenderer lineRenderer;
    public TMPro.TMP_Text nodeName;
    public List<Node> connectedWith = new List<Node>();
    public UnityEvent OnClick = new UnityEvent();
    public UnityEvent OnInteract = new UnityEvent();

    private void Awake()
    {
        gameObject.SetActive(alwaysVisible);
    }
    private void OnValidate()
    {
#if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isPlaying)
            gameObject.SetActive(true);
#endif
        lineRenderer.points.Clear();
        foreach (var p in connectedWith)
        {
            if (p == null) continue;
            if (!p.gameObject.activeSelf) continue;
            lineRenderer.points.Add(transform);
            lineRenderer.points.Add(p.transform);
        }
        lineRenderer.SetVerticesDirty();
        nodeName.text = name;
    }
    private void OnDisable()
    {
        foreach (var p in connectedWith)
            p.OnValidate();
    }
    private void OnEnable()
    {
        foreach (var p in connectedWith)
            p.OnValidate();
    }
    public void _OnOpenInvoke() => OnClick?.Invoke();
}
