using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public List<RaceComponent> Races;

    public Action<float> OnRaceEnd;

    
    private bool _isInRace;
    private bool _isStarted;
    private int _indexSelectedRace;
    private int _indexActiveCheckpoint;
    private int _indexActiveTurn;

    private float _raceTimer;
    public void PassCheckPoint() {
        if (_isInRace) {
            _indexActiveCheckpoint++;
            if (_indexActiveCheckpoint >= Races[_indexSelectedRace].CheckPointPopoteComponents.Count) {
                _indexActiveTurn++;
                if ( _indexActiveTurn >= Races[_indexSelectedRace].NbDeTours) {
                    Debug.Log(" Cours Terminer !");
                    OnRaceEnd.Invoke(_raceTimer);
                    _isStarted = false;
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
        _isStarted = false;
        _raceTimer = 0;
        _indexActiveCheckpoint = 0;
        _indexActiveTurn = 0;
    }

    public void StartRace()
    {
        _isStarted = true;
    }

    private void Update()
    {
        if (_isStarted) _raceTimer += Time.deltaTime;
    }
}
