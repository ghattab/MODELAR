using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CustomSlider : Slider
{
    //OnPointerDown is also required to receive OnPointerUp callbacks
    public override void OnPointerDown(PointerEventData eventData)
    {
    }

    //Do this when the mouse click on this selectable UI object is released.
    public override void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("The mouse click was released");
    }
}
