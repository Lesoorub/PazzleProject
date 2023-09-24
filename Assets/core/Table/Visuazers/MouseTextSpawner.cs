using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseTextSpawner : MonoBehaviour
{
    public GameObject prefab;
    public Transform parent;
    public string format = "+{0}";
    public float Lifetime = 3;
    public Vector2 velocity = Vector2.down * 10;
    public AnimationCurve velocityMult;
    public void Spawn(int count)
    {
        var obj = Instantiate(prefab, Input.mousePosition, Quaternion.identity, parent);
        Destroy(obj, Lifetime);
        var tmp = obj.GetComponentInChildren<TMPro.TMP_Text>();
        tmp.StartCoroutine(Movement(obj));
        tmp.text = FixTools.StringFormat(format, count);
    }

    public IEnumerator Movement(GameObject obj)
    {
        float startTime = Time.time;

        TMPro.TMP_Text text = obj.GetComponentInChildren<TMPro.TMP_Text>();
        text.CrossFadeAlpha(0, Lifetime, true);

        Image img = obj.GetComponentInChildren<Image>();
        img.CrossFadeAlpha(0, Lifetime, true);

        while (obj != null)
        {
            var v = (Time.time - startTime) / Lifetime;
            obj.transform.position += ((Vector3)velocity * Time.deltaTime) * velocityMult.Evaluate(v);
            
            yield return new WaitForEndOfFrame();
        }
    }
}
