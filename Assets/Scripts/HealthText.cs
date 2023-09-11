using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthText : MonoBehaviour
{
    TextMeshProUGUI thisText;
    Slider sliderParent;
    float timeReset = 0.2f;
    float time;

    private void Start()
    { 
        sliderParent = transform.parent.GetComponent<Slider>();
        thisText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        time -= Time.deltaTime;

        if (time <= 0 )
        {
            time = timeReset;
            thisText.SetText(sliderParent.value.ToString("f0"));
        } 
    }
}
