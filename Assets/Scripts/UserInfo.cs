using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfo : MonoBehaviour
{

    public static InputField nameF;

    public void Start()
    {
        nameF = GetComponent<InputField>();
    }

}
