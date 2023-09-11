using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class xpBar : MonoBehaviour
{
    public StatsApplier player;

    Slider xpSlider;

    void Start()
    {
        player = GameObject.Find("Player(Clone)").GetComponent<StatsApplier>();
        xpSlider = GetComponent<Slider>();

        if (player.level > 1)
        {
            xpSlider.minValue = (player.levelOneCap * (player.level - 1)) * ((player.level - 1) * player.levelCapMod);
            xpSlider.maxValue = (player.levelOneCap * player.level) * (player.level * player.levelCapMod);
        }
        else
        {
            xpSlider.minValue = 0;
            xpSlider.maxValue = player.levelOneCap / 4;
        }

        xpSlider.value = player.xp;
    }
}
