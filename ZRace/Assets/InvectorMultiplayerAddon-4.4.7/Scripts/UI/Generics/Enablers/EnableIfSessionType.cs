using CBGames.Core;
using Photon.Pun;
using UnityEngine;

namespace CBGames.UI
{ 
    public enum SessionType { Public, Private }

    public class EnableIfSessionType : MonoBehaviour
    {
        [SerializeField] protected bool isOwner = true;
        [SerializeField] protected SessionType type = SessionType.Public;
        [SerializeField] protected GameObject[] targets = new GameObject[] { };

        protected virtual void FixedUpdate()
        {
            if (NetworkManager.networkManager && PhotonNetwork.InRoom == true && PhotonNetwork.IsMasterClient == isOwner)
            {
                switch(type)
                {
                    case SessionType.Private:
                        if ((string)PhotonNetwork.CurrentRoom.CustomProperties[RoomProperty.RoomType] == RoomProperty.PrivateRoomType)
                        {
                            SetIsActive(true);
                        }
                        else
                        {
                            SetIsActive(false);
                        }
                        break;
                    case SessionType.Public:
                        if ((string)PhotonNetwork.CurrentRoom.CustomProperties[RoomProperty.RoomType] == RoomProperty.PublicRoomType)
                        {
                            SetIsActive(true);
                        }
                        else
                        {
                            SetIsActive(false);
                        }
                        break;
                }
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