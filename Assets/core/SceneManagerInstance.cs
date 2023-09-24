using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneManagerInstance : MonoBehaviour
{
    public UnityEvent<string> OnCurrentSceneChaching = new UnityEvent<string>();
    public UnityEvent<string> OnCurrentSceneChached = new UnityEvent<string>();
    public void _LoadScene(string sceneName)
    {
        OnCurrentSceneChaching?.Invoke(sceneName);
        SceneManager.LoadScene(sceneName);
        OnCurrentSceneChached?.Invoke(sceneName);
    }

}
