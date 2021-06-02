using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public List<RaceComponent> Races;


    private bool _isInRace;
    private int _indexSelectedRace;
    private int _indexActiveCheckpoint;
    private int _indexActiveTurn;

    public void PassCheckPoint()
    {
        if (_isInRace)
        {
            _indexActiveCheckpoint++;
            if (_indexActiveCheckpoint >= Races[_indexSelectedRace].CheckPointPopoteComponents.Count)
            {
                _indexActiveTurn++;
                if ( _indexActiveTurn >= Races[_indexSelectedRace].NbDeTours)
                {
                    Debug.Log(" Cours Terminer !");
                    return;
                } // ToDO ==> Mack EndRace MEthode

                _indexActiveCheckpoint = 0;
                Races[_indexSelectedRace].CheckPointPopoteComponents[0].IsActive = true;
            }

            else Races[_indexSelectedRace].CheckPointPopoteComponents[_indexActiveCheckpoint].IsActive = true;
        }
    }

    public void SetRace(int raceIndex)
    {
        Debug.Log("SetRaceData");
        _indexSelectedRace = raceIndex;
        _isInRace = true;
        Races[_indexSelectedRace].gameObject.SetActive(true);
        foreach (CheckPointPopoteComponent checkPoint in Races[_indexSelectedRace].CheckPointPopoteComponents) {
              checkPoint.IsActive = false;
              checkPoint.OnCheckPointPass = PassCheckPoint;
        }
        Races[_indexSelectedRace].CheckPointPopoteComponents[0].IsActive = true;
        _indexActiveCheckpoint = 0;
        _indexActiveTurn = 0;
    }
    
}
