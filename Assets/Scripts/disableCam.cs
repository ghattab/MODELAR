using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableCam : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount > 2) {
            GetComponent<Camera>().targetTexture = null;
            this.gameObject.SetActive(false);

        }
    }
}
