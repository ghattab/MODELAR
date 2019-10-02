using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class ChangeToSubmitScree : MonoBehaviour
{
    public GameObject guipre, guiafter;
    
    public void change()
    {
        guipre.SetActive(false);
        guiafter.SetActive(true);
        printEndToCSV();
    }

    public static void printEndToCSV()
    {
        var csv = new StringBuilder();
        var newLine = string.Format("\\{0};{1}", "Submit", Time.time);
        csv.AppendLine(newLine);
        File.AppendAllText(Application.persistentDataPath + "/History" + UserInfo.nameF.text + ".csv", csv.ToString());
        Debug.Log(Application.persistentDataPath + "/History");
    }
}
