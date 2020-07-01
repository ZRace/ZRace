using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Photon.Pun.UtilityScripts
{
    public class PlayerCount : MonoBehaviourPunCallbacks
    {
        [Tooltip("The max number of players allowed in room. Once full, a new room will be created by the next connection attemping to join.")]
        public int playerCount;

        public int maxPlayersRoom = 4;
        public TextMeshProUGUI textPlayerCount;
        private void FixedUpdate()
        {
            if (PhotonNetwork.CurrentRoom != null)
                playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

            textPlayerCount.text = playerCount + "/" + maxPlayersRoom;
        }

    }
}
