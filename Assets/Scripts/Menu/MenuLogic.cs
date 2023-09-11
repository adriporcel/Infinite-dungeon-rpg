using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class MenuLogic : MonoBehaviour
{
    public GameObject toast, mainMenu, characterSelection, nameInput, nameInputFieldObject, resumeButton;
    public Transform settingsMenuPosition;
    public Vector3 settingsClosedPosition, settingsDeployPosition, settingsTargetPosition;
    public TMP_Text fpsButtonText;

    string selectedClass;
    string[] names;
    int nameListLenght;

    TMP_InputField nameInputField;
    GLOBAL GLOBAL;

    private void Start()
    {
        GLOBAL = FindObjectOfType<GLOBAL>();

        if (PlayerPrefs.GetInt("ResumeGameAvailable") == 1)
        {
            resumeButton.SetActive(true);
        }

        nameInputField = nameInputFieldObject.GetComponent<TMP_InputField>();

        names = GetComponent<CharacterNameList>().names;        

        nameListLenght = names.Length;
    }

    private void Update()
    {
        if (settingsMenuPosition.localPosition != settingsTargetPosition)
        {
            if (settingsTargetPosition == settingsDeployPosition) // Menu opening position Lerp correction
            {
                if (settingsMenuPosition.localPosition.y < settingsDeployPosition.y + 5)
                    settingsMenuPosition.localPosition = settingsDeployPosition;
            }
            else if (settingsTargetPosition == settingsClosedPosition)
            {
                if (settingsMenuPosition.localPosition.y > settingsClosedPosition.y - 5)
                    settingsMenuPosition.localPosition = settingsClosedPosition;
            }

            settingsMenuPosition.localPosition = Vector3.Lerp(settingsMenuPosition.localPosition, settingsTargetPosition, 3 * Time.deltaTime);
        }
    }

    public void SetUpNewGame()
    {
        mainMenu.SetActive(false);
        characterSelection.SetActive(true);
    }
    public void SetUpNewGameCancel()
    {
        mainMenu.SetActive(true);
        characterSelection.SetActive(false);
    }

    public void ClassButtonPressed() // Can happen after SetUpNewGame()
    {
        selectedClass = EventSystem.current.currentSelectedGameObject.name; // Returns the name of the Button pressed

        if (selectedClass == "Rogue")
            GetComponent<PlayerClasses>().Rogue();
        else if (selectedClass == "Warrior")
            GetComponent<PlayerClasses>().Warrior();
        else if (selectedClass == "TimeLord")
            GetComponent<PlayerClasses>().TimeLord();
        else if (selectedClass == "Berserker")
            GetComponent<PlayerClasses>().Berserker();

        nameInput.SetActive(true);
    }

    public void AcceptNewNameStartGame()
    {
        string inputValue = nameInputField.text.ToString();
        if (inputValue != "") // No blank name input
        {

            GLOBAL.playerName = inputValue;
            GetComponent<PlayerClasses>().SaveStats();
            StartGame();
        }
    }

    public void ResumeGame()
    {
        GLOBAL.LoadGame();
    }

    public void ReturnRandomName()
    {
        // Uses CharacterNameList.cs
        string randomName = names[Random.Range(0, nameListLenght)];

        nameInputField.text = randomName;
    }

    public void CancelNameInput()
    {
        nameInput.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Fight");
    }
    public void SettingsButton()
    {
        if (settingsTargetPosition == settingsDeployPosition)
            settingsTargetPosition = settingsClosedPosition;
        else
            settingsTargetPosition = settingsDeployPosition;
    }

    // Settings buttons

    public void SetFpsButtonText(string framerateText)
    {
        fpsButtonText.SetText(framerateText + " FPS");
    }

    public void ChangeFps()
    {
        if (GLOBAL.frameRate == GLOBAL.lowFps)
            GLOBAL.frameRate = GLOBAL.highFps;
        else
            GLOBAL.frameRate = GLOBAL.lowFps;

        GameObject _newToast = Instantiate(toast);
        _newToast.GetComponent<Toast>().message = "FPS set to " + GLOBAL.frameRate.ToString();
        GLOBAL.SetFps();
    }

}
