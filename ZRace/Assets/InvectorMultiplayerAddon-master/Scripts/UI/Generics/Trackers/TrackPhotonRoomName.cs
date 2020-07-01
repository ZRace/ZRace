using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace CBGames.UI
{
    public class TrackPhotonRoomName : MonoBehaviour
    {
        [SerializeField] protected Text[] texts = new Text[] { };

        protected virtual void FixedUpdate()
        {
            if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
            {
                SetText(PhotonNetwork.CurrentRoom.Name);
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