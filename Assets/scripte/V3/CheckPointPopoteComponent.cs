using UnityEngine;
using System;
using Mirror;

[RequireComponent(typeof(BoxCollider))]
public class CheckPointPopoteComponent : MonoBehaviour
{

    public Action OnCheckPointPass ;
    public bool IsActive{
        get => _isActive;
        set {
            if (value == false)  Marquer.enabled = _isActive =false;
            else Marquer.enabled = _isActive = false;
              
        }
    }
    private bool _isActive;
    
    public SpriteRenderer Marquer;
    private BoxCollider Collider;
    
    
    void Awake()
    {
        Collider = GetComponent<BoxCollider>();
        Collider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<NetworkIdentity>() != null) {
            if (other.GetComponent<NetworkIdentity>().hasAuthority)
            {
                Debug.Log("Le joueur a passer le check point");
                IsActive = false;
                OnCheckPointPass?.Invoke();
            }
        }
    }
}
