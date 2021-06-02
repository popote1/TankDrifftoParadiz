using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Authentication.ExtendedProtection;
using Mirror;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


public class TankController : NetworkBehaviour
{
    
    public float MovePover = 10;
    public float RotationPower = 10;
    public float TorrelRotationSencibility=1;
    public Transform Tourret;
    public TrailRenderer trailRenderer1;
    public TrailRenderer trailRenderer2;
    public MeshRenderer MeshRenderer;
    public GameObject PanelName;
    public static Transform ActiveCam;
    public TMP_Text PlayerName;

    private Rigidbody _rigidbody;
    [SyncVar]public bool IsCOntrolled = true;
    [SyncVar] public  Color color;
    [SyncVar] public  string name;

    [Header("DriftMeter Info")] 
    public float TargetVelocity=3;
    public float VelocityFactor = 1;
    public float DriftFactor = 1;

    public float DritfMater;

    public float DriftValue;
    



    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        SetColor();
        SetName();
    }

    void Update()
    {
        if (hasAuthority&&IsCOntrolled) {
            if (Input.GetAxisRaw("Vertical") != 0) {
                _rigidbody.AddForce(transform.forward * MovePover * Time.deltaTime * Input.GetAxisRaw("Vertical"));
            }
            if (Input.GetAxisRaw("Horizontal") != 0) {
                //_rigidbody.AddTorque(Vector3.up * Input.GetAxisRaw("Horizontal") * Time.deltaTime * RotationPower);
                transform.Rotate(transform.up, Input.GetAxisRaw("Horizontal") * Time.deltaTime * RotationPower);
            }
            Tourret.up = _rigidbody.velocity.normalized;
        }

        if (ActiveCam!=null) PanelName.transform.forward = transform.position - ActiveCam.position;
    }

    private void DriftCalculator()
    {
        
    }
    
    public void SetColor()
    {
        trailRenderer1.material.color = color ;
        trailRenderer2.material.color = color ;
        MeshRenderer.material.color = color;  
    }

    private void SetName()
    {
        PlayerName.text = name;
    }
    
}
