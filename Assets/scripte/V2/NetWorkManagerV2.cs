using Mirror;
using UnityEngine;

public class NetWorkManagerV2 : NetworkManager
{
    [Header("test Data")]
    public LobbyManagerV2 LobbyManagerV2;
    public NetWorkMaster NetWorkMaster;
    public NetworkIdentity PlayerObject;
    public NetworkIdentity LocalPlayer;

    /*public void AddPlayerToGame(string name, Color color)
    {
        NetWorkMaster.UpdatePleyrList += UpdatePlayerList;
        NetWorkMaster.AddPlayerData( new PlayerNetData(name , color ,PlayerObject));
        Debug.Log("AddPlerIngame");
    }*/
    

    public void UpdatePlayerList()
    {
        LobbyManagerV2.UpdateUIPlayerList(NetWorkMaster.playerNetDatas);
    }
}

