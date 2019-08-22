using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeToSubmitScree : MonoBehaviour
{
    public GameObject guipre, guiafter;
    
    public void change()
    {
        guipre.SetActive(false);
        guiafter.SetActive(true);
    }
}
