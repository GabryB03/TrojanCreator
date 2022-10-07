using System;

public class InstructionDetails
{
    private System.Collections.Generic.List<Tuple<string, string>> theDetails;

    public InstructionDetails(string details)
    {
        theDetails = new System.Collections.Generic.List<Tuple<string, string>>();
        string currentName = "", currentValue = "";
        bool opened = false, evaluating = false;

        for (int i = 0; i < details.Length; i++)
        {
            if (details[i] == '[')
            {
                opened = true;
            }
            else if (details[i] == ']')
            {
                opened = false;
            }
            else if (details[i] == '\"')
            {
                evaluating = !evaluating;

                if (!evaluating)
                {
                    theDetails.Add(new Tuple<string, string>(currentName, InvertEscapes(currentValue)));
                    currentName = "";
                    currentValue = "";
                }
            }
            else
            {
                if (opened)
                {
                    currentName += details[i];
                }
                else if (evaluating)
                {
                    bool foundEscape = false, foundEscapeTwo = false;

                    try
                    {
                        if (details[i] == '\\')
                        {
                            if (details[i + 1] == 'n')
                            {
                                currentValue += '\n';
                                foundEscape = true;
                            }
                            else if (details[i + 1] == 'r')
                            {
                                currentValue += '\r';
                                foundEscape = true;
                            }
                            else if (details[i + 1] == 't')
                            {
                                currentValue += '\t';
                                foundEscape = true;
                            }
                            else if (details[i + 1] == '"')
                            {
                                currentValue += '\"';
                                foundEscapeTwo = true;
                            }
                        }
                    }
                    catch
                    {

                    }

                    if (foundEscape)
                    {
                        i += 3;
                    }
                    else if (foundEscapeTwo)
                    {
                        i++;
                    }
                    else
                    {
                        currentValue += details[i];
                    }
                }
            }
        }

        if (currentName != "" && currentValue != "")
        {
            theDetails.Add(new Tuple<string, string>(currentName, currentValue));
        }
    }

    private string InvertEscapes(string str)
    {
        return str.Replace("\\" + "r", '\r'.ToString()).Replace("\\" + "n", '\n'.ToString()).Replace("\\" + "t", '\t'.ToString()).Replace("\\" + "\"", '\"'.ToString());
    }

    public string GetProperty(string str)
    {
        foreach (Tuple<string, string> theTuple in theDetails)
        {
            if (theTuple.Item1.Equals(str))
            {
                return theTuple.Item2;
            }
        }

        return null;
    }
}