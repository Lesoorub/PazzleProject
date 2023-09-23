public static class FixTools
{
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