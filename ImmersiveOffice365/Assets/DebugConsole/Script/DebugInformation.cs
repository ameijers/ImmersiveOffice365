using Office365DataHub;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInformation : Singleton<DebugInformation> 
{
    private List<string> Logs = new List<string>();
    public void Log(string text)
    {
        Logs.Insert(0, text);
    }

    public string LogText
    {
        get
        {
            string text = "";

            foreach(string line in Logs)
            {
                text += line;
                text += string.Format("\r\n");
            }

            return text;
        }
    }
}
