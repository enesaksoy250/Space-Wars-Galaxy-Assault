using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UsernameInput : MonoBehaviour
{

   private TMP_InputField inputField;

    private void Awake()
    {

        inputField = GetComponent<TMP_InputField>();

    }

    void Start()
    {

        inputField.onValueChanged.AddListener(ValidateInputLength);

    }

   
    private void ValidateInputLength(string input)
    {

        if(input.Length > 10)
        {

            inputField.text = input.Substring(0, 10);

        }

    }

}
