using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class OnEnterPlayer : MonoBehaviour
{
    public NetworkIdentity NNetworkIdentity;
    void Start()
    {
        
        NNetworkIdentity = GetComponent<NetworkIdentity>();
        Debug.Log("Is Server ="+NNetworkIdentity.isServer);
        GameObject.Find("___NetWorkManager___").GetComponent<NetWorkManagerV2>().LocalPlayer = NNetworkIdentity;
        GameObject NetMaster = GameObject.Find("___NetWorkManager___");
        NetMaster.GetComponent<NetworkIdentity>().AssignClientAuthority(NNetworkIdentity.connectionToServer);

        //.GetComponent<NetWorkManagerV2>().NetWorkMaster.SetAutority(NetworkIdentity.connectionToClient);
    }

    
}
