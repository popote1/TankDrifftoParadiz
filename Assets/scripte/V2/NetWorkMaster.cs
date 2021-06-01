using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NetWorkMaster : NetworkBehaviour
{
    [SyncVar]public List<PlayerNetData> playerNetDatas = new List<PlayerNetData>();
    public NetworkIdentity NetworkIdentity;

    public  event Action UpdatePleyrList;
    private bool _isReady;
    // Start is called before the first frame update
    private void Start()
    {
        NetworkIdentity = GetComponent<NetworkIdentity>();
        
        //NetworkServer.Spawn(gameObject ,NetworkIdentity.connectionToServer);
    }

    private void Update()
    {
        if (!_isReady)
        {
            Debug.Log("Is Server ="+isServer);   
            Debug.Log("Is Client="+isClient);
            Debug.Log("Is ClientOnly ="+isClientOnly);
            Debug.Log("has Autority ="+hasAuthority);
            Debug.Log("is Serveur Only="+isServerOnly);
            //NetworkIdentity.AssignClientAuthority(NetworkIdentity.connectionToServer);
            
            
            _isReady = true;
        }
    }

    public void SetAutority(NetworkConnection connection)
    {
        NetworkIdentity.AssignClientAuthority(connection);
        Debug.Log("Is Server ="+isServer);   
        Debug.Log("Is Client="+isClient);
        Debug.Log("Is ClientOnly ="+isClientOnly);
        Debug.Log("has Autority ="+hasAuthority);
        Debug.Log("is Serveur Only="+isServerOnly);
    }

    public void AddPlayerData(PlayerNetData player)
    {
        playerNetDatas.Add(player);
        DoUpDate();
    }

    //[Command]
    public void DoUpDate()
    {
        PlayUpdate();
    }
    
   // Serveur Parte
   
   //[ClientRpc]
   public void PlayUpdate()
   {
       UpdatePleyrList.Invoke();
   }
}
