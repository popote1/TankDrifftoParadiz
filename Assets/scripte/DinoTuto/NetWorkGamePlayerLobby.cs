using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetWorkGamePlayerLobby : NetworkBehaviour
{

    [SyncVar] private string _displayName = "Loading ...";

    private NetWorkManagerLobby _room;

    private NetWorkManagerLobby Room
    {
        get
        {
            if (_room != null) return _room;
            return _room = NetworkManager.singleton as NetWorkManagerLobby;
        }
    }
    

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);
        Room.GamePlayers.Add(this);
       
    }

    public override void OnStopClient()
    {
        Room.GamePlayers.Remove(this);
    }
    
    [Server]
    public void SetDisplayName(string displayName)
    {
        _displayName = displayName;
    }
  
}


