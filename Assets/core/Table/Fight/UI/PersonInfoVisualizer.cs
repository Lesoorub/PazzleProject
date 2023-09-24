using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PersonInfoVisualizer : MonoBehaviour
{
    public TMP_Text NameText;

    public Image PersonImage;
    public Outline CurrentStepOutline;

    [Header("Health")]
    public TMP_Text HealthText;
    public string HealthTextFormat = "{0} / {1}";
    public Slider HealthSlider;

    [Header("Mana")]
    public ManaVisualizer Air;
    public ManaVisualizer Fire;
    public ManaVisualizer Plant;
    public ManaVisualizer Water;

    [Header("Spells")]
    public GameObject SpellPrefab;
    public Transform SpellsParent;
    private List<SpellVisualizer> spells = new List<SpellVisualizer>();
    public SpellVisualizer[] Spells => spells.ToArray();

    public void Set(FightPerson firhgtperson, FightController table)
    {
        NameText.text = firhgtperson.person.Name;
        PersonImage.sprite = firhgtperson.person.Image;
        PersonImage.color = firhgtperson.person.Color;

        foreach (var spell in firhgtperson.person.BaseSpells)
        {
            var obj = Instantiate(SpellPrefab, SpellsParent);
            var sv = obj.GetComponent<SpellVisualizer>();
            sv.spell = spell;
        }

        spells = SpellsParent.GetComponentsInChildren<SpellVisualizer>().ToList();

        foreach (var spell in spells)
        {
            spell.Init(firhgtperson, table);
        }
        UpdateStats(firhgtperson);
        UpdateSpellsUI();

        firhgtperson.OnStatsChanged?.AddListener(() => UpdateStats(firhgtperson));
    }

    public void UpdateStats(FightPerson person)
    {
        HealthText.text = FixTools.StringFormat(HealthTextFormat, Mathf.CeilToInt(person.CurHP), Mathf.CeilToInt(person.MaxHP));
        HealthSlider.value = person.CurHP / person.MaxHP;

        Air.Set(person.Air, person.MaxAir);
        Fire.Set(person.Fire, person.MaxFire);
        Plant.Set(person.Plant, person.MaxPlant);
        Water.Set(person.Water, person.MaxWater);

        foreach (var spell in spells)
        {
            spell.UpdateUI();
        }
    }

    public void SpellsCooldownTick()
    {
        foreach (var spell in spells)
        {
            spell.CooldownTick();
        }
    }
    public void UpdateSpellsUI()
    {
        foreach (var spell in spells)
        {
            spell.UpdateCooldownUI();
        }
    }
    public void SetCurrentStep(bool isMe)
    {
        CurrentStepOutline.enabled = isMe;
    }
}
