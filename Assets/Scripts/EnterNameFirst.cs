using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnterNameFirst : MonoBehaviour
{
    public static float time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(time > 0)
        {
            GetComponent<Text>().color = new Color(1f, 1f, 1f, time);
        } else
        {
            GetComponent<Text>().color = new Color(1f, 1f, 1f, 0);
        }
        time -= Time.deltaTime;
    }
}
