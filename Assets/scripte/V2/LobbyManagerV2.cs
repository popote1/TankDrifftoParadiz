using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LobbyManagerV2 : MonoBehaviour
{

   public NetWorkManagerV2 NetWorkManagerV2;
   [Header(("UI Elements"))]
   public GameObject LogInPanel;
   public InputField InputFieldPlayerName;
   public Dropdown PlayerColor;

   public Text TxtPlayersInGame;
/*
   public void UIJoinGame()
   {
      //if (InputFieldPlayerName.text != null) return;
      Color color = Color.gray;
      switch (PlayerColor.value)
      {
         case 0: color = Color.red;
            break;
         case 1:
            color = Color.blue;
            break;
         case 2:
            color = Color.green;
            break;
      }
      NetWorkManagerV2.AddPlayerToGame(InputFieldPlayerName.text, color);
      LogInPanel.SetActive(false);
   }
*/
   public void UpdateUIPlayerList(List<PlayerNetData> playerNetDatas)
   {
      string playerinfo = "Players In Game";
      foreach (PlayerNetData player in playerNetDatas)
      {
         playerinfo = playerinfo + " \n -" + player.Name;
      }
      TxtPlayersInGame.text = playerinfo;
   }
}
