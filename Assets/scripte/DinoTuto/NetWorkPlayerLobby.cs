using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetWorkPlayerLobby : NetworkBehaviour
{
    [Header("UI")] 
    [SerializeField] private GameObject _lobbyUI = null;

    [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[4];
    [SerializeField] private TMP_Text[] playerReadyTexts = new TMP_Text[4];
    [SerializeField] private Button StartGameButton = null;

    [SyncVar(hook = nameof( HandleDisplayNameChanged))] public string DisplayName = "Loading ...";
    [SyncVar(hook = nameof(HandleReadyStatusChanged))] public bool IsReady = false;

    private bool _isLeader;

    public bool IsLeader
    {
        set
        {
            _isLeader = value;
            StartGameButton.gameObject.SetActive(value);
        } 
    }

    private NetWorkManagerLobby _room;

    private NetWorkManagerLobby Room
    {
        get
        {
            if (_room != null) return _room;
            return _room = NetworkManager.singleton as NetWorkManagerLobby;
        }
    }

    public override void OnStartAuthority()
    {
        CmdSetDisplayName(PlayerNamer.DisplayName);
        _lobbyUI.SetActive(true);
        Debug.Log("Autority Is Given");
            
    }

    public override void OnStartClient()
    {
        Room.RoomPlayers.Add(this);
        UpdateDisplay();
    }

    public override void OnStopClient()
    {
        Room.RoomPlayers.Remove(this);
        UpdateDisplay();
    }

    public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();
    public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();

    private void UpdateDisplay()
    {
        if (!hasAuthority)
        {
            foreach (var player in Room.RoomPlayers)
            {
                if (player.hasAuthority)break;
            }

            return;
        }

        for (int i = 0; i < playerNameTexts.Length; i++)
        {
            playerNameTexts[i].text = "Waiting For Player ...";
            playerReadyTexts[i].text = String.Empty;
        }

        for (int i = 0; i < Room.RoomPlayers.Count; i++)
        {
            playerNameTexts[i].text = Room.RoomPlayers[i].DisplayName;
            playerReadyTexts[i].text = Room.RoomPlayers[i].IsReady
                ? "<color=green>Ready</color>"
                : "<color=red> Not Ready</color>";

        }
    }

    public void HandleReadyToStart(bool readyToStart)
        {
            if (!_isLeader) return;
            StartGameButton.interactable = readyToStart;
        }

    [Command]
    public void CmdSetDisplayName(string displayName)
    {
        DisplayName = displayName;
    }

    [Command]
    public void CmdReadyUp()
    {
        IsReady = !IsReady;
        Room.NotifyPlayerOfReadyState();

    }

    [Command]
    public void CmdStarGame()
    {
        if (Room.RoomPlayers[0].connectionToClient != connectionToClient) return;
        
        //Start Game;
    }
}


