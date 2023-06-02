using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUIElement : MonoBehaviour
{
    public void UpdateElementInt(int number, Text element)
    {
        element.text = "Score: " + number;
    }
}
