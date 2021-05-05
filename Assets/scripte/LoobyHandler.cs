using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using  TMPro;
using UnityEngine.UI;

public class LoobyHandler : MonoBehaviour
{

    public TMP_InputField InputFieldPlayerName;
    public PlayerData PlayerData;
    public PlayerContaner PlayerContaner;
    public NetworkManager NetworkManager;

    public GameObject PrefabTank;
    public Transform spawnPos;
    public GameObject LobbyPanel;
    

    public void SetLobbyOn(PlayerData playerData, PlayerContaner playerContaner)
    {
        PlayerData = playerData;
        PlayerContaner = playerContaner;
        LobbyPanel.SetActive(true);
    }
    [Server]
    public void UIClickPlay()
    {
        PlayerData.Name =InputFieldPlayerName.text;
        
        GameObject go = Instantiate(PrefabTank, spawnPos.position, Quaternion.identity);
       // NetworkServer.Spawn(go, conn);
       go.GetComponent<NetworkIdentity>().AssignClientAuthority(PlayerContaner.connectionToClient);
        Debug.Log("NewPlayer "+InputFieldPlayerName.text);
        LobbyPanel.SetActive(false);
    }
}
