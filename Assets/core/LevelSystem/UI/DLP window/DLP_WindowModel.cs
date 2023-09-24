using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

[ExecuteAlways]
public class DLP_WindowModel : MonoBehaviour
{
    public UnityEvent Changed;

    [SerializeField]
    int freePoints;
    public int FreePoints { get => this.freePoints; set { this.freePoints = value; this.Changed?.Invoke(); } }

    [SerializeField]
    int healthLevel;
    public int HealthLevel { get => this.healthLevel; set { this.healthLevel = value; this.Changed?.Invoke(); } }

    [SerializeField]
    int damageLevel;
    public int DamageLevel { get => this.damageLevel; set { this.damageLevel = value; this.Changed?.Invoke(); } }

    private void OnValidate()
    {
        this.Changed?.Invoke();
    }
}