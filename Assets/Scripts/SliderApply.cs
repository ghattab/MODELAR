using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SliderApply : MonoBehaviour
{
    public Dropdown effect;
    public Slider depthWeightSlider;
    public Slider transparancySlider;
    public Slider transparancyFalloffSlider;
    public Slider glossinessSlider;
    // Start is called before the first frame update
    void Start()
    {

        depthWeightSlider.onValueChanged.AddListener(delegate {
            setDepthWeight(depthWeightSlider);
        });

        EventTrigger trigger = depthWeightSlider.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((data) => { UpSetDepthWeight(depthWeightSlider); });
        trigger.triggers.Add(entry);

        transparancySlider.onValueChanged.AddListener(delegate {
            setTranparancy(transparancySlider);
        });

        trigger = transparancySlider.GetComponent<EventTrigger>();
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((data) => { UpSetTrancparancy(transparancySlider); });
        trigger.triggers.Add(entry);

        transparancyFalloffSlider.onValueChanged.AddListener(delegate {
            setTranparancyFalloff(transparancyFalloffSlider);
        });

        trigger = transparancyFalloffSlider.GetComponent<EventTrigger>();
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((data) => { UpSetTranparancyFalloff(transparancyFalloffSlider); });
        trigger.triggers.Add(entry);

        glossinessSlider.onValueChanged.AddListener(delegate {
            setSpecular(glossinessSlider);
        });

        trigger = glossinessSlider.GetComponent<EventTrigger>();
        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((data) => { UpSetSpecular(glossinessSlider); });
        trigger.triggers.Add(entry);

        effect.onValueChanged.AddListener(delegate
        {
            UpdateChilds();
        });

        UpdateChilds();

        UpdateSliders();

    }

    public void LateUpdate()
    {
        UpdateSliders();
    }

    // Update is called once per frame
    void UpdateChilds()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            if (i == effect.value)
                transform.GetChild(i).gameObject.SetActive(true);
            else
                transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void UpdateSliders()
    {
        MeshRenderer mr = DropdownDataProvider.currentSelected.GetComponent<MeshRenderer>();
        transparancySlider.value = mr.material.GetFloat("_Alpha") * 10f;
        transparancyFalloffSlider.value = mr.material.GetFloat("_AlphaFalloff") * 5f;
        glossinessSlider.value = mr.material.GetFloat("_Glossiness") * (1f/0.4f);
        depthWeightSlider.value = mr.material.GetFloat("_Depth_Weight") * 5f;
    }
    void setDepthWeight(Slider slider)
    {
        DropdownDataProvider.currentSelected.GetComponent<MeshRenderer>().material.SetFloat("_Depth_Weight", slider.value * 0.2f);
    }

    
    void setTranparancy(Slider slider)
    {
        DropdownDataProvider.currentSelected.GetComponent<MeshRenderer>().material.SetFloat("_Alpha", slider.value * 0.1f);
    }

    void setTranparancyFalloff(Slider slider)
    {
        DropdownDataProvider.currentSelected.GetComponent<MeshRenderer>().material.SetFloat("_AlphaFalloff", slider.value * 0.2f);
    }

    void setSpecular(Slider slider)
    {
        DropdownDataProvider.currentSelected.GetComponent<MeshRenderer>().material.SetFloat("_Glossiness", slider.value * 0.4f);
    }

    public static void printToCSV(string obj, string met, string val)
    {
        var csv = new StringBuilder();
        var newLine = string.Format("{0};{1};{2};{3}", obj, met, val, Time.time);
        csv.AppendLine(newLine);
        File.AppendAllText(Application.persistentDataPath + "/History" + UserInfo.nameF.text + ".csv", csv.ToString());
        Debug.Log(Application.persistentDataPath + "/History");
    }
    public void UpSetDepthWeight(Slider slider)
    {
        setDepthWeight(slider);
        RedoUndo.addToHistory(DropdownDataProvider.currentSelected, "_Depth_Weight", slider.value * 0.2f);
        printToCSV(DropdownDataProvider.currentSelected.name.ToString(), "Depth", slider.value.ToString());

    }
    public void UpSetTrancparancy(Slider slider)
    {
        setTranparancy(slider);
        RedoUndo.addToHistory(DropdownDataProvider.currentSelected, "_Alpha", slider.value * 0.1f);
        printToCSV(DropdownDataProvider.currentSelected.name.ToString(), "Opacity", slider.value.ToString());

    }

    public void UpSetTranparancyFalloff(Slider slider)
    {
        setTranparancyFalloff(slider);
        RedoUndo.addToHistory(DropdownDataProvider.currentSelected, "_AlphaFalloff", slider.value * 0.2f);
        printToCSV(DropdownDataProvider.currentSelected.name.ToString(), "Falloff", slider.value.ToString());
    }

    public void UpSetSpecular(Slider slider)
    {
        setSpecular(slider);
        RedoUndo.addToHistory(DropdownDataProvider.currentSelected, "_Glossiness", slider.value * 0.4f);
        printToCSV(DropdownDataProvider.currentSelected.name.ToString(), "Specularity", slider.value.ToString());
    }

}
