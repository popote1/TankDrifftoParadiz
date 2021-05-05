using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class TankController : NetworkBehaviour
{
    public float MovePover = 10;
    public float RotationPower = 10;
    public float TorrelRotationSencibility=1;
    public Transform Tourret;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
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
        }
    }
}
