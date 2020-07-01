using CBGames.Core;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace CBGames.UI
{
    public class PlayerListObject : MonoBehaviour
    {
        [HideInInspector] public string userId = null;
        [HideInInspector] public string sceneName = "Unknown";
        [SerializeField] private Text playerNameText = null;
        [SerializeField] private Text location = null;
        [SerializeField] private Text ownerText = null;
        [SerializeField] private Text readyText = null;
        [SerializeField] private Image playerImage = null;
        [SerializeField] private Image showIfOwner = null;
        [SerializeField] private GameObject readyImage = null;
        [SerializeField] private GameObject notReadyImage = null;
        [SerializeField] private bool hideLocationIfNotSet = false;

        private bool isReady = false;
        private UICoreLogic logic = null;

        private void Start()
        {
            logic = NetworkManager.networkManager.GetComponentInChildren<UICoreLogic>();
            if (GetComponent<PhotonView>())
            {
                object[] data = GetComponent<PhotonView>().InstantiationData;
                if (data != null)
                {
                    foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
                    {
                        if (player.UserId == (string)data[0])
                        {
                            SetPlayerContents(player);
                            SetReadyState(logic.PlayerIsReady(player.UserId));
                        }
                    }
                    Transform parentToSet = StaticMethods.FindTargetChild((int[])data[1], logic.transform);
                    transform.SetParent(parentToSet);
                    transform.localScale = new Vector3(1, 1, 1);
                    transform.position = Vector3.zero;
                }
            }
            PhotonNetwork.NetworkingClient.EventReceived += RecievedPhotonEvent;
        }
        private void OnDestroy()
        {
            PhotonNetwork.NetworkingClient.EventReceived -= RecievedPhotonEvent;
        }

        public void SetContents(PlayerListInfo info)
        {
            userId = info.userId;
            playerNameText.text = userId.Split(':')[0];
            if (location != null)
            {
                sceneName = "Unknown Location";
                if (info.sceneIndex > 9999 && hideLocationIfNotSet == true)
                {
                    location.gameObject.SetActive(false);
                }
                else
                {
                    location.gameObject.SetActive(true);
                    if (info.sceneIndex == -1)
                    {
                        sceneName = "Lobby";
                    }
                    else if (info.sceneIndex < NetworkManager.networkManager.database.storedScenesData.Count)
                    {
                        sceneName = NetworkManager.networkManager.database.storedScenesData.Find(x => x.index == info.sceneIndex).sceneName;
                    }
                }
                location.text = sceneName;
            }
        }
        public void SetPlayerContents(Photon.Realtime.Player player, string isOwnerText = "ROOM OWNER", string nonOwnerText = "")
        {
            userId = player.UserId;
            if (playerNameText != null)
            {
                playerNameText.text = player.NickName;
            }
            if (ownerText != null)
            {
                ownerText.text = (player.IsMasterClient == true) ? isOwnerText : nonOwnerText;
            }
            if (showIfOwner != null)
            {
                showIfOwner.gameObject.SetActive(player.IsMasterClient);
            }
        }
        public void SetPlayerImage(Sprite image)
        {
            if (playerImage != null)
            {
                playerImage.sprite = image;
            }
        }
        public void SetReadyState(bool inputIsReady)
        {
            isReady = inputIsReady;
            if (readyImage != null)
            {
                readyImage.SetActive(isReady);
            }
            if (notReadyImage != null)
            {
                notReadyImage.SetActive(!isReady);
            }
            if (readyText != null)
            {
                readyText.text = (isReady == true) ? "READY" : "NOT READY";
            }
        }
        public bool GetReadyState()
        {
            return isReady;
        }

        private void RecievedPhotonEvent(EventData obj)
        {
            if (obj.Code == PhotonEventCodes.CB_EVENT_READYUP)
            {
                object[] data = (object[])obj.CustomData;
                bool isReady = (bool)data[0];
                string receivedUserId = (string)data[1];
                if (userId == receivedUserId)
                {
                    SetReadyState(isReady);
                }
            }
        }
    }
}