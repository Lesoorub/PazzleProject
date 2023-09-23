using UnityEngine;

public class SingeltonMonoBehaviour<T> : MonoBehaviour where T : Component
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = new GameObject($"[{nameof(T)}]");
                instance = (T)obj.AddComponent(typeof(T));
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }
}
