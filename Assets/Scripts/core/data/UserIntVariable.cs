using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = nameof(UserIntVariable), menuName = Paths.PlayerDir + nameof(UserIntVariable))]
public class UserIntVariable : ScriptableObject
{
    [SerializeField]
    private int data;
    public int Data { get => this.data; set { this.data = value; this.onChange?.Invoke(); } }
    public UnityEvent onChange = new UnityEvent();
    public void Add(int count) 
    {
        this.Data += count;
    }
}