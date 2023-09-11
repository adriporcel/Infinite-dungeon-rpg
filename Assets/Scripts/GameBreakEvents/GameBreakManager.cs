using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBreakManager : MonoBehaviour
{
    public Transform mainCanvas;
    public GameObject levelUpPanel, shopPanel;

    GameObject levelUpPanelObject, shopPanelObject;

    GLOBAL GLOBAL;


    void Start()
    {
        GLOBAL = FindObjectOfType<GLOBAL>();

        if (GLOBAL.levelUpAvailable == 1)
        {
            InstantiatePanel("levelUp");
            GLOBAL.levelUpHealing = 1;
        }
        else if (GLOBAL.openShop == 1)
        {
            InstantiatePanel("shop");
        }
    }

    public void InstantiatePanel(string which)
    {
        if (which == "levelUp")
            levelUpPanelObject = Instantiate(levelUpPanel, mainCanvas);
        else if (which == "shop")
            shopPanelObject = Instantiate(shopPanel, mainCanvas);
    }

    public void ReloadGameBreak()
    {
        SceneManager.LoadScene("GameBreak");
    }

    public void ResumeGame()
    {
        SceneManager.LoadScene("Fight");
    }
}
