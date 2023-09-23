using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SkillCaster : MonoBehaviour
{
    public TableController table;
    public Button TNT;
    public Button BLOB;
    public Button Richochet;
    public Button Magnet;
    public Button LaserX;

    public UserIntVariable rupee;

    public Transform EffectParent;
    public GameObject EffectPrefab;

    bool preventNextSelect = false;
    List<UnityAction<Vector2Int>> actions = new List<UnityAction<Vector2Int>>();
    private void Awake()
    {
        table.OnStartSelect?.AddListener(Preventor);
    }


    void Preventor(Vector2Int pos)
    {
        if (preventNextSelect)
        {
            table.TableEndSelection();
            preventNextSelect = false;
            foreach (var a in actions)
                a?.Invoke(pos);
            actions.Clear();
        }
    }

    public void _TNT(int cost)
    {
        if (rupee.Data < cost)
            return;
        rupee.Data -= cost;
        TNT.interactable = false;
        preventNextSelect = true;
        actions.Add(new UnityAction<Vector2Int>(pos =>
        {
            //effect
            var obj = Instantiate(EffectPrefab, table.GetPosition(pos), Quaternion.identity, EffectParent);
            var rect = obj.GetComponent<RectTransform>();
            rect.sizeDelta = Vector2.one * (table.size * 5 + table.space * 3) / transform.lossyScale.z;
            var outline = obj.GetComponent<UIOutline>();
            outline.color = new Color(0x87, 0, 0);
            Destroy(obj, 1f);
            //do
            table.RemoveInRadius(pos.x, pos.y, 2);
            TNT.interactable = true;
            table.TableEndSelection();
        }));
    }
    public void _BLOB(int cost)
    {
        if (rupee.Data < cost)
            return;
        rupee.Data -= cost;
        BLOB.interactable = false;
        preventNextSelect = true;
        actions.Add(new UnityAction<Vector2Int>(pos =>
        {
            //effect
            var obj = Instantiate(EffectPrefab, table.GetPosition(pos), Quaternion.identity, EffectParent);
            var rect = obj.GetComponent<RectTransform>();
            rect.sizeDelta = Vector2.one * (table.size * 3 + table.space * 2) / transform.lossyScale.z;
            var outline = obj.GetComponent<UIOutline>();
            outline.color = new Color(0xFF, 0x4D, 0);
            Destroy(obj, 1f);
            //Do
            table.RemoveInRadius(pos.x, pos.y, 1);
            BLOB.interactable = true;
            table.TableEndSelection();
        }));
    }
    public void _Richochet(int cost)
    {
        if (rupee.Data < cost)
            return;
        rupee.Data -= cost;
        Richochet.interactable = false;
        preventNextSelect = true;
        actions.Add(new UnityAction<Vector2Int>(pos =>
        {
            TableElement e = table.Get(pos.x, pos.y);
            for (int y = 0; y < TableController.TableSizeHeight; y++)
                for (int x = 0; x < TableController.TableSizeWidth; x++)
                {
                    if (table.Get(x, y) == e)
                    {
                        //effect
                        var obj = Instantiate(EffectPrefab, table.GetPosition(x, y), Quaternion.identity, EffectParent);
                        var rect = obj.GetComponent<RectTransform>();
                        rect.sizeDelta = Vector2.one * (table.size / transform.lossyScale.z);
                        var outline = obj.GetComponent<UIOutline>();
                        outline.color = new Color(0xFF, 0xE7, 0);
                        Destroy(obj, 1f);
                        //Do
                        table.RemoveAt(x, y);
                    }
                }
            Richochet.interactable = true;
            table.TableEndSelection();
        }));
    }
    public void _Magnet(int cost)
    {
        if (rupee.Data < cost)
            return;
        rupee.Data -= cost;
        Magnet.interactable = false;
        preventNextSelect = true;
        actions.Add(new UnityAction<Vector2Int>(pos =>
        {
            TableElement selected = table.Get(pos.x, pos.y);

            List<Vector2Int> Find(TableElement e)
            {
                List<Vector2Int> r = new List<Vector2Int>();
                for (int y = 0; y < TableController.TableSizeHeight; y++)
                    for (int x = 0; x < TableController.TableSizeWidth; x++)
                    {
                        if (table.Get(x, y) == e) 
                            r.Add(new Vector2Int(x, y));
                    }
                return r;
            }

            void F(Vector2Int p)
            {
                Vector2Int near = p;
                float min = (p - pos).magnitude;
                for (int y = 0; y < TableController.TableSizeHeight; y++)
                    for (int x = 0; x < TableController.TableSizeWidth; x++)
                    {
                        var el = table.Get(x, y);
                        if (el != selected)
                        {
                            var p1 = new Vector2Int(x, y);
                            var d = (p1 - pos).magnitude;
                            if (d < min)
                            {
                                near = p1;
                                min = d;
                            }
                        }
                    }
                table.Swap(p, near);
            }

            var allfinded = Find(selected);
            foreach (var p in allfinded)
            {
                //effect
                var obj = Instantiate(EffectPrefab, table.GetPosition(p), Quaternion.identity, EffectParent);
                var rect = obj.GetComponent<RectTransform>();
                rect.sizeDelta = Vector2.one * (table.size / transform.lossyScale.z);
                var outline = obj.GetComponent<UIOutline>();
                outline.color = new Color(0x00, 0xFD, 0x75);
                Destroy(obj, 1f);
                //Do
                F(p);
            }
            Magnet.interactable = true;
            table.TableEndSelection();
        }));
    }
    public void _LaserX(int cost)
    {
        if (rupee.Data < cost)
            return;
        rupee.Data -= cost;
        LaserX.interactable = false;
        preventNextSelect = true;
        actions.Add(new UnityAction<Vector2Int>(pos =>
        {
            for (int k = 0; k < TableController.TableSizeWidth; k++)
                table.RemoveAt(k, pos.y);
            for (int k = 0; k < TableController.TableSizeHeight; k++)
                table.RemoveAt(pos.x, k);
            LaserX.interactable = true;
            table.TableEndSelection();
        }));
    }
}
