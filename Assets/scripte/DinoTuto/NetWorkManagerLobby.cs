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

    public static event Action OnClientConnected;
    public static event Action OnclienDisconected;

    public List<NetWorkPlayerLobby> RoomPlayers { get; } = new List<NetWorkPlayerLobby>();

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
}
