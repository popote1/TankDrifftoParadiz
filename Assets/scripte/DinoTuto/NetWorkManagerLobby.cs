using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetWorkManagerLobby : NetworkManager
{
    [SerializeField] private int minPlayer = 2;
    [Scene] [SerializeField] private string menuScene ;

    [Header("Room")] 
    [SerializeField] private NetWorkPlayerLobby roomPlayerPrefab = null;

    [Header("Game")] [SerializeField] private NetWorkGamePlayerLobby _gamePlayerPrefabs = null;
    [SerializeField] private GameObject _playerSpawnStystem = null;
    public static event Action OnClientConnected;
    public static event Action OnclienDisconected;
    public static event Action<NetworkConnection> OnServerReadied;

    public List<NetWorkPlayerLobby> RoomPlayers { get; } = new List<NetWorkPlayerLobby>();
    public List<NetWorkGamePlayerLobby> GamePlayers { get; } = new List<NetWorkGamePlayerLobby>();

    public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();

    public override void OnStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");
        foreach (var prefab in spawnablePrefabs)
        {
            NetworkClient.RegisterPrefab(prefab);
        }
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        OnclienDisconected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        if (numPlayers >= maxConnections)
        {
            Debug.Log("trop de Joueur");
            conn.Disconnect();
            return;
        }

        if (SceneManager.GetActiveScene().path != menuScene)
        {
            Debug.Log("Mauvaise Scene, la scene est "+ menuScene+" alors quelle devrai être "+SceneManager.GetActiveScene().path);
            conn.Disconnect();
            return;
        }
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        if (SceneManager.GetActiveScene().path == menuScene)
        {
            bool isLeader = RoomPlayers.Count == 0;
            NetWorkPlayerLobby playerLobbyInstance = Instantiate(roomPlayerPrefab);

            playerLobbyInstance.IsLeader = isLeader;
            NetworkServer.AddPlayerForConnection(conn, playerLobbyInstance.gameObject);
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (conn.identity != null)
        {
            var player = conn.identity.GetComponent<NetWorkPlayerLobby>();
            RoomPlayers.Remove(player);
            NotifyPlayerOfReadyState();
        }
        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        RoomPlayers.Clear();
    }

    public void NotifyPlayerOfReadyState()
    {
        foreach (var player in RoomPlayers)
        {
            player.HandleReadyToStart(IsReadyToStart());
        }
    }

    private bool IsReadyToStart()
    {
        if (numPlayers < minPlayer) return false;
        foreach (var player in RoomPlayers)
        {
            if (!player.IsReady) return false;
        }

        return true;
    }

    public void StartGame()
    {
        if (SceneManager.GetActiveScene().path == menuScene)
        {
            if (!IsReadyToStart()) return;
            ServerChangeScene("SampleScene");
            Debug.Log("La scene est la bonne");
        }
    }

    public override void ServerChangeScene(string newSceneName)
    {
        if (SceneManager.GetActiveScene().path == menuScene )
        {
           
            for (int i = RoomPlayers.Count - 1; i >= 0; i--)
            {
                var conn = RoomPlayers[i].connectionToClient;
                var gameplayerInstance = Instantiate(_gamePlayerPrefabs);
                gameplayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);

                NetworkServer.Destroy(conn.identity.gameObject);
                NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject);
            }

            base.ServerChangeScene(newSceneName);
        }
        else  Debug.Log("les condition de ServerChangeScene ne sont pas remplit");
    }


    public override void OnServerChangeScene(string newSceneName)
    {
        if (newSceneName.StartsWith("SampleScene"))
        {
            Debug.Log("Cest la scene des spawn et spawn le systeme de Spawn");
            if (_gamePlayerPrefabs==null)Debug.Log("ya pas la ref du player spawn systeme");
            GameObject playerSpawnSystemeInstance = Instantiate(_playerSpawnStystem);
            NetworkServer.Spawn(playerSpawnSystemeInstance);
            DontDestroyOnLoad(playerSpawnSystemeInstance);
        }
        else
        {
            Debug.Log("Pas la bonne scene pour faire le spawn");
        }
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);
        OnServerReadied?.Invoke(conn);
    }

    
}
