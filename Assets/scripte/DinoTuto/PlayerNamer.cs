using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerNamer : MonoBehaviour
{
    [Header("UI")] public TMP_InputField nameInputField;
    public Button continueButton;
    
    public static string DisplayName { get; private set; }

    private const string PlayerPrefsNameKey = "playerName";

    private void Start() => SetUpInputField();


    private void SetUpInputField()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)){return;}

        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);
        nameInputField.text = defaultName;
        SetPlayerName(defaultName);
    }

    public void SetPlayerName(string name)
    {
        continueButton.interactable = !string.IsNullOrEmpty(name);
    }

    public void SavePlayerName()
    {
        DisplayName = nameInputField.text;
        PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
    }
}
