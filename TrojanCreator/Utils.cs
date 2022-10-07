using System.Collections.Generic;

public class Utils
{
    public static List<TrojanInstruction> instructions = new List<TrojanInstruction>();

    public static string AdjustEscapes(string str)
    {
        return str.Replace('\r'.ToString(), "\\" + "r").Replace('\n'.ToString(), "\\" + "n").Replace('\t'.ToString(), "\\" + "t").Replace('\"'.ToString(), "\\" + "\"");
    }
}