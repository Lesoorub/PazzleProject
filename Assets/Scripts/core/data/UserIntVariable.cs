using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "IntVariable", menuName = "core/User/Int variable", order = 3)]
public class UserIntVariable : ScriptableObject
{
    [SerializeField]
    private int data;
    public int Data { get => data; set { data = value; onChange?.Invoke(); } }
    public UnityEvent onChange = new UnityEvent();
    public void Add(int count) 
    { 
        Data += count;
    }
}