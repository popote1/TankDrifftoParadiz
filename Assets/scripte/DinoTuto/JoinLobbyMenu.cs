using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
   [SerializeField] private NetWorkManagerLobby _netWorkManagerLobby = null;

   [Header("UI")] 
   [SerializeField] private GameObject _landingPanel = null;
   [SerializeField] private TMP_InputField _ipAddressInputFild = null;
   [SerializeField] private Button _joinButton = null;

   private void OnEnable()
   {
      NetWorkManagerLobby.OnClientConnected += HandelClientConnected;
      NetWorkManagerLobby.OnclienDisconected += HandleClientDisconnected;
   }

   private void OnDisable()
   {
      NetWorkManagerLobby.OnClientConnected -= HandelClientConnected;
      NetWorkManagerLobby.OnclienDisconected -= HandleClientDisconnected;  
   }

   public void JoinLobby()
   {
      string ipAddress = _ipAddressInputFild.text;
      _netWorkManagerLobby.networkAddress = ipAddress;
      _netWorkManagerLobby.StartClient();
   }

   private void HandelClientConnected()
   {
      _joinButton.interactable = true;
      gameObject.SetActive(false);
      _landingPanel.SetActive(false);
   }

   private void HandleClientDisconnected()
   {
      _joinButton.interactable = true;
   }
}
