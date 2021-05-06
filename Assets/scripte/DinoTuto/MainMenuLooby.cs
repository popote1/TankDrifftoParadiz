using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuLooby : MonoBehaviour
{
    [SerializeField] private NetWorkManagerLobby netWorkManager = null;
    [Header("UI")]
    [SerializeField] private GameObject LangingPagePanel = null;

    public void HostLobby()
    {
        netWorkManager.StartHost();
        LangingPagePanel.SetActive(false);
    }
}
