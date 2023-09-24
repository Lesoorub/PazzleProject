using UnityEngine.Events;
using UnityEngine;
public class FightPerson
{
    public Person person;

    public FightPerson(Person person)
    {
        this.person = person;
        CurHP = MaxHP = person.BaseHealth;
    }

    public bool isDeadth => CurHP <= 0;
    public float CurHP { get; private set; }
    public float MaxHP { get; private set; }

    public int Air   { get; private set; }
    public int Fire  { get; private set; }
    public int Plant { get; private set; }
    public int Water { get; private set; }

    public int MaxAir   { get; private set; } = 30;
    public int MaxFire  { get; private set; } = 30;
    public int MaxPlant { get; private set; } = 30;
    public int MaxWater { get; private set; } = 30;

    public void ApplyDamage(float Damage)
    {
        CurHP -= Damage;
        if (CurHP > MaxHP)
            CurHP = MaxHP;
        else if (CurHP <= 0)
        {
            CurHP = 0;
            OnDeath?.Invoke();
        }
        OnStatsChanged?.Invoke();
    }
    public void AddMana(ManaType type, int count)
    {
        switch (type)
        {
            case ManaType.air:
                Air = Mathf.Min(Air + count, MaxAir);
                break;
            case ManaType.fire:
                Fire = Mathf.Min(Fire + count, MaxFire);
                break;
            case ManaType.plant:
                Plant = Mathf.Min(Plant + count, MaxPlant);
                break;
            case ManaType.water:
                Water = Mathf.Min(Water + count, MaxWater);
                break;
        }
        OnStatsChanged?.Invoke();
    }

    public UnityEvent OnStatsChanged = new UnityEvent();
    public UnityEvent OnDeath = new UnityEvent();
}