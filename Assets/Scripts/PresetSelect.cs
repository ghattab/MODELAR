using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class PresetSelect : MonoBehaviour
{
    public GameObject preGUI, afterGUI, submitGUI;

    public GameObject preset;

    public Material blood, kidney, tumor;
    public GameObject bloodTarget, kindeyTarget, tumorTarget;

    public InputField nameF;

    public RawImage presetPreview;
    public void hideGUI()
    {

            if(nameF.text != "") {
                preGUI.SetActive(false);
                afterGUI.SetActive(true);
            } else
        {
            EnterNameFirst.time = 1.5f;
        }

    }

    public void showGUI()
    {
        preGUI.SetActive(true);
        afterGUI.SetActive(false);
        submitGUI.SetActive(false);
    }

    public void setPresets()
    {
        if (nameF.text != "")
        {
            kindeyTarget.GetComponent<Renderer>().material = (preset.GetComponentsInChildren<Renderer>()[0].material);
            bloodTarget.GetComponent<Renderer>().material = (preset.GetComponentsInChildren<Renderer>()[1].material);
            tumorTarget.GetComponent<Renderer>().material = (preset.GetComponentsInChildren<Renderer>()[2].material);
            //tumor = tumorTarget.GetComponent<Renderer>().material;
            //blood = bloodTarget.GetComponent<Renderer>().material;
            //kidney = kindeyTarget.GetComponent<Renderer>().material;
            presetPreview.texture = GetComponent<RawImage>().texture;

            printToCSV(GetComponent<RawImage>().texture.name);
        } else
        {
            EnterNameFirst.time = 1.5f;
        }

    }

    public static void printToCSV(string num)
    {
        var csv = new StringBuilder();
        var newLine = string.Format("//{0};{1}", num, Time.time);
        csv.AppendLine(newLine);
        File.AppendAllText(Application.persistentDataPath + "/History" + UserInfo.nameF.text + ".csv", csv.ToString());
        Debug.Log(Application.persistentDataPath + "/History");
    }

    

    public void Update()
    {
        if(nameF != null && nameF.text == "")
        {
            ColorBlock colors = GetComponent<Button>().colors;
            colors.normalColor = new Color(0.5f,0.5f,0.5f,0.5f);
            colors.selectedColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            GetComponent<Button>().colors = colors;
        }
        else
        {
            ColorBlock colors = GetComponent<Button>().colors;
            colors.normalColor = Color.white;
            colors.selectedColor = Color.white;
            GetComponent<Button>().colors = colors;
        }
    }
}
