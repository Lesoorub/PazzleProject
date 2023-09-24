using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Main : MonoBehaviour
{
    public UserData userData;
    public float SaveIntervalInSeconds = 15;
    float lastsave;
    void Start()
    {
        Application.targetFrameRate = 60;
        userData.Load();
    }
    public void _ForceSave()
    {
        lastsave = Time.time;
        userData.Save();
    }
    private void Update()
    {
        if (lastsave == 0) return;
        if (Time.time - lastsave > SaveIntervalInSeconds)
        {
            _ForceSave();
        }
    }


    public void _SaveAndQuit()
    {
        userData.Save();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
