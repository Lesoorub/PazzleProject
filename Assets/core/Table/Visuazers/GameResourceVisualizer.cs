using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameResourceVisualizer : MonoBehaviour
{
    public Slider slider;
    public TMPro.TMP_Text text;
    public UserIntVariable variable;
    public UserIntVariable maximum;

    private void Awake()
    {
        variable.onChange?.AddListener(SetText);
        maximum?.onChange?.AddListener(SetText);
        SetText();
    }
    private void OnDestroy()
    {
        variable.onChange?.RemoveListener(SetText);
        maximum?.onChange?.RemoveListener(SetText);
    }
    public virtual void SetText()
    {
        text.text = variable.Data.ToString();
        slider.value =
            maximum != null ? 
            Mathf.Clamp01((float)variable.Data / maximum.Data) 
            : 0;
    }
}
