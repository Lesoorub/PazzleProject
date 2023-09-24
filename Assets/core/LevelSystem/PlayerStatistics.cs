using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = nameof(PlayerStatistics), menuName = Paths.PlayerDir + nameof(PlayerStatistics))]
public class PlayerStatistics : ScriptableObject
{
    public int FreePoints;

    public Statistic Health;
    public Statistic Damage;
    public Statistic MinAir;
    public Statistic MinFire;
    public Statistic MinPlant;
    public Statistic MinWater;
    public Statistic MaxAir;
    public Statistic MaxFire;
    public Statistic MaxPlant;
    public Statistic MaxWater;
}

[Serializable]
public class Statistic
{
    public int Level;

    public float Offset;
    public AnimationCurve Multiplayer;

    public float Value => 
        this.GetValueFromLevel(this.Level);

    public float GetValueFromLevel(int level) => 
        this.Offset +
        this.Multiplayer.Evaluate(this.Level);
}