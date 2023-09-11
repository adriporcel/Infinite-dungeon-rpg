using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedUpButtonTextUpdater : MonoBehaviour
{
    int currentValue;
    string x = "x";
    GLOBAL _GLOBAL;

    void Start()
    {
        _GLOBAL = FindObjectOfType<GLOBAL>();
    }

    void Update()
    {
        if (_GLOBAL.timeMultiplier != currentValue)
        {
            currentValue = _GLOBAL.timeMultiplier;
            GetComponent<TextMeshProUGUI>().SetText(x + currentValue.ToString());
        }
    }
}
