using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ManaVisualizer : MonoBehaviour
{
    public Slider slider;
    public TMPro.TMP_Text text;

    public void Set(int cur, int max)
    {
        text.text = cur.ToString();
        slider.value = Mathf.Clamp01((float)cur / max);
    }
}
