using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Toast : MonoBehaviour
{
    /* How to display custom toast, add to the script that spawns this prefab

    public GameObject toast;

    Instantiate(toast);
    GameObject.Find("Toast(Clone)").message = "message";
    */

    public TextMeshProUGUI text;
    public string message;

    CanvasGroup _fade;
    float _timer = 2f;

    void Start()
    {
        _fade = GetComponent<CanvasGroup>();
        text.SetText(message.ToString());
    }

    void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0)
            _fade.alpha = Mathf.Lerp(_fade.alpha, 0, 4 * Time.deltaTime);

        if (_fade.alpha < 0.05)
            DestroyImmediate(gameObject);
    }
}
