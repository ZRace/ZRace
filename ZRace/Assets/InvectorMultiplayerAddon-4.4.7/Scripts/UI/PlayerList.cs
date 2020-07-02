using UnityEngine;
using CBGames.Core;
using System.Collections;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace CBGames.UI
{
    [RequireComponent(typeof(ChatBox))]
    [RequireComponent(typeof(NetworkManager))]
    public class PlayerList : MonoBehaviour
    {
        #region Modifiables
        public enum PressType { OnHold, OnPress, Invoke }
        [Tooltip("The gameobject that will hold the content of your scroll view.")]
        [SerializeField] protected GameObject content = null;
        [Tooltip("When enabling/disable this playerList it will enable/disable this object.")]
        [SerializeField] protected GameObject rootObj = null;
        [Tooltip("The object that will hold the visual item that will be " +
            "instantiated as a child of the content gameobject. This object " +
            "must have the \"PlayerListObject\" component attached to it's root.")]
        [SerializeField] protected GameObject playerJoinObject = null;
        [Tooltip("OnHold = Display the player window when you're holding the selected key.\n\n" +
            "OnPress = Display the player window and close the player window with the key press.\n\n" +
            "Invoke = Have another script open the window. No key press will open this window.")]
        [SerializeField] protected PressType openWindow = PressType.OnPress;
        [SerializeField] protected string keyToPress = "PlayerList";
        [Tooltip("Automatically close the playerlist window when the chatbox is open.")]
        [SerializeField] protected bool autoCloseWithChatBox = true;
        [Tooltip("How long to wait after a button press to disable/enable the chat window. " +
            "This is helpful if you are playing an animation.")]
        [SerializeField] protected float delayDisable = 2.0f;
        [Tooltip("If supplied will trigger the open/close animations on this component.")]
        [SerializeField] protected Animation anim = null;
        [Tooltip("The open animation name to play on the anim component.")]
        [SerializeField] protected string openAnimation = "OpenList";
        [Tooltip("The close animation name to play on the anim component.")]
        [SerializeField] protected string closeAnimation = "CloseList";
        [Tooltip("The AudioSource that the open/close sounds will play from.")]
        [SerializeField] protected AudioSource soundSource = null;
        [Tooltip("The sound that will play when opening the player list window.")]
        [SerializeField] protected AudioClip openSound = null;
        [Range(0,1)]
        [SerializeField] protected float openSoundVolume = 0.5f;
        [Tooltip("The sound that will play when closing the player list window.")]
        [SerializeField] protected AudioClip closeSound = null;
        [Range(0, 1)]
        [SerializeField] protected float closeSoundVolume = 0.5f;
        [Tooltip("If you want to see everything this is doing in runtime. Turn on debugging to log to the console.")]
        [SerializeField] protected bool debugging = false;
        #endregion

        #region Internal Use Variables
        protected bool enableWindow = false;
        protected ChatBox chatbox;
        protected bool isRunning = false;
        #endregion

        #region Initializations
        protected virtual void Awake()
        {
            rootObj.SetActive(false);
        }
        protected virtual void Start()
        {
            chatbox = FindObjectOfType<ChatBox>();
        }
        #endregion

        #region Management
        public virtual void UpdatePlayer(PlayerListInfo info)
        {
            if (debugging == true) Debug.Log("PLAYER LIST- Updating player list with: " + info.userId);
            foreach (Transform child in content.transform)
            {
                if (child.GetComponent<PlayerListObject>().userId == info.userId)
                {
                    if (debugging == true) Debug.Log("PLAYERLIST - Found player, updating: " + info.userId+ " sceneIndex: "+info.sceneIndex);
                    child.GetComponent<PlayerListObject>().SetContents(info);
                    break;
                }
            }
        }

        public virtual void AddPlayer(string chatUserId)
        {
            AddPlayer(new PlayerListInfo(
                chatUserId,
                true
            ));
        }
        public virtual void AddPlayers(List<PlayerListInfo> players)
        {
            foreach (PlayerListInfo player in players)
            {
                AddPlayer(player);
            }
        }
        public virtual void AddPlayer(PlayerListInfo player)
        {
            if (debugging == true) Debug.Log("PLAYERLIST - Adding player: " + player.userId);
            GameObject obj = Instantiate(playerJoinObject) as GameObject;
            obj.transform.SetParent(content.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.GetComponent<PlayerListObject>().SetContents(player);
        }

        public virtual void RemovePlayer(string chatUserId)
        {
            RemovePlayer(new PlayerListInfo(
                chatUserId,
                false
            ));
        }
        public virtual void RemovePlayer(PlayerListInfo player)
        {
            if (debugging == true) Debug.Log("PLAYERLIST - Attempting To Remove Player: " + player.userId);
            foreach (Transform child in content.transform)
            {
                if (child.GetComponent<PlayerListObject>().userId == player.userId)
                {
                    if (debugging == true) Debug.Log("PLAYERLIST - Found player, removing: " + player.userId);
                    Destroy(child.gameObject);
                    break;
                }
            }
        }

        public virtual void ClearPlayerList(string temp)
        {
            ClearPlayerList();
        }
        public virtual void ClearPlayerList()
        {
            if (debugging == true) Debug.Log("PLAYERLIST - Clearing player list");
            foreach (Transform child in content.transform)
            {
                Destroy(child.gameObject);
            }
        }
        #endregion

        #region ChatBox
        public virtual void UpdateLocationToGoingToScene()
        {
            NetworkManager.networkManager.GetChabox().BroadcastData(
                NetworkManager.networkManager.GetChatDataChannel(),
                new PlayerListInfo(
                    NetworkManager.networkManager.GetChabox().GetUserId(),
                    true,
                    NetworkManager.networkManager.GetCurrentSceneIndex()
                )
            );
        }
        public virtual void UpdateLocationToCurrentScene()
        {
            NetworkManager.networkManager.GetChabox().BroadcastData(
                NetworkManager.networkManager.GetChatDataChannel(),
                new PlayerListInfo(
                    NetworkManager.networkManager.GetChabox().GetUserId(),
                    true,
                    SceneManager.GetActiveScene().buildIndex
                )
            );
        }
        public virtual List<string> GetDataSubScribers()
        {
            if (debugging == true) Debug.Log("PLAYERLIST - Getting data channel subscribers.");
            List<string> subscribers = new List<string>();
            subscribers = FindObjectOfType<ChatBox>().GetChannelSubscribers(NetworkManager.networkManager.GetChatDataChannel());
            if (debugging == true) Debug.Log("PLAYERLIST - Subscribers count: "+subscribers.Count);
            return subscribers;
        }
        public virtual void SetPlayerList()
        {
            if (debugging == true) Debug.Log("PLAYERLIST - Setting player list ...");
            List<string> userIds = GetDataSubScribers();
            foreach(string userId in userIds)
            {
                AddPlayer(new PlayerListInfo(
                    userId,
                    true
                ));
            }
        }
        #endregion

        #region Enable/Disable
        public virtual void EnablePlayerListWindow(bool isEnabled)
        {
            if (isRunning == true) return;
            StartCoroutine(DoEnable(isEnabled));
        }
        protected virtual IEnumerator DoEnable(bool isEnabled)
        {
            isRunning = true;
            if (anim != null)
            {
                if (isEnabled == true)
                {
                    rootObj.SetActive(isEnabled);
                }
                anim.Play((isEnabled == true) ? openAnimation : closeAnimation);
            }
            PlaySound((isEnabled == true) ? openSound : closeSound, (isEnabled == true) ? openSoundVolume : closeSoundVolume);
            yield return new WaitForSeconds(delayDisable);
            rootObj.SetActive(isEnabled);
            isRunning = false;
        }
        #endregion

        #region Sounds
        protected virtual void PlaySound(AudioClip soundClip = null, float volumeLevel = 0.5f)
        {
            if (soundClip == null || soundSource == null) return;
            soundSource.clip = soundClip;
            soundSource.volume = volumeLevel;
            soundSource.Play();
        }
        #endregion

        protected virtual void Update()
        {
            if (openWindow == PressType.Invoke || isRunning == true || PhotonNetwork.InRoom == false) return;
            if (autoCloseWithChatBox == true && chatbox.IsEnabled() == true)
            {
                if (enableWindow == true)
                {
                    enableWindow = false;
                    EnablePlayerListWindow(false);
                }
                return;
            }
            if (openWindow == PressType.OnHold)
            {
                if (Input.GetButton(keyToPress) && chatbox.IsEnabled() == false)
                {
                    if (enableWindow == false)
                    {
                        enableWindow = true;
                        EnablePlayerListWindow(true);
                    }
                }
                else
                {
                    enableWindow = false;
                    EnablePlayerListWindow(false);
                }
            }
            else if (openWindow == PressType.OnPress)
            {
                if (Input.GetButtonDown(keyToPress) && chatbox.IsEnabled() == false)
                {
                    enableWindow = !enableWindow;
                    EnablePlayerListWindow(enableWindow);
                }
            }
        }
    }
}