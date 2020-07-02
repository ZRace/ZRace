using CBGames.Core;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CBGames.UI
{
    public class TrackConnectionStatus : MonoBehaviour
    {
        [SerializeField] protected Text[] texts = new Text[] { };
        [SerializeField] protected UnityEvent OnConnectedToLobby = new UnityEvent();
        [SerializeField] protected UnityEvent OnConnectedToRoom = new UnityEvent();

        protected bool _firedLobbyEvents = false;
        protected bool _firedRoomEvents = false;

        protected virtual void OnEnable()
        {
            _firedLobbyEvents = false;
            _firedRoomEvents = false;
        }

        protected virtual void Update()
        {
            if (NetworkManager.networkManager)
            {
                SetText(NetworkManager.networkManager._connectStatus);
            }
            if (PhotonNetwork.IsConnected)
            {
                if (_firedLobbyEvents == false && PhotonNetwork.InLobby)
                {
                    OnConnectedToLobby.Invoke();
                }
                if (_firedRoomEvents == false && PhotonNetwork.InRoom)
                {
                    OnConnectedToRoom.Invoke();
                }
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