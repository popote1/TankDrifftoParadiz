using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMenu : MonoBehaviour
{
    public GameObject PrefhbTank;
    public Transform SpawnPo1;
    public Transform SpawnPo2;
    public Transform SpawnPo3;
    public Transform SpawnPo4;
    public GameObject SpawnPanel;

    public void SpawnAtPo1()
    {
        Instantiate(PrefhbTank, SpawnPo1.position, Quaternion.identity);
        SpawnPanel.SetActive(false);
    }
    public void SpawnAtPo2()
    {
        Instantiate(PrefhbTank, SpawnPo2.position, Quaternion.identity);
        SpawnPanel.SetActive(false);
    }
    public void SpawnAtPo3()
    {
        Instantiate(PrefhbTank, SpawnPo3.position, Quaternion.identity);
        SpawnPanel.SetActive(false);
    }
    public void SpawnAtPo4()
    {
        Instantiate(PrefhbTank, SpawnPo4.position, Quaternion.identity);
        SpawnPanel.SetActive(false);
    }
    
    
}
