using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class SpellVisualizer : MonoBehaviour
{
    public Spell spell;

    public int CurrentCooldown = 0;

    public FightPerson f_person;
    FightController table;

    public Image image;
    public Image canExecute;
    public Color32 NotCanExecuteColor = new Color32(255,255,255,128);
    public Color32 CanExecuteColor = new Color32(255, 255, 255, 255);
    public TMPro.TMP_Text CooldownText;
    public TMPro.TMP_Text air;
    public TMPro.TMP_Text fire;
    public TMPro.TMP_Text plant;
    public TMPro.TMP_Text water;

    public bool hasCooldown() => CurrentCooldown > 0;
    public bool hasMana() => f_person.Air >= spell.Air &&
            f_person.Fire >= spell.Fire &&
            f_person.Plant >= spell.Plant &&
            f_person.Water >= spell.Water;

    public void Init(FightPerson f_person, FightController table)
    {
        this.f_person = f_person;
        this.table = table;
        CurrentCooldown = spell.Cooldown;
        OnValidate();
    }

    public void UpdateUI()
    {
        byte f(byte a, byte b) => (byte)((a / 255f) * (b / 255f) * 255);
        Color32 M(Color32 a, Color32 b)
        {
            return new Color32(
                f(a.r, b.r),
                f(a.g, b.g),
                f(a.b, b.b),
                f(a.a, b.a));
        }

        if (f_person == null)
        {
            canExecute.color = NotCanExecuteColor;
            return;
        }
        if (spell == null) return;
        canExecute.color = hasMana() ? M(spell.Color, CanExecuteColor) : M(spell.Color, NotCanExecuteColor);
    }

    private void OnValidate()
    {
        if (spell == null) return;
        image.sprite = spell.Image;
        UpdateUI();

        air.text = spell.Air.ToString();
        fire.text = spell.Fire.ToString();
        plant.text = spell.Plant.ToString();
        water.text = spell.Water.ToString();
    }

    public void _OnClick()
    {
        if (f_person != table.Me) return;
        if (!hasMana()) return;
        if (hasCooldown()) return;

        if (spell != null)
        {
            CurrentCooldown = spell.Cooldown;
            spell.Execute(table, f_person, table.Enemy, this);
            UpdateCooldownUI();
        }

        f_person.AddMana(ManaType.air, -spell.Air);
        f_person.AddMana(ManaType.fire, -spell.Fire);
        f_person.AddMana(ManaType.plant, -spell.Plant);
        f_person.AddMana(ManaType.water, -spell.Water);
    }

    public void CooldownTick()
    {
        if (CurrentCooldown > 0)
            CurrentCooldown--;
        UpdateCooldownUI();
    }
    public void UpdateCooldownUI()
    {
        CooldownText.gameObject.SetActive(hasCooldown());
        CooldownText.text = CurrentCooldown.ToString();
    }
}
