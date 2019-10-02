using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Category10ColorSelect : MonoBehaviour
{
    public GameObject buttonPrefab;
    public List<Color> colors;
    public bool col2 = false;
    // Start is called before the first frame update
    void Start()
    {
        int yPos = 0;
        foreach(Color color in colors)
        {
            GameObject button = Instantiate(buttonPrefab, this.transform);
            button.GetComponent<RectTransform>().localPosition = new Vector2(0,-yPos);
            ColorBlock newColor = new ColorBlock();
            newColor.normalColor = color;
            newColor.disabledColor = color;
            newColor.highlightedColor = color * 1.1f;
            newColor.selectedColor = new Color(newColor.highlightedColor.r, newColor.highlightedColor.g, newColor.highlightedColor.b, 1);
            newColor.pressedColor = color * 0.9f ;
            newColor.fadeDuration = 0.1f;
            newColor.colorMultiplier = 1f;
            button.GetComponent<Button>().colors = newColor;

            EventTrigger trigger = button.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((data) => {
                markSelectedButton(button);
                setCurrentColor(button.GetComponent<Button>());
            });
            trigger.triggers.Add(entry);

            button.GetComponent<Button>().onClick.AddListener(delegate {
               // markSelectedButton(button);
               // setCurrentColor(button.GetComponent<Button>());
            });

            yPos += 140;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        markSelectedButton(DropdownDataProvider.currentSelected.GetComponent<MeshRenderer>().material.GetColor("_Color"));
    }

    public void markSelectedButton(GameObject button)
    {
        foreach (GameObject cButton in GameObject.FindGameObjectsWithTag("ColorButton"))
        {
            cButton.GetComponentInChildren<Text>().text = "";
            cButton.GetComponent<RectTransform>().sizeDelta = new Vector2(140, 140);
        }
        button.GetComponentInChildren<Text>().text = "Selected";
        button.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
    }
    public static void markSelectedButton(Color color)
    {
        GameObject button = null;
        foreach (GameObject cButton in GameObject.FindGameObjectsWithTag("ColorButton"))
        {
            cButton.GetComponentInChildren<Text>().text = "";
            cButton.GetComponent<RectTransform>().sizeDelta = new Vector2(140, 140);
            if (cButton.GetComponent<Button>().colors.normalColor == color)
            {
                button = cButton;
            }
        }
        if(button != null) {
        button.GetComponentInChildren<Text>().text = "Selected";
        button.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
        }
    }

    void setCurrentColor(Button button)
    {
        if(!col2) { 
            DropdownDataProvider.currentSelected.GetComponent<MeshRenderer>().material.SetColor("_Color",button.colors.normalColor);
            RedoUndo.addToHistory(DropdownDataProvider.currentSelected, "_Color", button.colors.normalColor);
            SliderApply.printToCSV(DropdownDataProvider.currentSelected.name.ToString(), "Color", button.colors.normalColor.ToString());
        } else
        {
            DropdownDataProvider.currentSelected.GetComponent<MeshRenderer>().material.SetColor("_Color2", button.colors.normalColor);
            RedoUndo.addToHistory(DropdownDataProvider.currentSelected, "_Color2", button.colors.normalColor);
            SliderApply.printToCSV(DropdownDataProvider.currentSelected.name.ToString(), "Color2", button.colors.normalColor.ToString());
        }
    }
}
