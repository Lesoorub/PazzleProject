using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotVisualizer : MonoBehaviour
{
    public Image alt;
    public Image image;

    public void SetItem(Item item)
    {
    }

    public void SetImage(Sprite sprite)
    {
        if (sprite != null)
        {
            if (alt.sprite != null && alt.gameObject.activeSelf)
                alt.gameObject.SetActive(false);
            image.gameObject.SetActive(true);
            image.sprite = sprite;
        }
        else
        {
            image.sprite = null;
            image.gameObject.SetActive(false);
        }
    }
}
