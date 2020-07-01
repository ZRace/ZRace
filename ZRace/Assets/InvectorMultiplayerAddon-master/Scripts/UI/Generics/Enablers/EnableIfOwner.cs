using CBGames.Core;
using Photon.Pun;
using UnityEngine;

namespace CBGames.UI
{
    public class EnableIfOwner : MonoBehaviour
    {
        [SerializeField] protected bool isOwner = true;
        [SerializeField] protected GameObject[] targets = new GameObject[] { };

        protected virtual void FixedUpdate()
        {
            if (NetworkManager.networkManager && PhotonNetwork.InRoom == true && PhotonNetwork.IsMasterClient == isOwner)
            {
                SetIsActive(true);
            }
            else
            {
                SetIsActive(false);
            }
        }

        protected virtual void SetIsActive(bool isActive)
        {
            foreach (GameObject target in targets)
            {
                if (target.activeInHierarchy != isActive)
                {
                    target.SetActive(isActive);
                }
            }
        }
    }
}