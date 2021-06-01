using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScripteV3 : MonoBehaviour
{
    [Header("Logging shit")]
    public InputField InputFieldPlayerName;
    public Dropdown DropdownPlayerColor;
    public GameObject PanelLoging;
    public event Action OnEnterGame;
    [Header("In Game Panels")]
    public Text TxtPlayerInGameList;
    public GameObject PanelHost;
    public event Action OnStartNewRaceGame;
    public GameObject PanelTimerRace;
    public Text TxtRaceTimer;
    
    

    
    [Header("Player data")] 
    public string PlayerName;
    public Color PlayerColor =Color.gray;


    private float _raceTimer;
    private float _secondeTimer;
    
    
    void Start()
    {
        
    }
    public void UIStartNewRace()
    {
        OnStartNewRaceGame.Invoke();
    }
    public void UIJoinGame()
    {
        //if (InputFieldPlayerName.text != null) return;
        
        switch (DropdownPlayerColor.value)
        {
            case 0: PlayerColor = Color.red;
                break;
            case 1:
                PlayerColor = Color.blue;
                break;
            case 2:
                PlayerColor = Color.green;
                break;
        }
        PlayerName = InputFieldPlayerName.text;
        PanelLoging.SetActive(false);
        OnEnterGame.Invoke();
    }

    public void UpdateUIPlayerList(List<PlayerNetData> playerNetDatas)
    {
        string playerinfo = "Players In Game";
        foreach (PlayerNetData player in playerNetDatas)
        {
            playerinfo = playerinfo + " \n -" + player.Name;
        }
        TxtPlayerInGameList.text = playerinfo;
    }

    public void SetHostThings()
    {
        PanelHost.SetActive(true);
    }

    public void StartRaceTimer(float value)
    {
        _raceTimer = value;

        TxtRaceTimer.text = Mathf.FloorToInt(value) + "";
        PanelTimerRace.SetActive(true);
    }

    private void Update()
    {
        if (_raceTimer != 0) {
            _raceTimer -= Time.deltaTime;
            TxtRaceTimer.text = Mathf.FloorToInt(_raceTimer) + "";
            if (_raceTimer <= 0) {
                _raceTimer = 0;
                PanelTimerRace.SetActive(false);
            }
        }
    }
}
