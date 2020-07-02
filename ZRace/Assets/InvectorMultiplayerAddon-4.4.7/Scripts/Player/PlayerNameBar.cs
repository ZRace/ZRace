using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace CBGames.Player
{
    [RequireComponent(typeof(PhotonView))]
    [DisallowMultipleComponent]
    public class PlayerNameBar : MonoBehaviour
    {
        [Tooltip("The text to modify with the Network Nickname.")]
        public Text playerName;
        [Tooltip("The holder object for the player name bar. Will disable this if not a network version of this player.")]
        public GameObject playerBar;

        public virtual void Awake()
        {
            if (GetComponent<PhotonView>().IsMine == true)
            {
                SetPlayerName(PhotonNetwork.NickName);
                Destroy(playerBar);
            }
        }
        public virtual void SetPlayerName(string nameText)
        {
            playerName.text = nameText;
            GetComponent<PhotonView>().RPC("NetworkSetPlayerName", RpcTarget.OthersBuffered, nameText);
        }

        [PunRPC]
        protected virtual void NetworkSetPlayerName(string nameText)
        {
            playerName.text = nameText;
        }
    }
}
