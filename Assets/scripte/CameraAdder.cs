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
        if (hasAuthority)
        {
            CinemachineVirtualCamera cam =Instantiate(Cam);
            cam.Follow = transform;
            cam.LookAt = transform;
            TankController.ActiveCam = cam.transform;
        }
    }

    
    
}
