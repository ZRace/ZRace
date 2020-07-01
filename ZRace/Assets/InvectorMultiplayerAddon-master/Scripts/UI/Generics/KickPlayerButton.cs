using Photon.Pun;
using UnityEngine;

namespace CBGames.UI
{
    public class KickPlayerButton : MonoBehaviour
    {
        [SerializeField] private PlayerListObject playerInfo = null;
        protected UICoreLogic logic;

        protected virtual void Start()
        {
            logic = FindObjectOfType<UICoreLogic>();    
        }

        public virtual void KickPlayer()
        {
            if (!playerInfo) return;
            if (!string.IsNullOrEmpty(playerInfo.userId) && PhotonNetwork.IsMasterClient == true)
            {
                logic.KickPlayer(playerInfo.userId);
            }
        }
    }
}