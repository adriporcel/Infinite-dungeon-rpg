using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatText : MonoBehaviour
{
    float _speed = 3f;
    float _lifeSpan = 0.8f;

    TextMeshProUGUI _thisText;
    GLOBAL _GLOBAL;

    private void Start()
    {
        _thisText = gameObject.GetComponent<TextMeshProUGUI>();
        _GLOBAL = FindObjectOfType<GLOBAL>();
    }

    void FixedUpdate()
    {
        // why does this use FixedUpdate? TO DO
        transform.position += new Vector3(0f, _speed * _GLOBAL.timeMultiplier, 0f);

        if (_lifeSpan <= 0)
            _thisText.alpha -= Time.fixedDeltaTime * _GLOBAL.timeMultiplier;
        else
            _lifeSpan -= Time.fixedDeltaTime * _GLOBAL.timeMultiplier;

        if (_thisText.alpha <= 0)
            Destroy(gameObject);
    }
}