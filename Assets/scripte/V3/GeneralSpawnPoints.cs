using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralSpawnPoints : MonoBehaviour
{
   public static List<Transform> GeneralsSpawnPoints= new List<Transform>();

   public void Start()
   {
      GeneralsSpawnPoints.Add(transform);
   }
}
