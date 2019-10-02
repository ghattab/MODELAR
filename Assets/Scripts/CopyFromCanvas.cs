using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[ExecuteInEditMode]
public class CopyFromCanvas : MonoBehaviour
{
    RectTransform rectTrans;
    // Start is called before the first frame update
    void Start()
    {
        rectTrans = GetComponent<RectTransform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        rectTrans.position = transform.parent.GetComponent<RectTransform>().position;
        //rectTrans.localScale = transform.parent.GetComponent<RectTransform>().localScale;
        rectTrans.sizeDelta = transform.parent.GetComponent<RectTransform>().sizeDelta;
        Debug.Log("Copied");
    }
}
