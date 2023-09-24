//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;

//public class DLP_WIndowVisualizer : MonoBehaviour
//{
//    public PlayerStatistics PlayerLevelStats { get; set; }

//    [SerializeField]
//    TMP_Text AvailablePoints;

//    [SerializeField]
//    DLP_ElementVisualizer Health;

//    [SerializeField]
//    DLP_ElementVisualizer Damage;

//    [SerializeField]
//    DLP_ElementVisualizer MaxAir;

//    [SerializeField]
//    DLP_ElementVisualizer MaxFire;

//    [SerializeField]
//    DLP_ElementVisualizer MaxPlant;

//    [SerializeField]
//    DLP_ElementVisualizer MaxWater;

//    [SerializeField]
//    DLP_ElementVisualizer MinAir;

//    [SerializeField]
//    DLP_ElementVisualizer MinFire;

//    [SerializeField]
//    DLP_ElementVisualizer MinPlant;

//    [SerializeField]
//    DLP_ElementVisualizer MinWater;

//    string availbalePointsFormat;

//    private void Start()
//    {
//        this.availbalePointsFormat = this.AvailablePoints.text;

//        Link(this.PlayerLevelStats.Health,   this.Health);
//        Link(this.PlayerLevelStats.Damage,   this.Damage);
//        Link(this.PlayerLevelStats.MinAir,   this.MinAir);
//        Link(this.PlayerLevelStats.MinFire,  this.MinFire);
//        Link(this.PlayerLevelStats.MinPlant, this.MinPlant);
//        Link(this.PlayerLevelStats.MinWater, this.MinWater);
//        Link(this.PlayerLevelStats.MaxAir,   this.MaxAir);
//        Link(this.PlayerLevelStats.MaxFire,  this.MaxFire);
//        Link(this.PlayerLevelStats.MaxPlant, this.MaxPlant);
//        Link(this.PlayerLevelStats.MaxWater, this.MaxWater);

//        this.UpdateLabels();
//    }

//    private void UpdateLabels()
//    {
//        this.AvailablePoints.text = string.Format(
//            this.availbalePointsFormat, 
//            this.PlayerLevelStats.FreePoints);
//    }

//    private static void Link(
//        Statistic statistic,
//        DLP_ElementVisualizer visualizer)
//    {
//        if (statistic == null || visualizer == null)
//            return;
//        visualizer.Show(statistic);
//    }
//}
