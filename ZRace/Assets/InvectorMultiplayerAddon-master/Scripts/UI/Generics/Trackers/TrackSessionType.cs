using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace CBGames.UI
{ 
    public class TrackSessionType : MonoBehaviour
    {
        [SerializeField] protected Text[] texts = new Text[] { };

        protected virtual void FixedUpdate()
        {
            if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
            {
                SetText((string)PhotonNetwork.CurrentRoom.CustomProperties[RoomProperty.RoomType]);
            }
            else
            {
                SetText("");
            }
        }
        protected virtual void SetText(string inputText)
        {
            foreach (Text text in texts)
            {
                text.text = inputText;
            }
        }
    }
}