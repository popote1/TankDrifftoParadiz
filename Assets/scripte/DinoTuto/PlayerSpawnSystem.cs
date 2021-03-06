using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class PlayerSpawnSystem : NetworkBehaviour
{
   [SerializeField] private GameObject _playerPrefabs = null;
   
   private static List<Transform> spawnPoints = new List<Transform>();

   private int nextIndex = 0;

   public static void AddSpawnPoint(Transform tranform)
   {
      spawnPoints.Add(tranform);
      
      spawnPoints = spawnPoints.OrderBy(x=> x.GetSiblingIndex()).ToList();
   }

   public static void RemoveSpawnPoint(Transform transform) => spawnPoints.Remove(transform);

   public override void OnStartServer() => NetWorkManagerLobby.OnServerReadied += SpawnPlayer;

   [ServerCallback]
   private void OnDestroy() => NetWorkManagerLobby.OnServerReadied -= SpawnPlayer;
   
   public void SpawnPlayer(NetworkConnection conn)
   {
      Transform spawnPoint = spawnPoints.ElementAtOrDefault(nextIndex);
      Debug.Log("l'adress est " + conn.address);
      Debug.Log(" fait spawnner un joueur");
      GameObject playerInstance =
         Instantiate(_playerPrefabs, spawnPoints[nextIndex].position, spawnPoints[nextIndex].rotation);
      NetworkServer.Spawn(playerInstance, conn);
      playerInstance.GetComponent<TankController>().color = Random.ColorHSV();
      nextIndex++;
   }
}
