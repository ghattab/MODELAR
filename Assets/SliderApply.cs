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
        transparancySlider.value = DropdownDataProvider.currentSelected.GetComponent<MeshRenderer>().material.GetFloat("_Alpha");
        transparancyFalloffSlider.value = DropdownDataProvider.currentSelected.GetComponent<MeshRenderer>().material.GetFloat("_AlphaFalloff");
        glossinessSlider.value = DropdownDataProvider.currentSelected.GetComponent<MeshRenderer>().material.GetFloat("_Glossiness");
        depthWeightSlider.value = DropdownDataProvider.currentSelected.GetComponent<MeshRenderer>().material.GetFloat("_Depth_Weight");
    }
    void setDepthWeight(Slider slider)
    {
        DropdownDataProvider.currentSelected.GetComponent<MeshRenderer>().material.SetFloat("_Depth_Weight", slider.value);
    }

    
    void setTranparancy(Slider slider)
    {
        DropdownDataProvider.currentSelected.GetComponent<MeshRenderer>().material.SetFloat("_Alpha", slider.value);
    }

    void setTranparancyFalloff(Slider slider)
    {
        DropdownDataProvider.currentSelected.GetComponent<MeshRenderer>().material.SetFloat("_AlphaFalloff", slider.value);
    }

    void setSpecular(Slider slider)
    {
        DropdownDataProvider.currentSelected.GetComponent<MeshRenderer>().material.SetFloat("_Glossiness", slider.value);
    }

    public static void printToCSV(string obj, string met, string val)
    {
        var csv = new StringBuilder();
        var newLine = string.Format("{0},{1},{2}", obj, met, val);
        csv.AppendLine(newLine);
        File.AppendAllText("History" + UserInfo.nameF.text + ".csv", csv.ToString());
    }
    public void UpSetDepthWeight(Slider slider)
    {
        setDepthWeight(slider);
        RedoUndo.addToHistory(DropdownDataProvider.currentSelected, "_Depth_Weight", slider.value);
        printToCSV(DropdownDataProvider.currentSelected.name.ToString(), "_Depth_Weight", slider.value.ToString());

    }
    public void UpSetTrancparancy(Slider slider)
    {
        setTranparancy(slider);
        RedoUndo.addToHistory(DropdownDataProvider.currentSelected, "_Alpha", slider.value);
        printToCSV(DropdownDataProvider.currentSelected.name.ToString(), "_Alpha", slider.value.ToString());

    }

    public void UpSetTranparancyFalloff(Slider slider)
    {
        setTranparancyFalloff(slider);
        RedoUndo.addToHistory(DropdownDataProvider.currentSelected, "_AlphaFalloff", slider.value);
        printToCSV(DropdownDataProvider.currentSelected.name.ToString(), "_AlphaFalloff", slider.value.ToString());
    }

    public void UpSetSpecular(Slider slider)
    {
        setSpecular(slider);
        RedoUndo.addToHistory(DropdownDataProvider.currentSelected, "_Glossiness", slider.value);
        printToCSV(DropdownDataProvider.currentSelected.name.ToString(), "_Glossiness", slider.value.ToString());
    }

}
