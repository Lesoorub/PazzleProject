/// <summary>
/// Предоставляет методы устраняющие неполнятные баги билда под Android
/// </summary>
public static class FixTools
{
    /// <summary>
    /// Кастомный, кривой <see cref="string.Format(string, object[])"/>.
    /// </summary>
    /// <param name="formatString"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    public static string StringFormat(string formatString, params object[] objs)
    {
        string str = (string)formatString.Clone();
        for (int k = 0; k < objs.Length; k++)
        {
            if (objs[k] == null)
                objs[k] = "null";
            str = str.Replace("{" + k + "}", objs[k].ToString());
        }
        return str;
    }
}