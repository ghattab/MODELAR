using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PresetSelect : MonoBehaviour
{
    public GameObject preGUI, afterGUI;

    public GameObject preset;

    public Material blood, kidney, tumor;
    public GameObject bloodTarget, kindeyTarget, tumorTarget;

    public RawImage presetPreview;
    public void hideGUI()
    {
        preGUI.SetActive(false);
        afterGUI.SetActive(true);
        
    }

    public void showGUI()
    {
        preGUI.SetActive(true);
        afterGUI.SetActive(false);
    }

    public void setPresets()
    {
        kindeyTarget.GetComponent<Renderer>().material = (preset.GetComponentsInChildren<Renderer>()[0].material);
        bloodTarget.GetComponent<Renderer>().material = (preset.GetComponentsInChildren<Renderer>()[1].material);
        tumorTarget.GetComponent<Renderer>().material = (preset.GetComponentsInChildren<Renderer>()[2].material);
        //tumor = tumorTarget.GetComponent<Renderer>().material;
        //blood = bloodTarget.GetComponent<Renderer>().material;
        //kidney = kindeyTarget.GetComponent<Renderer>().material;
        presetPreview.texture = GetComponent<RawImage>().texture;
    }
}
