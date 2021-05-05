using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerContaner : NetworkBehaviour
{
    public PlayerData PlayerData;


    public void Start()
    {
        PlayerData = new PlayerData();
        GameObject.Find("___NetWorkManager___").GetComponent<LoobyHandler>().SetLobbyOn(PlayerData, this);
    }
}
