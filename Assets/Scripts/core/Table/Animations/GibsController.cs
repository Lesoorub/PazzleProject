using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GibsController : MonoBehaviour
{
    public GameObject GibPrefab;
    public Transform GibParent;
    public static GibsController Instance { get; private set; }
    public void Awake()
    {
        Instance = this;
    }
    private void OnDestroy()
    {
        Instance = null;
    }
    public static void Create(
        Sprite image,
        Color32 color,
        Vector2 from_position,
        Vector2 to_position,
        UnityAction finalAction)
    {
        Create(image, color, from_position, to_position, finalAction, Vector2.zero);
    }
    public static void Create(
        Sprite image,
        Color32 color,
        Vector2 from_position,
        Vector2 to_position,
        UnityAction finalAction,
        Vector2 start_velocity)
    {
        var obj = Instantiate(
                        Instance.GibPrefab,
                        from_position,
                        Quaternion.identity,
                        Instance.GibParent);

        var img = obj.GetComponentInChildren<Image>();
        img.sprite = image;
        img.color = color;

        Instance.StartCoroutine(
            GibLogic(
                new Gib(
                    obj, 
                    from_position, 
                    to_position, 
                    finalAction,
                    start_velocity,
                    Instance.GibParent.lossyScale.z)
                )
            );
    }

    static IEnumerator GibLogic(Gib g)
    {
        while (!g.isFinish())
        {
            g.UpdatePosition();
            yield return new WaitForEndOfFrame();
        }
        g.Destroy();
        yield break;
    }

    private class Gib
    {
        Vector2 to_position;
        UnityAction finalAction;
        GameObject instance;
        float screenFactor;

        public Vector2 position;
        public Vector2 velocity = Vector2.zero;
        public Gib(
            GameObject instance, 
            Vector2 from_position, 
            Vector2 to_position, 
            UnityAction finalAction,
            Vector2 start_velocity,
            float screenFactor)
        {
            this.instance = instance;
            this.position = from_position;
            this.to_position = to_position;
            this.finalAction = finalAction;
            this.velocity = start_velocity;
            this.screenFactor = screenFactor;
        }

        public bool isFinish()
        {
            float threashold = 28 * screenFactor;
            return Vector2.Distance(position, to_position) < threashold;
        }

        public void Destroy()
        {
            finalAction?.Invoke();
            const float final_lifetime = .5f;
            instance.transform.position = to_position;
            var img = instance.GetComponentInChildren<Image>();
            img.CrossFadeAlpha(0, final_lifetime, true);
            img.StartCoroutine(CrossScale(Vector2.zero, final_lifetime));
            Object.Destroy(instance, final_lifetime);
        }

        public void UpdatePosition()
        {
            float Acselerate = 64 * screenFactor;

            velocity = Vector2.Lerp(
                velocity.normalized, 
                (to_position - position).normalized, 
                Time.deltaTime * 10) 
                * velocity.magnitude;
            velocity += (to_position - position).normalized * Acselerate * Time.deltaTime;
            position += velocity;
            instance.transform.position = position;
        }

        public IEnumerator CrossScale(Vector2 final, float duration)
        {
            Vector2 start = instance.transform.localScale;
            float startTime = Time.time;
            while (Time.time < startTime + duration)
            {
                float value = (Time.time - startTime) / duration;
                instance.transform.localScale = Vector2.Lerp(start, final, value);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
