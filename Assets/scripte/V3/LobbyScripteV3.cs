using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
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
    public GameObject PanelHostInRace;
    public event Action<int> OnStartNewRaceGame;
    public Action OnCMDStopRace;
    public GameObject PanelTimerRace;
    public Text TxtRaceTimer;

    public Dropdown DropdownRaceSelector;
    public GameObject PanelRaceScoreBoard;
    public GameObject PanelRaceScoreBoardHolder;
    public Text TxtTimeToEnd;

    public UIRacePlayerScore PrefabPlayerScore;




    [Header("Player data")] 
    public string PlayerName;
    public Color PlayerColor =Color.gray;


    private float _raceTimer;
    private float _secondeTimer;
    private float _TimeEndRace;
    private List<PlayerResultat> _playerResultats = new List<PlayerResultat>();
    private bool _isHost;
    
    
    void Start()
    {
        
    }
    public void UIStartNewRace()
    {
        OnStartNewRaceGame.Invoke(DropdownRaceSelector.value);
    }

    public void UIStopRace()
    {
        OnCMDStopRace.Invoke();
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
        _isHost = true;
    }

    public void StartRaceTimer(float value)
    {
        _raceTimer = value;

        TxtRaceTimer.text = Mathf.FloorToInt(value) + "";
        PanelTimerRace.SetActive(true);
        if (_isHost) {
            PanelHost.SetActive(false);
            PanelHostInRace.SetActive(true);
        }
        
    }

    public void SetOnRaceScoreBoard()
    {
        PanelRaceScoreBoard.SetActive(true);
        _playerResultats.Clear();
        
    }

    public void SetRaceResult(List<PlayerResultat> resutls)
    {
        Debug.Log(" le nombre de résultat envoyer et de "+resutls.Count+" alors que les résulatr déja enregister sont "+ _playerResultats.Count);
       /* if (PanelRaceScoreBoardHolder.transform.childCount == 0)
        {
            _TimeEndRace = 30;
            foreach (PlayerResultat resultat in resutls) {
                UIRacePlayerScore panel = Instantiate(PrefabPlayerScore, PanelRaceScoreBoardHolder.transform);
                panel.TxtPosition.text = (PanelRaceScoreBoardHolder.transform.childCount)+ "";
                panel.TxtPlayerName.text = resultat.Player.Name;
                panel.TxtTime.text = resultat.Time + "";
            }
            _playerResultats = resutls;
        }
        else*/
        //{
        if (_TimeEndRace==0)_TimeEndRace = 30;
            PlayerResultat resultat = resutls[resutls.Count - 1];
            UIRacePlayerScore panel = Instantiate(PrefabPlayerScore, PanelRaceScoreBoardHolder.transform);
            panel.TxtPosition.text = resultat.Place + "";
            panel.TxtPlayerName.text = resultat.Player.Name;
            panel.TxtTime.text = resultat.Time + "";
            _playerResultats .Add(resultat);
        //}
    }

    public void CloseResultPanel()
    {
        PanelRaceScoreBoard.SetActive(false);
        for (int i = 0; i < PanelRaceScoreBoardHolder.transform.childCount; i++)
        {
            Destroy(PanelRaceScoreBoardHolder.transform.GetChild(i).gameObject,0.1f);
        }

        if (_isHost)
        {
            PanelHost.SetActive(true);
            PanelHostInRace.SetActive(false);
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

        if (_TimeEndRace != 0)
        {
            _TimeEndRace -= Time.deltaTime;
            if (_TimeEndRace <= 0) _TimeEndRace = 0;
            TxtTimeToEnd.text = Mathf.FloorToInt(_TimeEndRace) + "";
        }
    }
}
