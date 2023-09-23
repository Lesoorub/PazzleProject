using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPResourceVisualizer : GameResourceVisualizer
{
    public TMPro.TMP_Text currentLevel;
    public override void SetText()
    {
        ExpFormula(variable.Data / 10f, out var lvl, out var current, out var needxp);
        if (text != null)
            text.text = variable.Data.ToString();
        if (slider != null)
            slider.value = current / needxp;
        if (currentLevel != null)
            currentLevel.text = lvl.ToString();
    }

    public void ExpFormula(float total, out int lvl, out float current, out int needxp)
    {
        lvl = level(total);
        current = total - totalExp(lvl);
        needxp = reqExp(lvl);
    }
    float totalExp(int level)
    {
        if (level >= 0 && level <= 16)
            return level * level + 6f * level;
        if (level <= 31)
            return 2.5f * level * level - 40.5f * level + 360;
        return 4.5f * level * level - 162.5f * level + 2220;
    }

    int level(float total)
    {
        if (total >= 0 && total <= 352)
            return (int)(Mathf.Sqrt(total + 9) - 3);
        if (total <= 1507)
            return (int)(8.1f + Mathf.Sqrt(0.4f * (total - 7839f / 40)));
        return (int)(328f / 18f + Mathf.Sqrt(2f / 9f * (total - 54215f / 72)));
    }
    int reqExp(int level)
    {
        if (level >= 0 && level <= 15)
            return 2 * level + 7;
        if (level <= 30)
            return 5 * level - 38;
        return 9 * level - 158;
    }
}
