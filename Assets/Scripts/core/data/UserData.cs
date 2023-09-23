using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "userdata", menuName = "core/User/data", order = 2)]
public class UserData : ScriptableObject
{
    public Person person;

    public UserIntVariable wood;
    public UserIntVariable stone;
    public UserIntVariable cloth;
    public UserIntVariable crystals;
    public UserIntVariable rupee;
    public UserIntVariable totalExp;
    public void Save()
    {
        Debug.Log("Save");
        PlayerPrefs.SetInt("wood", wood.Data);
        PlayerPrefs.SetInt("stone", stone.Data);
        PlayerPrefs.SetInt("cloth", cloth.Data);
        PlayerPrefs.SetInt("crystals", crystals.Data);
        PlayerPrefs.SetInt("rupee", rupee.Data);
        PlayerPrefs.SetInt("totalExp", totalExp.Data);
    }
    public void Load()
    {
        wood.Data = PlayerPrefs.GetInt("wood");
        stone.Data = PlayerPrefs.GetInt("stone");
        cloth.Data = PlayerPrefs.GetInt("cloth");
        crystals.Data = PlayerPrefs.GetInt("crystals");
        rupee.Data = PlayerPrefs.GetInt("rupee");
        totalExp.Data = PlayerPrefs.GetInt("totalExp");
    }
}
