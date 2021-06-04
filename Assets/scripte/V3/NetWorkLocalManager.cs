using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Mirror;
using Unity.Mathematics;
using UnityEngine;

public class NetWorkLocalManager : NetworkBehaviour
{
    public LobbyScripteV3 PrefabHUDLobby;
    public LobbyScripteV3 HUDLobby;
    public RaceManager RaceManager;
    public static int SpawnIndex;
    public static List<PlayerNetData> playerNetDatas = new List<PlayerNetData>();

    public TankController PrefabTank;
    public TankController Tank;
    public Vector3 SpawnPos = new Vector3(1,1,1);

    [Header("Server Shit")]
    public float IntervalCheck=2;
    private float _intervaltimer;
    [Header("Race Parameters")] 
    public int selectedRace=0;
    public float TimeBeforeRace=10;
    private float _raceTimer;
    public float TimeStartRace = 3;
    private float _startRaceTime;
    public float TimeEndRace = 30;
    public static  float _timeEndRace;
    private List<PlayerNetData> _racers = new List<PlayerNetData>();
    private  static List<PlayerResultat> _playerResultats = new List<PlayerResultat>();
    
    
    
    void Start()
    {
        if (isLocalPlayer) {
            HUDLobby = Instantiate(PrefabHUDLobby);
            HUDLobby.OnEnterGame += AddPlayer;
            RaceManager = GameObject.Find("RaceManager").GetComponent<RaceManager>();
        }
    }

    public void StartNewRace(int index) {
        if (isServer)
        {
            selectedRace = index;
            CMDStartNewRace();
        }
    }

    public void StopRaceHost()
    {
        CMDStopRace();
    }

    public void AddPlayer()
    {
        Debug.Log("lance sur cette instance  le AddPlayer");
        CMDAddPlayer( HUDLobby.PlayerName, HUDLobby.PlayerColor, this);
        if (isServer) {
            HUDLobby.SetHostThings();
            HUDLobby.OnStartNewRaceGame += StartNewRace;
            HUDLobby.OnCMDStopRace += StopRaceHost;
        }
    }

    public void FinishRace(float time)
    {
        CMDOnePlayerFinishRace(time , this);
        //SetTankControl(false);
        HUDLobby.SetOnRaceScoreBoard();
    }

    public Vector3 GetNextSpawnPoint(out Quaternion rot)
    {
        rot =GeneralSpawnPoints.GeneralsSpawnPoints[SpawnIndex].rotation;
        Vector3 pos = GeneralSpawnPoints.GeneralsSpawnPoints[SpawnIndex].position;
        SpawnIndex++;
        if (SpawnIndex >= GeneralSpawnPoints.GeneralsSpawnPoints.Count) SpawnIndex = 0;
        return pos;
    }
    [ClientRpc]
    public void UpdatePlayerListPanel(List<PlayerNetData> list)
    {
        Debug.Log("lance sur cette instance  le UpdateListPanel");
        if (HUDLobby!=null) HUDLobby.UpdateUIPlayerList(list);
    }

    [ClientRpc]
    public void SetRace(int raceIndex) {
        Debug.Log("resois l'ordre de Set la Race");
        if (RaceManager != null) {
            Debug.Log("lance l'ordre de Set la Race");
            RaceManager.SetRace(raceIndex);
            RaceManager.OnRaceEnd = FinishRace;
        }
    }
    [ClientRpc]
    public void StartRace() {
        if (RaceManager != null) {
            RaceManager.StartRace();
        }
    }
    [ClientRpc]
    public void StartRaceTimer() {
        if (HUDLobby!=null) HUDLobby.StartRaceTimer(TimeBeforeRace);
    }

    [ClientRpc]
    public void SetTankControl(bool value) {
        if (Tank != null) Tank.IsCOntrolled=value;
    }
    
    [ClientRpc]
    public void SetPosition(Vector3 pos , Quaternion ori) {
        if (Tank != null) {
            Tank.GetComponent<Rigidbody>().velocity = Vector3.zero;
            //Debug.Log( ("la position est de"+pos.position +" et la rotation est de "+ pos.rotation));
            Tank.GetComponent<NetworkTransform>().ServerTeleport(pos, ori);
            Tank.trailRenderer1.Clear();
            Tank.trailRenderer2.Clear();
        }
    }

    [ClientRpc]
    public void SendRaceResult(List<PlayerResultat> value) {
        if (HUDLobby != null) HUDLobby.SetRaceResult(value); 
    }
    
    [ClientRpc]
    public void CloseRace()
    {
        if (HUDLobby != null) HUDLobby.CloseResultPanel();
        if (RaceManager != null) RaceManager.CloseRace();
        
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
        if (selectedRace < RaceManager.Races.Count && selectedRace >= 0) StartCountToRace();
        else Debug.Log("l'index de la course ne correspond a aucune course");
    }
    [Command]public void CMDOnePlayerFinishRace(float time , NetWorkLocalManager netWorkLocalManager)
    {
        AddOneRaceResultat(time , netWorkLocalManager);
    }

    [Command]
    public void CMDStopRace()
    {
        StopRace();
    }

    // SERVEUR PART
    [Server]
    public void AddPlayerToList(string name , Color color, NetWorkLocalManager networkIdentity )
    {
        Debug.Log("lance sur cette instance  le AddPlayerToList");
        PlayerNetData newplayer = new PlayerNetData(name, color, networkIdentity);
        playerNetDatas.Add(newplayer);
        Debug.Log( "la list des Joueurs est de "+playerNetDatas.Count);
        foreach (PlayerNetData player in playerNetDatas) {
            player.Connection.UpdatePlayerListPanel(playerNetDatas);
        }
        SpawnTank(newplayer);
    }
    [Server]
    private void SpawnTank(PlayerNetData playerNetData)
    {
        Quaternion rot;
        Vector3 pos = GetNextSpawnPoint(out rot);
        TankController go= Instantiate(PrefabTank, pos,rot);
        go.color = playerNetData.Color;
        go.name = playerNetData.Name;
        NetworkServer.Spawn(go.gameObject, playerNetData.Connection.netIdentity.connectionToClient);
        go.netIdentity.AssignClientAuthority(playerNetData.Connection.netIdentity.connectionToClient);
        playerNetData.Connection.Tank = go;
    }

    [Server]
    private void StartCountToRace() {
        if (_raceTimer != 0) return;
        Debug.Log("Start new Race");
        _raceTimer = TimeBeforeRace;
        foreach (PlayerNetData player in playerNetDatas){
            player.Connection.StartRaceTimer();
            Debug.Log(" Order SetRace on "+player.Name);
        }
    }
    [Server]
    private void AddOneRaceResultat(float  time ,NetWorkLocalManager netWorkLocalManager) {
        
            if (_timeEndRace <= 0) _timeEndRace = TimeEndRace;
            foreach (PlayerNetData players in playerNetDatas)
            {
                if (players.Connection.netIdentity.connectionToClient ==
                    netWorkLocalManager.netIdentity.connectionToClient)
                {
                    _playerResultats.Add(new PlayerResultat(players, _playerResultats.Count + 1, time));
                    players.Connection.SetTankControl(false);
                }
            }

            foreach (PlayerNetData players in playerNetDatas)
            {
                players.Connection.SendRaceResult(_playerResultats);
            }
    }

    [Server]
    private void StopRace()
    {
        _timeEndRace = 0;
        foreach (PlayerNetData players in playerNetDatas) {
            players.Connection.CloseRace();
            Quaternion rot;
            Vector3 pos = GetNextSpawnPoint(out rot);
            players.Connection.SetPosition(pos, rot);
            players.Connection.SetTankControl(true);
        }
    }


    [ServerCallback]
    void Update()
    {
        if (_raceTimer != 0) {
            _raceTimer -= Time.deltaTime;
            if (_raceTimer <= 0) {
                _raceTimer = 0;
                _startRaceTime = TimeStartRace;
               _racers.Clear();
               _playerResultats.Clear();
               for (int i = 0; i < playerNetDatas.Count; i++)
               {
                   if (RaceManager.Races[selectedRace].StartPos.Count > i)
                   {
                       playerNetDatas[i].Connection.SetPosition(RaceManager.Races[selectedRace].StartPos[i].position,
                           RaceManager.Races[selectedRace].StartPos[i].rotation);
                       playerNetDatas[i].Connection.SetRace(selectedRace);
                       _racers.Add(playerNetDatas[i]);
                   }
                   else
                   {
                       Quaternion rot;
                       playerNetDatas[i].Connection.SetPosition(GetNextSpawnPoint(out rot) ,rot);
                       playerNetDatas[i].Connection.SetRace(selectedRace);
                   }
               }
               /*foreach (PlayerNetData player in playerNetDatas) {
                   player.Connection.SetPosition(new Vector3(pos, 1f, 1f));
                   player.Connection.Tank.IsCOntrolled = false;                    pos += 2;
               }*/
                
                Debug.Log(" Race Ready");
            }
        }
        
        if (_startRaceTime != 0) {
            _startRaceTime -= Time.deltaTime;
            if (_startRaceTime <= 0) {
                _startRaceTime = 0;
                foreach (PlayerNetData player in playerNetDatas) {
                    player.Connection.SetTankControl(true);
                    player.Connection.StartRace();
                }
                Debug.Log(" Start the RACE !!!");
            }
        }

        Debug.Log(" time de End race est de " + _timeEndRace);
        if (_timeEndRace != 0 && hasAuthority)
        {
            _timeEndRace -= Time.deltaTime;
            if (_timeEndRace <= 0) {
                _timeEndRace = 0;
                foreach (PlayerNetData players in playerNetDatas) {
                    players.Connection.CloseRace();
                    Quaternion rot;
                    Vector3 pos = GetNextSpawnPoint(out rot);
                    players.Connection.SetPosition(pos, rot);
                    players.Connection.SetTankControl(true);
                }
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

public struct PlayerResultat {
    public PlayerNetData Player;
    public int Place;
    public float Time;

    public PlayerResultat(PlayerNetData player, int place, float time) {
        Player = player;
        Place = place;
        Time = time;

    }
}
