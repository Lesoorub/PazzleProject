using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

/// <summary>
/// Представление окна распределения очков уровней.
/// </summary>
public class DLP_WindowView : MonoBehaviour
{
    [Header("Set in inspector")]
    [SerializeField]
    TMP_Text AvailablePoints;

    [SerializeField]
    DLP_ElementVisualizer Health;

    [SerializeField]
    DLP_ElementVisualizer Damage;

    [SerializeField]
    DLP_ElementVisualizer MaxAir;

    [SerializeField]
    DLP_ElementVisualizer MaxFire;

    [SerializeField]
    DLP_ElementVisualizer MaxPlant;

    [SerializeField]
    DLP_ElementVisualizer MaxWater;

    [SerializeField]
    DLP_ElementVisualizer MinAir;

    [SerializeField]
    DLP_ElementVisualizer MinFire;

    [SerializeField]
    DLP_ElementVisualizer MinPlant;

    [SerializeField]
    DLP_ElementVisualizer MinWater;

    public string _availablePointsFormat = "{0}";

    private void Start()
    {
        this._availablePointsFormat = this.AvailablePoints.text;
    }

    /// <summary>
    /// Загрузка из переданных данных.
    /// </summary>
    /// <param name="health"></param>
    /// <param name="damage"></param>
    /// <param name="minAir"></param>
    /// <param name="maxAir"></param>
    /// <param name="minFire"></param>
    /// <param name="maxFire"></param>
    /// <param name="minPlant"></param>
    /// <param name="maxPlant"></param>
    /// <param name="minWater"></param>
    /// <param name="maxWater"></param>
    /// <param name="freePoints"></param>
    public void From( // Пока что так, позже нужно подумать как правильно это сделать.
        float health
        , float damage
        , float minAir
        , float maxAir
        , float minFire
        , float maxFire
        , float minPlant
        , float maxPlant
        , float minWater
        , float maxWater
        , float freePoints
        )
    {
        this.AvailablePoints.text = string.Format(this._availablePointsFormat, freePoints);
        this.Health.SetCounter(health);
        this.Damage.SetCounter(damage);
        this.MinAir.SetCounter(minAir);
        this.MaxAir.SetCounter(maxAir);
        this.MinFire.SetCounter(minFire);
        this.MaxFire.SetCounter(maxFire);
        this.MinPlant.SetCounter(minPlant);
        this.MaxPlant.SetCounter(maxPlant);
        this.MinWater.SetCounter(minWater);
        this.MaxWater.SetCounter(maxWater);
    }
}