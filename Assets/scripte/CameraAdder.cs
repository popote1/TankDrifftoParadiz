using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Mirror;

public class CameraAdder :NetworkBehaviour
{
    public CinemachineVirtualCamera Cam;
    void Start()
    {
        if (isLocalPlayer)
        {
            CinemachineVirtualCamera cam =Instantiate(Cam);
            cam.Follow = transform;
            cam.LookAt = transform;
        }
    }

    
    
}
