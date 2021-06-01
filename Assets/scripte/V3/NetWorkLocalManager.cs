using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NetWorkLocalManager : NetworkBehaviour
{
    public LobbyScripteV3 PrefabHUDLobby;
    public LobbyScripteV3 HUDLobby;
    public static List<PlayerNetData> playerNetDatas = new List<PlayerNetData>();

    public TankController PrefabTank;
    public Vector3 SpawnPos = new Vector3(1,1,1);

    [Header("Server Shit")]
    public float IntervalCheck=2;
    private float _intervaltimer;
    [Header("Race Parameters")] 
    public float TimeBeforeRace=10;
    private float _raceTimer;
    
    
    void Start()
    {
        if (isLocalPlayer) {
            HUDLobby = Instantiate(PrefabHUDLobby);
            HUDLobby.OnEnterGame += AddPlayer;
        }
    }

    public void StartNewRace()
    {
        if (isServer)
        {
            
            CMDStartNewRace();
        }
    }

    public void AddPlayer()
    {
        Debug.Log("lance sur cette instance  le AddPlayer");
        CMDAddPlayer( HUDLobby.PlayerName, HUDLobby.PlayerColor, this);
        if (isServer) {
            HUDLobby.SetHostThings();
            HUDLobby.OnStartNewRaceGame += StartNewRace;
        }
    }
    [ClientRpc]
    public void UpdatePlayerListPanel(List<PlayerNetData> list)
    {
        Debug.Log("lance sur cette instance  le UpdateListPanel");
        if (HUDLobby!=null) HUDLobby.UpdateUIPlayerList(list);
    }

    [ClientRpc]
    public void StartRaceTimer()
    {
        if (HUDLobby!=null) HUDLobby.StartRaceTimer(TimeBeforeRace);
    }
    
    //Commandes
    [Command]
    public void CMDAddPlayer(string name , Color color, NetWorkLocalManager networkIdentity )
    {
        Debug.Log("lance sur cette instance  le CMDAddPlayer");
        AddPlayerToList( name, color, networkIdentity);
    }

    [Command]
    public void CMDStartNewRace()
    {
        StartCountToRace();
    }
    
    // SERVEUR PART
    [Server]
    public void AddPlayerToList(string name , Color color, NetWorkLocalManager networkIdentity )
    {
        Debug.Log("lance sur cette instance  le AddPlayerToList");
        PlayerNetData newplayer = new PlayerNetData(name, color, networkIdentity);
        playerNetDatas.Add(newplayer);
        Debug.Log( "la list des Joueurs est de "+playerNetDatas.Count);
        foreach (PlayerNetData player in playerNetDatas)
        {
            player.Connection.UpdatePlayerListPanel(playerNetDatas);
        }
        SpawnTank(newplayer);
    }
    [Server]
    private void SpawnTank(PlayerNetData playerNetData)
    {
        TankController go= Instantiate(PrefabTank, SpawnPos, Quaternion.identity);
        go.color = playerNetData.Color;
        go.name = playerNetData.Name;
        NetworkServer.Spawn(go.gameObject, playerNetData.Connection.netIdentity.connectionToClient);
        go.netIdentity.AssignClientAuthority(playerNetData.Connection.netIdentity.connectionToClient);
    }

    [Server]
    private void StartCountToRace()
    {
        if (_raceTimer != 0) return;
        Debug.Log("Start new Race");
        _raceTimer = TimeBeforeRace;
        foreach (PlayerNetData player in playerNetDatas)
        {
            player.Connection.StartRaceTimer();
        }
        
    } 
    
    
    [ServerCallback]
    void Update()
    {
        if (_raceTimer != 0)
        {
            _raceTimer -= Time.deltaTime;
            if (_raceTimer <= 0)
            {
                _raceTimer = 0;
                Debug.Log(" Start the RACE !!!");
            }
        }
        
        
        
        
        // Check for deconection
        _intervaltimer += Time.deltaTime;
        if (_intervaltimer > IntervalCheck) {
            bool doUpdate = false;
            _intervaltimer = 0;
            List<PlayerNetData> playersToRemove = new List<PlayerNetData>();
            foreach (PlayerNetData player in playerNetDatas) {
                if (player.Connection == null) {
                    playersToRemove.Add(player);
                    doUpdate = true;
                }
            }
            foreach (PlayerNetData player in playersToRemove) playerNetDatas.Remove(player);
            if (doUpdate) {
                foreach (PlayerNetData player in playerNetDatas) {
                    player.Connection.UpdatePlayerListPanel(playerNetDatas);
                }
            }
        }
    }
    
    
}
public struct PlayerNetData
{
    public string Name;
    public Color Color;
    public NetWorkLocalManager Connection;
    public PlayerNetData(string name, Color color ,NetWorkLocalManager connection ) {
        Name = name;
        Color = color;
        Connection = connection;
    }
}
