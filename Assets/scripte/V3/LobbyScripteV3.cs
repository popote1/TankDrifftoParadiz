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

    public GameObject PanelRaceScoreBoard;
    public GameObject PanelRaceScoreBoardHolder;

    public UIRacePlayerScore PrefabPlayerScore;




    [Header("Player data")] 
    public string PlayerName;
    public Color PlayerColor =Color.gray;


    private float _raceTimer;
    private float _secondeTimer;
    private List<PlayerResultat> _playerResultats;
    
    
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

    public void SetOnRaceScoreBoard()
    {
        PanelRaceScoreBoard.SetActive(true);
        _playerResultats.Clear();
        for (int i = 0; i < PanelRaceScoreBoardHolder.transform.childCount; i++)
        {
            Destroy(PanelRaceScoreBoardHolder.transform.GetChild(i),0.1f);
        }
    }

    public void SetRaceResult(List<PlayerResultat> resutls)
    {
        if (_playerResultats.Count == 0) {
            foreach (PlayerResultat resultat in resutls) {
                UIRacePlayerScore panel = Instantiate(PrefabPlayerScore, PanelRaceScoreBoardHolder.transform);
                panel.TxtPosition.text = resultat.Place + "";
                panel.TxtPlayerName.text = resultat.Player.Name;
                panel.TxtTime.text = resultat.Time + "";
            }
        }
        else
        {
            PlayerResultat resultat = resutls[resutls.Count - 1];
            UIRacePlayerScore panel = Instantiate(PrefabPlayerScore, PanelRaceScoreBoardHolder.transform);
            panel.TxtPosition.text = resultat.Place + "";
            panel.TxtPlayerName.text = resultat.Player.Name;
            panel.TxtTime.text = resultat.Time + "";
        }

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
