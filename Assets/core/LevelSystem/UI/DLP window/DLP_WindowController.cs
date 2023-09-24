using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Контроллер окна распределения очков уровней.
/// </summary>
[ExecuteAlways]
public class DLP_WindowController : MonoBehaviour
{
    public DLP_WindowView view;
    public DLP_WindowModel model;

    public PlayerStatistics playerStatistics;

    private void Start()
    {
        if (this.model != null)
            this.model.Changed.AddListener(this._UpdateView);
        this._UpdateView();
    }

    private void OnEnable()
    {
        this.CopyFrom(this.playerStatistics);
    }

    private void OnValidate()
    {
        this._UpdateView();
    }

    /// <summary>
    /// Применить выбранные параметры к данным игрока.
    /// </summary>
    public void _Apply()
    {
        this.SaveTo(this.playerStatistics);
    }

    /// <summary>
    /// Копирование данных в модель.
    /// </summary>
    /// <param name="playerStatistics"></param>
    public void CopyFrom(PlayerStatistics playerStatistics)
    {
        this.model.FreePoints = playerStatistics.FreePoints;
        this.model.HealthLevel = playerStatistics.Health.Level;
        this.model.DamageLevel = playerStatistics.Damage.Level;

        this._UpdateView();
    }

    /// <summary>
    /// Сохранение данных уровней из модели.
    /// </summary>
    /// <param name="playerStatistics"></param>
    public void SaveTo(PlayerStatistics playerStatistics)
    {
        playerStatistics.FreePoints = this.model.FreePoints;
        playerStatistics.Health.Level = this.model.HealthLevel;
        playerStatistics.Damage.Level = this.model.DamageLevel;

        this._UpdateView();
    }

    /// <summary>
    /// Обновляет представление из модели (целиком).
    /// </summary>
    public void _UpdateView()
    {
        if (this.model == null ||
            this.view == null ||
            this.playerStatistics == null)
            return;

        this.playerStatistics.Health.Level = this.model.HealthLevel;

        this.view.From(
        this.playerStatistics.Health.GetValueFromLevel(this.model.HealthLevel)
        , this.playerStatistics.Damage.GetValueFromLevel(this.model.DamageLevel)
        , 0
        , 0
        , 0
        , 0
        , 0
        , 0
        , 0
        , 0
        , this.model.FreePoints
        );
    }
}