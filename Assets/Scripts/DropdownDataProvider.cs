using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownDataProvider : MonoBehaviour
{
    [System.Serializable]
    public struct DropdownItem
    {
        public Dropdown.OptionData optData;
        public GameObject obj;
    }

    public List<DropdownItem> items;

    private Dropdown dropdown;
    public static GameObject currentSelected;
    public static MeshRenderer currentSelectedMr;
    // Start is called before the first frame update
    void Start()
    {
        dropdown = GetComponent<Dropdown>();
        UpdateDropdownList();
        currentSelected = items[0].obj;
        currentSelectedMr = currentSelected.GetComponent<MeshRenderer>();
        dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdown);
        });
    }

    public void UpdateDropdownList()
    {
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        foreach(DropdownItem item in items)
        {
            options.Add(item.optData);
        }

        dropdown.options = options;
    }

    void DropdownValueChanged(Dropdown change)
    {
        currentSelected = items[change.value].obj;
        FindObjectOfType<SliderApply>().UpdateSliders();
        currentSelectedMr = currentSelected.GetComponent<MeshRenderer>();

    }
}
