using UnityEngine;

public static class PlayerPrefsHelper
{
    public static bool HasVector2(string path) =>
        PlayerPrefs.HasKey(System.IO.Path.Combine(path, "x")) &&
        PlayerPrefs.HasKey(System.IO.Path.Combine(path, "y"));
    public static void SetVector2(string path, Vector2 value)
    {
        PlayerPrefs.SetFloat(System.IO.Path.Combine(path, "x"), value.x);
        PlayerPrefs.SetFloat(System.IO.Path.Combine(path, "y"), value.y);
    }
    public static Vector2 GetVector2(string path)
    {
        return new Vector2(
            PlayerPrefs.GetFloat(System.IO.Path.Combine(path, "x")),
            PlayerPrefs.GetFloat(System.IO.Path.Combine(path, "y")));
    }
}