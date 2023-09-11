using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class FightSceneButtons : MonoBehaviour
{
    GameManager _gameManager;
    GLOBAL _GLOBAL;
    public GameObject pauseBackground, confirmExitMenu;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _GLOBAL = FindObjectOfType<GLOBAL>();
    }

    public void ToggleGameReady()
    {
        _gameManager.gamePaused = !_gameManager.gamePaused;
        pauseBackground.SetActive(_gameManager.gamePaused);
        GameObject.Find("Pause").transform.SetAsLastSibling(); // Fix layer order, bring to foreground
    }

    public void MainMenuButton()
    {
        // Check menu
        confirmExitMenu.SetActive(!confirmExitMenu.activeSelf);
    }

    public void ConfirmExit()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void FastForward()
    {
        int _mutiplier = _GLOBAL.timeMultiplier;

        // TO DO x4 premium??
        if (_mutiplier == 1)
            _GLOBAL.timeMultiplier = 2;
        else if (_mutiplier == 2)
            _GLOBAL.timeMultiplier = 3; 
        else if (_mutiplier == 3)
            _GLOBAL.timeMultiplier = 1;
    }
}
