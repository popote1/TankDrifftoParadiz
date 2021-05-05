using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Mirror;
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

    private Rigidbody _rigidbody;
    private Color _color;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _color = Random.ColorHSV();

        trailRenderer1.material.color = _color ;
        trailRenderer2.material.color = _color ;
        MeshRenderer.material.color = _color;


    }

    void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                _rigidbody.AddForce(transform.forward * MovePover * Time.deltaTime * Input.GetAxisRaw("Vertical"));
            }

            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                //_rigidbody.AddTorque(Vector3.up * Input.GetAxisRaw("Horizontal") * Time.deltaTime * RotationPower);
                transform.Rotate(transform.up, Input.GetAxisRaw("Horizontal") * Time.deltaTime * RotationPower);
            }

            
            Tourret.up = _rigidbody.velocity.normalized;
        }
    }
}
