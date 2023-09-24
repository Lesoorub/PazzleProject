using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Визуализатор элемента окна DLP (Distribute level points).
/// </summary>
[ExecuteAlways]
public class DLP_ElementVisualizer : MonoBehaviour
{
    [SerializeField]
    TMP_Text DescriptionText;

    [SerializeField]
    TMP_Text CounterText;

    [SerializeField]
    Button AddBtn;

    [SerializeField]
    Button SubBtn;

    //public UnityEvent CounterAdded = new UnityEvent();
    //public UnityEvent CounterSubed = new UnityEvent();

    /// <summary>
    /// Текст описания элемента.
    /// </summary>
    public string Description;

    /// <summary>
    /// Значение счетчика.
    /// </summary>
    public float Counter;

    public string CounterToStrFormat = "N0";

    //private Statistic m_statistic;

    //public void Show(Statistic statistic)
    //{
    //    this.m_statistic = statistic;

    //    Update();

    //    if (statistic != null)
    //        statistic.Changed.RemoveListener(Update);
    //    statistic.Changed.AddListener(Update);

    //    return;
    //    void Update()
    //    {
    //        this.Counter = statistic.Value;
    //        this.UpdateLabels();
    //    }
    //}


    private void OnValidate()
    {
        this.UpdateLabels();
    }

    /// <summary>
    /// Обновление интерфейса.
    /// </summary>
    private void UpdateLabels()
    {
        if (this.DescriptionText == null ||
            this.CounterText == null)
            return;

        this.DescriptionText.text = this.Description;
        this.CounterText.text = this.Counter.ToString(this.CounterToStrFormat);
    }

    public void SetCounter(float value)
    {
        this.Counter = value;
        this.CounterText.text = this.Counter.ToString(this.CounterToStrFormat);
    }
}
