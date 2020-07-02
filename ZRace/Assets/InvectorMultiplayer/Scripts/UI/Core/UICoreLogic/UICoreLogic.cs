using CBGames.Core;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static CBGames.Core.CoreDelegates;

namespace CBGames.UI
{
    [Serializable]
    public class SceneOption
    {
        public string sceneName = "";
        public Sprite sceneSprite = null;

        public SceneOption(string inputName, Sprite inputSprite)
        {
            this.sceneName = inputName;
            this.sceneSprite = inputSprite;
        }
    }
    [Serializable]
    public class RoomListWrapper
    {
        public List<SceneOption> wrapper = new List<SceneOption>();
    }

    public partial class UICoreLogic : MonoBehaviour
    {
        #region Delegates
        public BasicDelegate teamsUpdated;
        public BasicDelegate voiceViewUpdated;
        #endregion

        #region Properties
        #region Session Settings
        [Tooltip("If you don't set the scene to load this is the default level index it will use.")]
        [SerializeField] protected int defaultLevelIndex = 0;
        [Tooltip("The complete list of players that are selectable to the end user.")]
        public GameObject[] selectablePlayers = new GameObject[] { };
        public UnityEvent OnStart = new UnityEvent();
        public SceneEvent OnSceneLoaded = new SceneEvent();
        #endregion

        #region Sound Options
        [Tooltip("The audio source that will play your menu music.")]
        [SerializeField] protected AudioSource musicSource = null;
        [Tooltip("The audio source that will play your menu sounds.")]
        [SerializeField] protected AudioSource soundSource = null;
        [Tooltip("The sound that will be played when you call the \"PlayerMouseEnter\" function.")]
        [SerializeField] protected AudioClip mouseEnter = null;
        [Range(0, 1)]
        [Tooltip("How loud to play the \"mouseEnter\" sound.")]
        [SerializeField] protected float mouseEnterVolume = 0.5f;
        [Tooltip("The sound that will be played when you call the \"PlayerMouseExit\" function.")]
        [SerializeField] protected AudioClip mouseExit = null;
        [Range(0, 1)]
        [Tooltip("How loud to play the \"mouseExit\" sound.")]
        [SerializeField] protected float mouseExitVolume = 0.5f;
        [Tooltip("The sound that will be played when you call the \"PlayerMouseClick\" function.")]
        [SerializeField] protected AudioClip mouseClick = null;
        [Range(0, 1)]
        [Tooltip("How loud to play the \"mouseClick\" sound.")]
        [SerializeField] protected float mouseClickVolume = 0.5f;
        [Tooltip("The sound that will be played when you call the \"PlayerFinalClick\" function.")]
        [SerializeField] protected AudioClip finalClick = null;
        [Range(0, 1)]
        [Tooltip("How loud to play the \"finalClick\" sound.")]
        [SerializeField] protected float finalClickVolume = 0.5f;
        [Tooltip("How loud to set your audio to.")]
        [SerializeField] protected float startVolume = 0.5f;
        [Tooltip("When first loading the UI, fade in the audio.")]
        [SerializeField] protected bool fadeInAudio = false;
        [Tooltip("If fading out your music how to fast to fade out. Higher values will fade out faster.")]
        [Range(0.01f, 5.0f)]
        [SerializeField] protected float fadeInSpeed = 0.25f;
        #endregion

        #region Player Settings
        public StringUnityEvent OnNameEnterFailed;
        public UnityEvent OnNameEnterSuccess;
        #endregion

        #region Room Settings
        [SerializeField] public List<SceneOption> sceneList = new List<SceneOption>();
        public StringUnityEvent OnCreateRoomFailed;
        public UnityEvent OnCreateRoomSuccess;
        public UnityEvent OnWaitToJoinPhotonRoomsLobby;
        public UnityEvent OnStartSession;
        #endregion

        #region Generic Network
        public StringUnityEvent OnNetworkError;
        [Tooltip("Log everything that happens to the unity console.")]
        [SerializeField] protected bool debugging = false;
        #endregion

        #region Loading Page
        [Tooltip("The Parent Object that holds all of the loading page transforms.")]
        public GameObject loadingParent = null;
        [Tooltip("The image that will display when loading the screen.")]
        public List<Sprite> loadingImages = new List<Sprite>();
        [Tooltip("How long to display each image before fading to the next one.")]
        [SerializeField] protected float loadingImageDisplayTime = 8.0f;
        [Tooltip("How fast to fade the images in and out.")]
        [SerializeField] protected float loadingPageFadeSpeed = 0.25f;
        [Tooltip("The title text that will be set when the loading screen is displayed")]
        public string loadingTitle = "";
        [Tooltip("The description text that will be set when the loading screen is displayed.")]
        public List<string> loadingDesc = new List<string>();
        [Tooltip("The image that will be displaying your main loading sprite")]
        [SerializeField] protected Image mainLoadingImage = null;
        [Tooltip("The text object that will be used to display your loading title text.")]
        [SerializeField] protected Text loadingTitleText = null;
        [Tooltip("The text object that will be used to display your loading description text.")]
        [SerializeField] protected Text loadingDescText = null;
        [Tooltip("The loading bar.")]
        [SerializeField] protected Image loadingBar = null;
        public UnityEvent OnStartLoading;
        public UnityEvent OnCompleteLevelLoading;
        #endregion

        #region Countdown
        public FloatUnityEvent OnCountdownStarted;
        public UnityEvent OnCountdownStopped;
        #endregion

        #region Misc
        public FloatUnityEvent OnReceiveRoundTime;
        public UnityEvent OnResetEverything;
        #endregion

        #region Internal Only
        protected bool _roomNameChecking = true;
        protected bool _fadeOutAudio = false;
        protected bool _trackLoadingBar = false;
        protected bool _cycleLoadingPage = false;
        protected bool _loadFading = false;
        protected bool _loadIsFadingOut = false;
        protected string _playerName = "";
        protected string _roomName = "";
        protected string _roomPassword = "";
        protected Dictionary<string, RoomInfo> _rooms = new Dictionary<string, RoomInfo>();
        protected Dictionary<string, bool> _playersReady = new Dictionary<string, bool>();
        protected Dictionary<string, string> _teamData = new Dictionary<string, string>();
        protected Dictionary<string, int> _playerVoiceChatViews = new Dictionary<string, int>();
        protected Dictionary<int, int> _sceneVotes = new Dictionary<int, int>();
        protected int _myPrevVote = -1;
        protected List<SceneOption> _randomSceneList = new List<SceneOption>();
        protected string _savedLevelName = "";
        protected int _savedLevelIndex = 0;
        protected int _selectedLoadImageIndex = 0;
        protected int _selectedDescTextIndex = 0;
        protected int _maxVote = 0;
        protected int _setPlayerIndex = 0;
        protected List<GameObject> _playerUIs = new List<GameObject>();
        protected float _loadingFadeTimer = 0.0f;
        protected Color _tempLoadColor = new Color();
        protected Color _tempLoadTextColor = new Color();
        #endregion

        #region Editor Only
        [HideInInspector] public bool e_events_oneTime = false;
        [HideInInspector] public bool e_events_loading = false;
        [HideInInspector] public bool e_events_naming = false;
        [HideInInspector] public bool e_events_errors = false;
        [HideInInspector] public bool e_events_room = false;
        [HideInInspector] public bool e_show_events = false;
        [HideInInspector] public bool e_show_loading = false;
        [HideInInspector] public bool e_show_audio = false;
        [HideInInspector] public bool e_show_core = false;
        [HideInInspector] public bool e_show_countdown = false;
        [HideInInspector] public bool e_show_misc = false;
        #endregion
        #endregion

        #region Initializations
        protected virtual void Start()
        {
            musicSource.volume = (fadeInAudio == false) ? startVolume : 0;
            _savedLevelIndex = defaultLevelIndex;
            List<DatabaseScene> sceneData = NetworkManager.networkManager.database.storedScenesData;
            _savedLevelName = sceneData.Find(x => x.index == _savedLevelIndex).sceneName;
            OnStart.Invoke();

            PhotonNetwork.NetworkingClient.EventReceived += RecievedPhotonEvent;
            SceneManager.sceneLoaded += SceneLoaded;
            NetworkManager.networkManager.OnPlayerJoinedCurrentRoom += AddNewPlayerToPlayerReadyList;
            NetworkManager.networkManager.OnPlayerLeftCurrentRoom += RemovePlayerToPlayerReadyList;

            CreateRandomSceneList();
        }
        public virtual void ResetEverything()
        {
            if (debugging == true) Debug.Log("Resetting all data...");
            _roomName = "";
            _roomPassword = "";
            _rooms.Clear();
            _playersReady.Clear();
            _teamData.Clear();
            _sceneVotes.Clear();
            _playersReady.Clear();
            _randomSceneList.Clear();
            _playerVoiceChatViews.Clear();
            _savedLevelName = "";
            _savedLevelIndex = 0;
            _selectedLoadImageIndex = 0;
            _selectedDescTextIndex = 0;
            _maxVote = 0;
            _playerUIs.Clear();
            SetPlayer(0);
            SceneManager.sceneLoaded -= SceneLoaded;
            PhotonNetwork.NetworkingClient.EventReceived -= RecievedPhotonEvent;
            NetworkManager.networkManager.OnPlayerJoinedCurrentRoom -= AddNewPlayerToPlayerReadyList;
            NetworkManager.networkManager.OnPlayerLeftCurrentRoom -= RemovePlayerToPlayerReadyList;
            OnResetEverything.Invoke();
            Start();
        }
        #endregion

        #region Heartbeat
        protected virtual void Update()
        {
            if (fadeInAudio == true)
            {
                musicSource.volume += Time.deltaTime * fadeInSpeed;
                if (musicSource.volume >= startVolume)
                {
                    musicSource.volume = startVolume;
                    fadeInAudio = false;
                }
            }
            if (_fadeOutAudio == true)
            {
                musicSource.volume -= Time.deltaTime * fadeInSpeed;
                if (musicSource.volume <= 0)
                {
                    musicSource.volume = 0;
                    _fadeOutAudio = false;
                }
            }
            if (_trackLoadingBar == true)
            {
                loadingBar.fillAmount = PhotonNetwork.LevelLoadingProgress;
                if (PhotonNetwork.LevelLoadingProgress == 1)
                {
                    if (debugging == true) Debug.Log("Completed Leveling Loading.");
                    OnCompleteLevelLoading.Invoke();
                    EnableLoadingPage(false);
                }
            }
            if (_cycleLoadingPage == true)
            {
                if (_loadFading == false)
                {
                    _loadingFadeTimer += Time.deltaTime;
                    if (_loadingFadeTimer >= loadingImageDisplayTime)
                    {
                        _loadIsFadingOut = (mainLoadingImage.color.a == 0) ? false : true;
                        _loadFading = true;
                    }
                }
                if (_loadFading == true)
                {
                    _tempLoadColor = mainLoadingImage.color;
                    _tempLoadTextColor = loadingDescText.color;
                    if (_loadIsFadingOut == false)
                    {
                        _tempLoadColor.a += Time.deltaTime * loadingPageFadeSpeed;
                        _tempLoadTextColor.a += Time.deltaTime * loadingPageFadeSpeed;
                    }
                    else
                    {
                        _tempLoadColor.a -= Time.deltaTime * loadingPageFadeSpeed;
                        _tempLoadTextColor.a -= Time.deltaTime * loadingPageFadeSpeed;
                    }
                    mainLoadingImage.color = _tempLoadColor;
                    loadingDescText.color = _tempLoadTextColor;
                    if (_tempLoadColor.a <= 0 || _tempLoadColor.a >= 1)
                    {
                        _loadFading = false;
                        _tempLoadColor.a = (_tempLoadColor.a <= 0) ? 0 : 1;
                        mainLoadingImage.color = _tempLoadColor;
                        if (mainLoadingImage.color.a == 0)
                        {
                            _selectedLoadImageIndex = (loadingImages.Count <= (_selectedLoadImageIndex + 1)) ? 0 : _selectedLoadImageIndex + 1;
                            mainLoadingImage.sprite = loadingImages[_selectedLoadImageIndex];

                            _selectedDescTextIndex = (loadingDesc.Count <= (_selectedDescTextIndex + 1)) ? 0 : _selectedDescTextIndex + 1;
                            loadingDescText.text = loadingDesc[_selectedDescTextIndex];
                        }
                    }
                }
            }
        }
        #endregion

        #region Sound/Music Events
        public virtual void RestartMusic()
        {
            musicSource.Stop();
            musicSource.Play();
        }
        public virtual void StopMusic()
        {
            musicSource.Stop();
        }
        public virtual void PlayMusic()
        {
            musicSource.Play();
        }
        public virtual void EnableMusic(bool isEnabled)
        {
            if (debugging == true) Debug.Log("Enable Music: " + isEnabled);
            musicSource.enabled = isEnabled;
        }
        public virtual void FadeMusic(bool fadeOut)
        {
            if (debugging == true) Debug.Log("Fade music: " + fadeOut);
            if (musicSource.volume < startVolume && fadeOut == false)
            {
                fadeInAudio = true;
                _fadeOutAudio = false;
            }
            else if (musicSource.volume > 0 && fadeOut == true)
            {
                fadeInAudio = false;
                _fadeOutAudio = true;
            }
        }
        public virtual void SetFadeToVolume(float fadeToVolume)
        {
            startVolume = fadeToVolume;
        }
        public virtual void PlayMouseEnter()
        {
            if (soundSource == null || mouseEnter == null) return;
            soundSource.clip = mouseEnter;
            soundSource.volume = mouseEnterVolume;
            soundSource.Play();
        }
        public virtual void PlayMouseExit()
        {
            if (soundSource == null || mouseExit == null) return;
            soundSource.clip = mouseExit;
            soundSource.volume = mouseExitVolume;
            soundSource.Play();
        }
        public virtual void PlayMouseClick()
        {
            if (soundSource == null || mouseClick == null) return;
            soundSource.clip = mouseClick;
            soundSource.volume = mouseClickVolume;
            soundSource.Play();
        }
        public virtual void PlayFinalClick()
        {
            if (soundSource == null || finalClick == null) return;
            soundSource.clip = finalClick;
            soundSource.volume = finalClickVolume;
            soundSource.Play();
        }
        public virtual void LoopMusic(bool setLooping)
        {
            musicSource.loop = setLooping;
        }
        public virtual void SetMusicVolume(float volume)
        {
            musicSource.volume = volume;
        }
        public virtual void SetMusicAudio(AudioClip clip)
        {
            musicSource.clip = clip;
        }
        #endregion

        #region Player Settings
        #region Player - Kick
        public virtual void KickPlayer(string userId)
        {
            StartCoroutine(VisualKickPlayer(userId));
        }
        IEnumerator VisualKickPlayer(string userId)
        {
            object[] data = new object[] { PhotonNetwork.LocalPlayer.UserId };
            RaiseEventOptions options = new RaiseEventOptions
            {
                CachingOption = EventCaching.DoNotCache,
                Receivers = ReceiverGroup.Others
            };
            if (debugging == true) Debug.Log("Raising CB_EVENT_KICKPLAYER photon event...");
            PhotonNetwork.RaiseEvent(PhotonEventCodes.CB_EVENT_KICKPLAYER, data, options, SendOptions.SendReliable);
            yield return new WaitForSeconds(0.1f);
            NetworkManager.networkManager.KickPlayer(userId);
        }
        #endregion

        #region Player - Set PlayerPrefab
        public virtual GameObject GetSetPlayer()
        {
            return NetworkManager.networkManager.playerPrefab;
        }
        public virtual int GetSetPlayerIndex()
        {
            return _setPlayerIndex;
        }
        public virtual void SetPlayer(int index)
        {
            _setPlayerIndex = index;
            if (debugging == true) Debug.Log("Set Network Manager player prefab from index: " + index);
            if (index < 0 || index >= selectablePlayers.Length) return;
            NetworkManager.networkManager.playerPrefab = selectablePlayers[index];
        }
        #endregion

        #region Player - Name
        public virtual void SubmitSavedPlayerName()
        {
            if (debugging == true) Debug.Log("Attempting to set the saved players network name: " + _playerName);
            if (string.IsNullOrEmpty(_playerName))
            {
                OnNameEnterFailed.Invoke("You must enter a name to continue.");
            }
            else if (_playerName.Contains(":"))
            {
                OnNameEnterFailed.Invoke("Name contains ':' which is not allowed.");
            }
            else if (_playerName.Contains("_"))
            {
                OnNameEnterFailed.Invoke("Name contains '_' which is not allowed.");
            }
            else
            {
                NetworkManager.networkManager.SetPlayerName(_playerName);
                OnNameEnterSuccess.Invoke();
            }
        }
        public virtual void SavePlayerName(string playerName)
        {
            if (debugging == true) Debug.Log("Save player name: " + playerName);
            _playerName = playerName;
        }
        #endregion

        #region Player - Team
        public virtual void SetMyTeamName(string teamName = "")
        {
            if (debugging == true) Debug.Log("Setting My Team Name...");
            NetworkManager.networkManager.teamName = teamName;
            if (!string.IsNullOrEmpty(teamName))
            {
                AddToTeamData(PhotonNetwork.LocalPlayer.UserId, teamName);
                object[] data = new object[] { PhotonNetwork.LocalPlayer.UserId, teamName };
                RaiseEventOptions options = new RaiseEventOptions
                {
                    CachingOption = EventCaching.AddToRoomCache,
                    Receivers = ReceiverGroup.Others
                };
                if (debugging == true) Debug.Log("Raising CB_EVENT_TEAMCHANGE photon event...");
                PhotonNetwork.RaiseEvent(PhotonEventCodes.CB_EVENT_TEAMCHANGE, data, options, SendOptions.SendReliable);
            }
        }
        public virtual string GetMyTeamName()
        {
            return NetworkManager.networkManager.teamName;
        }
        public virtual string GetUserTeamName(string userId)
        {
            return (_teamData.ContainsKey(userId)) ? _teamData[userId] : "";
        }
        public virtual Dictionary<string, string> GetTeamData()
        {
            return _teamData;
        }
        public virtual void AddToTeamData(string userId, string teamName)
        {
            if (debugging == true) Debug.Log("Adding: " + userId + " to team: " + teamName);
            if (_teamData.ContainsKey(userId))
            {
                _teamData[userId] = teamName;
            }
            else
            {
                _teamData.Add(userId, teamName);
            }
        }
        public virtual void ClearTeamData()
        {
            if (debugging == true) Debug.Log("Clear saved team Data");
            _teamData.Clear();
        }
        #endregion

        #region Player - Ready
        public virtual void SendReadyState(bool isReady)
        {
            if (debugging == true) Debug.Log("Sending ready state: " + isReady);
            object[] data = new object[] { isReady, PhotonNetwork.LocalPlayer.UserId };
            RaiseEventOptions options = new RaiseEventOptions
            {
                CachingOption = EventCaching.AddToRoomCache,
                Receivers = ReceiverGroup.All
            };
            if (debugging == true) Debug.Log("Raising Photon event: CB_EVENT_READYUP");
            PhotonNetwork.RaiseEvent(PhotonEventCodes.CB_EVENT_READYUP, data, options, SendOptions.SendReliable);
        }
        public virtual void ClearPlayerReadyDict()
        {
            if (debugging == true) Debug.Log("Clearing player ready list.");
            _playersReady.Clear();
        }
        public virtual Dictionary<string, bool> GetPlayersReadyDict()
        {
            return _playersReady;
        }
        public virtual bool PlayerIsReady(string userId)
        {
            return (_playersReady.ContainsKey(userId)) ? _playersReady[userId] : false;
        }
        public virtual bool AllPlayersReady()
        {
            int readyCount = 0;
            foreach (bool player_ready in _playersReady.Values)
            {
                readyCount = (player_ready == true) ? readyCount + 1 : readyCount;
            }
            return readyCount == PhotonNetwork.CurrentRoom.PlayerCount;
        }
        public virtual bool PlayerInReadyDict(string userId)
        {
            return _playersReady.ContainsKey(userId);
        }
        public virtual void AddNewPlayerToPlayerReadyList(Photon.Realtime.Player player)
        {
            if (debugging == true) Debug.Log("Add new player to ready list: " + player.NickName);
            if (_playersReady.ContainsKey(player.UserId))
            {
                _playersReady[player.UserId] = false;
            }
            else
            {
                _playersReady.Add(player.UserId, false);
            }
        }
        public virtual void RemovePlayerToPlayerReadyList(Photon.Realtime.Player player)
        {
            if (debugging == true) Debug.Log("Remove player from ready list: " + player.NickName);
            if (_playersReady.ContainsKey(player.UserId))
            {
                _playersReady.Remove(player.UserId);
            }
        }
        #endregion
        #endregion

        #region Photon Lobby Events
        public virtual void JoinLobby()
        {
            if (debugging == true) Debug.Log("Join default lobby...");
            NetworkManager.networkManager.JoinLobby();
        }
        #endregion

        #region Photon Room Settings
        public virtual void SaveRoomName(string roomName)
        {
            if (debugging == true) Debug.Log("Save room name: " + roomName);
            _roomName = roomName;
        }
        public virtual void JoinRoom(string roomname)
        {
            if (debugging == true) Debug.Log("Join Room: " + roomname);
            NetworkManager.networkManager.JoinRoom(roomname);
            OnWaitToJoinPhotonRoomsLobby.Invoke();
        }
        public virtual void JoinSavedRoomName(bool useSavedPassword)
        {
            if (debugging == true) Debug.Log("Join Saved Room: " + _roomName);
            if (useSavedPassword == true)
            {
                NetworkManager.networkManager.JoinRandomRoom(new ExitGames.Client.Photon.Hashtable {
                    { RoomProperty.RoomName, _roomName },
                    { RoomProperty.Password, _roomPassword },
                    { RoomProperty.RoomType, RoomProperty.PublicRoomType }
                });
            }
            else
            {
                NetworkManager.networkManager.JoinRoom(_roomName);
            }
            OnWaitToJoinPhotonRoomsLobby.Invoke();
        }
        public virtual void JoinPrivateRoom()
        {
            if (debugging == true) Debug.Log("Join Private Room: " + _roomName + " With Password: " + _roomPassword);
            NetworkManager.networkManager.JoinRandomRoom(new ExitGames.Client.Photon.Hashtable {
                { RoomProperty.RoomName, _roomName },
                { RoomProperty.Password, _roomPassword },
                { RoomProperty.RoomType, RoomProperty.PrivateRoomType }
            });
        }
        public virtual void JoinRandomPublicRoomOrCreateOne()
        {
            StartCoroutine(WaitForRandomJoin());
        }
        IEnumerator WaitForRandomJoin()
        {
            if (debugging == true) Debug.Log("Waiting to connect to lobby...");
            yield return new WaitUntil(() => PhotonNetwork.NetworkClientState == ClientState.JoinedLobby);
            if (debugging == true) Debug.Log("Connected To Lobby!");
            yield return new WaitForSeconds(0.05f);
            bool exists = false;
            if (NetworkManager.networkManager.cachedRoomList.Count < 1)
            {
                if (debugging == true) Debug.Log("No games exist...");
                exists = false;
            }
            else
            {
                if (debugging == true) Debug.Log("Checking for existing games...");
                foreach (RoomInfo room in NetworkManager.networkManager.cachedRoomList.Values)
                {
                    foreach (DictionaryEntry item in room.CustomProperties)
                    {
                        Debug.Log(item.Key + " = " + item.Value);
                    }
                    if (room.CustomProperties.ContainsKey(RoomProperty.RoomType))
                    {
                        if ((string)room.CustomProperties[RoomProperty.RoomType] == RoomProperty.PublicRoomType &&
                            room.PlayerCount < NetworkManager.networkManager.maxPlayerPerRoom)
                        {
                            exists = true;
                            break;
                        }
                    }
                }
            }
            if (exists == true)
            {
                if (debugging == true) Debug.Log("Joining a found room...");
                NetworkManager.networkManager.JoinRandomRoom(new ExitGames.Client.Photon.Hashtable {
                    { RoomProperty.RoomType, RoomProperty.PublicRoomType }
                });
            }
            else
            {
                if (debugging == true) Debug.Log("No rooms found, creating a new random room...");
                _roomName = "";
                _roomPassword = "";
                EnableRoomNameChecking(false);
                CreateSessionWithSavedRoomName(false);
            }
        }
        public virtual void SaveRoomPassword(string password)
        {
            _roomPassword = password;
        }
        public virtual void ClearSavedRoomPassword()
        {
            _roomPassword = "";
        }

        public virtual void SetRoomIsJoinable(bool isJoinable)
        {
            if (debugging == true) Debug.Log("Set room joinable state to: " + isJoinable);
            NetworkManager.networkManager.SetRoomIsOpen(isJoinable);
        }
        public virtual void SetRoomVisibility(bool isVisible)
        {
            if (debugging == true) Debug.Log("Set room visibility to: " + isVisible);
            NetworkManager.networkManager.SetRoomVisibility(isVisible);
        }

        public virtual void SendStartCountdown(float amount)
        {
            object[] data = new object[] { true, amount };
            RaiseEventOptions options = new RaiseEventOptions
            {
                CachingOption = EventCaching.AddToRoomCache,
                Receivers = ReceiverGroup.All
            };
            if (debugging == true) Debug.Log("Raising CB_EVENT_STARTCOUNTDOWN photon event...");
            PhotonNetwork.RaiseEvent(PhotonEventCodes.CB_EVENT_STARTCOUNTDOWN, data, options, SendOptions.SendReliable);
        }
        public virtual void SendStopCountdown()
        {
            object[] data = new object[] { false, 0 };
            RaiseEventOptions options = new RaiseEventOptions
            {
                CachingOption = EventCaching.AddToRoomCache,
                Receivers = ReceiverGroup.All
            };
            if (debugging == true) Debug.Log("Raising CB_EVENT_STARTCOUNTDOWN photon event...");
            PhotonNetwork.RaiseEvent(PhotonEventCodes.CB_EVENT_STARTCOUNTDOWN, data, options, SendOptions.SendReliable);
        }

        public virtual void SendSceneVote(int sceneIndex)
        {
            if (_myPrevVote != sceneIndex)
            {
                object[] data = new object[] { _myPrevVote, sceneIndex };
                RaiseEventOptions options = new RaiseEventOptions
                {
                    CachingOption = EventCaching.AddToRoomCache,
                    Receivers = ReceiverGroup.All
                };
                if (debugging == true) Debug.Log("Raising CB_EVENT_SCENEVOTE photon event...");
                PhotonNetwork.RaiseEvent(PhotonEventCodes.CB_EVENT_SCENEVOTE, data, options, SendOptions.SendReliable);

                _myPrevVote = sceneIndex;
            }
        }
        public virtual int GetSceneVotes(int sceneIndex)
        {
            return (_sceneVotes.ContainsKey(sceneIndex)) ? _sceneVotes[sceneIndex] : 0;
        }
        public virtual void ClearSceneVoteList()
        {
            _maxVote = 0;
            _myPrevVote = -1;
            _sceneVotes.Clear();
        }

        public virtual void CreateRandomSceneList()
        {
            _randomSceneList.Clear();
            List<SceneOption> temp = new List<SceneOption>();
            temp.AddRange(sceneList);
            foreach (SceneOption option in sceneList)
            {
                SceneOption item = temp[UnityEngine.Random.Range(0, temp.Count)];
                _randomSceneList.Add(item);
                temp.Remove(item);
            }
        }
        public virtual void SetRandomSceneList(List<SceneOption> randomRoomList)
        {
            _randomSceneList = randomRoomList;
        }
        public virtual void SendCreatedRandomSceneList()
        {
            RoomListWrapper container = new RoomListWrapper();
            container.wrapper = _randomSceneList;
            object[] data = new object[] { JsonUtility.ToJson(container) };
            RaiseEventOptions options = new RaiseEventOptions
            {
                CachingOption = EventCaching.AddToRoomCache,
                Receivers = ReceiverGroup.All
            };
            if (debugging == true) Debug.Log("Raising CB_EVENT_RANDOMSCENELIST photon event...");
            PhotonNetwork.RaiseEvent(PhotonEventCodes.CB_EVENT_RANDOMSCENELIST, data, options, SendOptions.SendReliable);
        }
        public virtual SceneOption GetRandomSceneNumber(int value)
        {
            if (_randomSceneList.Count <= value)
            {
                return new SceneOption("", null);
            }
            else
            {
                return _randomSceneList[value];
            }
        }
        public virtual SceneOption GetSceneNumber(int value)
        {
            return sceneList[value];
        }

        public virtual void SaveRoomList()
        {
            if (debugging == true) Debug.Log("Save Room List: " + NetworkManager.networkManager.cachedRoomList.Count);
            _rooms = NetworkManager.networkManager.cachedRoomList;
        }
        public virtual Dictionary<string, RoomInfo> GetRoomList()
        {
            SaveRoomList();
            return _rooms;
        }
        public virtual void EnableRoomNameChecking(bool isEnabled)
        {
            _roomNameChecking = isEnabled;
        }
        public virtual void CreateSessionWithSavedRoomName(bool useSavedPassword = false)
        {
            if (debugging == true) Debug.Log("Create Session With Saved Room Name: " + _roomName + "...");
            bool canCreate = true;
            if (_roomNameChecking == true)
            {
                if (string.IsNullOrEmpty(_roomName))
                {
                    OnCreateRoomFailed.Invoke("You must specify a room name to create a session.");
                    canCreate = false;
                }
                else if (_roomName.Contains("_"))
                {
                    OnCreateRoomFailed.Invoke("You cannot create a room with the '_' symbol in it.");
                    canCreate = false;
                }
                else if (_roomName.Contains(":"))
                {
                    OnCreateRoomFailed.Invoke("You cannot create a room with the ':' symbol in it.");
                    canCreate = false;
                }
            }
            if (PhotonNetwork.InLobby && canCreate == true)
            {
                foreach (KeyValuePair<string, RoomInfo> room in NetworkManager.networkManager.cachedRoomList)
                {
                    if (Regex.Replace(room.Key, "_.*", "") == _roomName)
                    {
                        OnCreateRoomFailed.Invoke("A session with that name already exists, choose another.");
                        return;
                    }
                }
                if (useSavedPassword == true)
                {
                    NetworkManager.networkManager.CreateRoom(
                        _roomName,
                        customRoomProperties: new ExitGames.Client.Photon.Hashtable {
                            { RoomProperty.Password, _roomPassword },
                            { RoomProperty.RoomName, _roomName },
                            { RoomProperty.RoomType, RoomProperty.PublicRoomType }
                        },
                        exposePropertiesToLobby: new string[]
                        {
                            RoomProperty.Password,
                            RoomProperty.RoomName,
                            RoomProperty.RoomType
                        }
                    );
                }
                else
                {
                    NetworkManager.networkManager.CreateRoom(
                        _roomName,
                        customRoomProperties: new ExitGames.Client.Photon.Hashtable {
                            { RoomProperty.Password, _roomPassword },
                            { RoomProperty.RoomName, _roomName },
                            { RoomProperty.RoomType, RoomProperty.PublicRoomType }
                        },
                        exposePropertiesToLobby: new string[]
                        {
                            RoomProperty.Password,
                            RoomProperty.RoomName,
                            RoomProperty.RoomType
                        }
                     );
                }
                OnCreateRoomSuccess.Invoke();
            }
            else if (canCreate == true)
            {
                if (useSavedPassword == true)
                {
                    NetworkManager.networkManager.CreateRoom(
                        _roomName,
                        customRoomProperties: new ExitGames.Client.Photon.Hashtable {
                            { RoomProperty.Password, _roomPassword },
                            { RoomProperty.RoomName, _roomName },
                            { RoomProperty.RoomType, RoomProperty.PublicRoomType }
                        },
                        exposePropertiesToLobby: new string[] {
                            RoomProperty.Password,
                            RoomProperty.RoomName,
                            RoomProperty.RoomType
                        }
                    );
                }
                else
                {
                    NetworkManager.networkManager.CreateRoom(
                        _roomName,
                        customRoomProperties: new ExitGames.Client.Photon.Hashtable {
                            { RoomProperty.Password, _roomPassword },
                            { RoomProperty.RoomName, _roomName },
                            { RoomProperty.RoomType, RoomProperty.PublicRoomType }
                        },
                        exposePropertiesToLobby: new string[] {
                            RoomProperty.Password,
                            RoomProperty.RoomName,
                            RoomProperty.RoomType
                        }
                     );
                }
                OnCreateRoomSuccess.Invoke();
            }
        }
        public virtual void CreatePrivateSession()
        {
            if (debugging == true) Debug.Log("Create Session With Saved Room Name: " + _roomName + "...");
            if (string.IsNullOrEmpty(_roomName))
            {
                OnCreateRoomFailed.Invoke("You must specify a room name to create a session.");
            }
            else if (_roomName.Contains("_"))
            {
                OnCreateRoomFailed.Invoke("You cannot create a room with the '_' symbol in it.");
            }
            else if (_roomName.Contains(":"))
            {
                OnCreateRoomFailed.Invoke("You cannot create a room with the ':' symbol in it.");
            }
            NetworkManager.networkManager.CreateRoom(
                _roomName,
                customRoomProperties: new ExitGames.Client.Photon.Hashtable {
                    { RoomProperty.Password, _roomPassword },
                    { RoomProperty.RoomName, _roomName },
                    { RoomProperty.RoomType, RoomProperty.PrivateRoomType }
                },
                exposePropertiesToLobby: new string[]
                {
                    RoomProperty.Password,
                    RoomProperty.RoomName,
                    RoomProperty.RoomType
                }
            );
            OnCreateRoomSuccess.Invoke();
        }

        public virtual void SendRoundTime(float roundTime)
        {
            object[] data = new object[] { roundTime };
            RaiseEventOptions options = new RaiseEventOptions
            {
                CachingOption = EventCaching.AddToRoomCache,
                Receivers = ReceiverGroup.All
            };
            if (debugging == true) Debug.Log("Raising CB_EVENT_ROUNDTIME photon event...");
            PhotonNetwork.RaiseEvent(PhotonEventCodes.CB_EVENT_ROUNDTIME, data, options, SendOptions.SendReliable);
        }
        #endregion

        #region Generic Network Events
        public virtual void Disconnect()
        {
            ResetEverything();
            if (debugging == true) Debug.Log("Disconnect from Photon...");
            NetworkManager.networkManager.Disconnect();
        }
        public virtual void NetworkErrorOccured(string errorMessage)
        {
            if (debugging == true) Debug.Log("Recieved Network Error: " + errorMessage);
            OnNetworkError.Invoke(errorMessage);
        }
        #endregion

        #region Photon Events
        protected virtual void RecievedPhotonEvent(EventData obj)
        {
            try
            {
                if (debugging == true) Debug.Log("Receiving Photon Event: " + obj.Code + " ...");
                if (obj.CustomData == null || obj.CustomData.GetType() != typeof(object[])) return;
                object[] data = (object[])obj.CustomData;
                switch (obj.Code)
                {
                    case PhotonEventCodes.CB_EVENT_READYUP:
                        PhotonEvent_READUP(data);
                        break;
                    case PhotonEventCodes.CB_EVENT_TEAMCHANGE:
                        PhotonEvent_TEAMCHANGE(data);
                        break;
                    case PhotonEventCodes.CB_EVENT_STARTSESSION:
                        PhotonEvent_STARTSESSION(data);
                        break;
                    case PhotonEventCodes.CB_EVENT_AUTOSPAWN:
                        PhotonEvent_AUTOSPAWN(data);
                        break;
                    case PhotonEventCodes.CB_EVENT_KICKPLAYER:
                        PhotonEvent_KICKPLAYER(data);
                        break;
                    case PhotonEventCodes.CB_EVENT_MAPCHANGE:
                        PhotonEvent_MAPCHANGE(data);
                        break;
                    case PhotonEventCodes.CB_EVENT_RANDOMSCENELIST:
                        PhotonEvent_RANDOMSCENELIST(data);
                        break;
                    case PhotonEventCodes.CB_EVENT_SCENEVOTE:
                        PhotonEvent_SCENEVOTE(data);
                        break;
                    case PhotonEventCodes.CB_EVENT_STARTCOUNTDOWN:
                        PhotonEvent_STARTCOUNTDOWN(data);
                        break;
                    case PhotonEventCodes.CB_EVENT_ROUNDTIME:
                        PhotonEvent_ROUNDTIME(data);
                        break;
                    case PhotonEventCodes.CB_EVENT_VOICEVIEW:
                        PhotonEvent_VOICEVIEW(data);
                        break;
                }
            }
            catch (Exception ex)
            {
                if (debugging == true) Debug.Log(ex);
            }
        }
        protected virtual void PhotonEvent_READUP(object[] data)
        {
            if (debugging == true) Debug.Log("Received: CB_EVENT_READYUP");
            bool isReady = (bool)data[0];
            string userId = (string)data[1];
            if (debugging == true) Debug.Log("READY: " + isReady + ", USERID: " + userId);
            if (_playersReady.ContainsKey(userId))
            {
                _playersReady[userId] = isReady;
            }
            else
            {
                _playersReady.Add(userId, isReady);
            }
        }
        protected virtual void PhotonEvent_TEAMCHANGE(object[] data)
        {
            if (debugging == true) Debug.Log("Received: CB_EVENT_TEAMCHANGE");
            string userId = (string)data[0];
            string teamName = (string)data[1];
            if (debugging == true) Debug.Log("USERID: " + userId + ", TEAMNAME: " + teamName);
            AddToTeamData(userId, teamName);
            if (teamsUpdated != null)
            {
                teamsUpdated.Invoke();
            }
        }
        protected virtual void PhotonEvent_STARTSESSION(object[] data)
        {
            if (debugging == true) Debug.Log("Received: CB_EVENT_STARTMATCH");
            PhotonNetwork.NetworkingClient.EventReceived -= RecievedPhotonEvent;
            bool starting = (bool)data[0];
            if (starting == true)
            {
                OnStartSession.Invoke();
            }
        }
        protected virtual void PhotonEvent_AUTOSPAWN(object[] data)
        {
            if (debugging == true) Debug.Log("Received: CB_EVENT_AUTOSPAWN");
            bool spawnEnabled = (bool)data[0];
            if (debugging == true) Debug.Log("AUTO_SPAWN_ENABLED: " + spawnEnabled);
            SendEnableAutoSpawn(spawnEnabled);
        }
        protected virtual void PhotonEvent_KICKPLAYER(object[] data)
        {
            NetworkErrorOccured("You have been kicked from the room");
        }
        protected virtual void PhotonEvent_MAPCHANGE(object[] data)
        {
            _savedLevelName = (string)data[0];
            _savedLevelIndex = (int)data[1];
        }
        protected virtual void PhotonEvent_RANDOMSCENELIST(object[] data)
        {
            RoomListWrapper container = JsonUtility.FromJson<RoomListWrapper>((string)data[0]);
            foreach (SceneOption option in container.wrapper)
            {
                option.sceneSprite = sceneList.Find(x => x.sceneName == option.sceneName).sceneSprite;
            }
            _randomSceneList = container.wrapper;
        }
        protected virtual void PhotonEvent_SCENEVOTE(object[] data)
        {
            int prevScene = (int)data[0];
            int currentScene = (int)data[1];
            if (prevScene != -1)
            {
                _sceneVotes[prevScene] -= 1;
            }
            if (_sceneVotes.ContainsKey(currentScene))
            {
                _sceneVotes[currentScene] += 1;
            }
            else
            {
                _sceneVotes.Add(currentScene, 1);
            }
            if (PhotonNetwork.IsMasterClient == true)
            {
                string winning_level_name = _savedLevelName;
                int max_vote = 0;
                List<DatabaseScene> sceneData = NetworkManager.networkManager.database.storedScenesData;
                foreach (KeyValuePair<int, int> item in _sceneVotes)
                {
                    if (item.Value > _maxVote)
                    {
                        winning_level_name = sceneData.Find(x => x.index == item.Key).sceneName;
                        max_vote = item.Value;
                    }
                }
                if (winning_level_name != _savedLevelName)
                {
                    _savedLevelIndex = sceneData.Find(x => x.sceneName == winning_level_name).index;
                    SendSceneChangeInfo(_savedLevelName, _savedLevelIndex);
                }
            }
        }
        protected virtual void PhotonEvent_STARTCOUNTDOWN(object[] data)
        {
            bool start_countdown = (bool)data[0];
            float countdown_amount = (float)data[1];
            if (start_countdown)
            {
                OnCountdownStarted.Invoke(countdown_amount);
            }
            else
            {
                OnCountdownStopped.Invoke();
            }
        }
        protected virtual void PhotonEvent_ROUNDTIME(object[] data)
        {
            float round_time = (float)data[0];
            if (debugging == true) Debug.Log("Received Round Time Update: " + round_time);
            OnReceiveRoundTime.Invoke(round_time);
        }
        protected virtual void PhotonEvent_VOICEVIEW(object[] data)
        {
            string UserId = (string)data[0];
            int ViewId = (int)data[1];
            if (debugging == true) Debug.Log("Recieved new VOICEVIEW, UserId: " + UserId + ", ViewID: " + ViewId);
            if (_playerVoiceChatViews.ContainsKey(UserId))
            {
                if (debugging == true) Debug.Log("Updating, UserId: " + UserId + ", To ViewID: " + ViewId);
                _playerVoiceChatViews[UserId] = ViewId;
            }
            else
            {
                if (debugging == true) Debug.Log("Adding new UserId: " + UserId + ", With ViewID: " + ViewId);
                _playerVoiceChatViews.Add(UserId, ViewId);
            }
            if (voiceViewUpdated != null) voiceViewUpdated.Invoke();
        }
        #endregion

        #region Session Events
        public virtual void SendStartSession()
        {
            if (debugging == true) Debug.Log("Sending start session...");
            object[] data = new object[] { true };
            RaiseEventOptions options = new RaiseEventOptions
            {
                CachingOption = EventCaching.AddToRoomCacheGlobal,
                Receivers = ReceiverGroup.All
            };
            if (debugging == true) Debug.Log("Raising Photon Event: CB_EVENT_STARTMATCH");
            PhotonNetwork.RaiseEvent(PhotonEventCodes.CB_EVENT_STARTSESSION, data, options, SendOptions.SendReliable);
        }
        public virtual void SendEnableAutoSpawn(bool enableSpawn)
        {
            if (debugging == true) Debug.Log("Sending Enable Auto Spawn: " + enableSpawn);
            object[] data = new object[] { enableSpawn };
            RaiseEventOptions options = new RaiseEventOptions
            {
                CachingOption = EventCaching.AddToRoomCache,
                Receivers = ReceiverGroup.All
            };
            if (debugging == true) Debug.Log("Raising Photon Event: CB_EVENT_AUTOSPAWN");
            PhotonNetwork.RaiseEvent(PhotonEventCodes.CB_EVENT_AUTOSPAWN, data, options, SendOptions.SendReliable);
        }
        public virtual string GetSavedSceneToLoadName()
        {
            return _savedLevelName;
        }
        protected virtual void SendSceneChangeInfo(string sceneName, int index)
        {
            object[] data = new object[] { sceneName, index };
            RaiseEventOptions options = new RaiseEventOptions
            {
                CachingOption = EventCaching.AddToRoomCache,
                Receivers = ReceiverGroup.Others
            };
            if (debugging == true) Debug.Log("Raising CB_EVENT_MAPCHANGE photon event...");
            PhotonNetwork.RaiseEvent(PhotonEventCodes.CB_EVENT_MAPCHANGE, data, options, SendOptions.SendReliable);
        }
        public virtual void SaveSceneToLoad(string levelName)
        {
            if (debugging == true) Debug.Log("Save Scene To Load: " + levelName);
            _savedLevelName = levelName;
            List<DatabaseScene> sceneData = NetworkManager.networkManager.database.storedScenesData;
            _savedLevelIndex = sceneData.Find(x => x.sceneName == _savedLevelName).index;
            SendSceneChangeInfo(_savedLevelName, _savedLevelIndex);
        }
        public virtual void SaveSceneToLoad(int levelIndex)
        {
            if (debugging == true) Debug.Log("Save Scene To Load Index: " + levelIndex);
            _savedLevelIndex = levelIndex;
            List<DatabaseScene> sceneData = NetworkManager.networkManager.database.storedScenesData;
            _savedLevelName = sceneData.Find(x => x.index == _savedLevelIndex).sceneName;
            SendSceneChangeInfo(_savedLevelName, _savedLevelIndex);
        }
        public virtual void LoadSavedLevel()
        {
            if (debugging == true) Debug.Log("Load the saved level: " + _savedLevelName);
            List<DatabaseScene> sceneData = NetworkManager.networkManager.database.storedScenesData;
            if (!string.IsNullOrEmpty(_savedLevelName))
            {
                NetworkManager.networkManager.NetworkLoadLevel(sceneData.Find(x => x.sceneName == _savedLevelName).index);
            }
            else
            {
                NetworkManager.networkManager.NetworkLoadLevel(_savedLevelIndex, sendEveryone: false);
            }
        }
        public virtual void EveryoneLoadSavedLevel()
        {
            if (debugging == true) Debug.Log("Every load the saved level: " + _savedLevelName);
            List<DatabaseScene> sceneData = NetworkManager.networkManager.database.storedScenesData;
            if (!string.IsNullOrEmpty(_savedLevelName))
            {
                NetworkManager.networkManager.NetworkLoadLevel(sceneData.Find(x => x.sceneName == _savedLevelName).index);
            }
            else
            {
                NetworkManager.networkManager.NetworkLoadLevel(_savedLevelIndex);
            }
        }
        public virtual void EveryoneLoadLevel(string levelName)
        {
            if (debugging == true) Debug.Log("Every load level: " + levelName);
            OnStartSession.Invoke();
            List<DatabaseScene> sceneData = NetworkManager.networkManager.database.storedScenesData;
            NetworkManager.networkManager.NetworkLoadLevel(sceneData.Find(x => x.sceneName == levelName).index);
        }
        public virtual void EveryoneLoadLevel(int levelIndex)
        {
            if (debugging == true) Debug.Log("Every load level index: " + levelIndex);
            OnStartSession.Invoke();
            NetworkManager.networkManager.NetworkLoadLevel(levelIndex);
        }
        public virtual void EnablePlayerAutoSpawn(bool setActive)
        {
            if (debugging == true) Debug.Log("Enable Player Auto Spawn: " + setActive);
            NetworkManager.networkManager.autoSpawnPlayer = setActive;
        }
        #endregion

        #region Loading Pages
        public virtual void EnableLoadingPage(bool isEnabled)
        {
            loadingParent.SetActive(isEnabled);
            if (isEnabled == true)
            {
                if (debugging == true) Debug.Log("Start Level Loading...");
                ResetLoadingBar();

                _cycleLoadingPage = (loadingImages.Count > 1 || loadingDesc.Count > 1) ? true : false;
                _selectedLoadImageIndex = 0;
                _selectedDescTextIndex = 0;

                _tempLoadColor = mainLoadingImage.color;
                _tempLoadTextColor = loadingDescText.color;
                _tempLoadColor.a = 1;
                _tempLoadTextColor.a = 1;
                mainLoadingImage.color = _tempLoadColor;
                loadingDescText.color = _tempLoadTextColor;

                mainLoadingImage.sprite = (loadingImages.Count > 0) ? loadingImages[0] : mainLoadingImage.sprite;
                loadingTitleText.text = loadingTitle;
                loadingDescText.text = (loadingDesc.Count > 0) ? loadingDesc[0] : loadingDescText.text;

                mainLoadingImage.gameObject.SetActive(true);
                loadingTitleText.gameObject.SetActive(true);
                loadingDescText.gameObject.SetActive(true);
                StartCoroutine(WaitForSceneToStartLoading());
                OnStartLoading.Invoke();
            }
            else
            {
                if (debugging == true) Debug.Log("End Level Loading.");
                mainLoadingImage.gameObject.SetActive(false);
                loadingTitleText.gameObject.SetActive(false);
                loadingDescText.gameObject.SetActive(false);
                _trackLoadingBar = false;
                _cycleLoadingPage = false;
            }
        }
        IEnumerator WaitForSceneToStartLoading()
        {
            yield return new WaitUntil(() => PhotonNetwork.LevelLoadingProgress < 1);
            _trackLoadingBar = true;
        }
        public virtual void ResetLoadingBar()
        {
            if (debugging == true) Debug.Log("Reset Loading Bar.");
            loadingBar.fillAmount = 0;
        }
        #endregion

        #region Application Options
        public virtual void QuitGame()
        {
            Application.Quit();
        }
        #endregion

        #region Chatbox
        public virtual void EnableChatboxSlideOut(bool isEnabled)
        {
            if (debugging == true) Debug.Log("Enable ChatBox GameObject: " + isEnabled);
            ChatBox chatbox = FindObjectOfType<ChatBox>();
            if (chatbox)
            {
                chatbox.EnableChat(isEnabled);
            }
        }
        public virtual void EnableChatBoxVisibility(bool isEnabled)
        {
            if (debugging == true) Debug.Log("Enable Visual ChatBox: " + isEnabled);
            ChatBox chatbox = FindObjectOfType<ChatBox>();
            if (chatbox)
            {
                chatbox.EnableVisualBox(isEnabled);
            }
        }
        #endregion

        #region Invector Sources
        public virtual void EnableSavedPlayerUI(bool isEnabled)
        {
            if (debugging == true) Debug.Log("Enable Saved Player UI: " + isEnabled);
            foreach (GameObject ui in _playerUIs)
            {
                ui.SetActive(isEnabled);
            }
        }
        public virtual void SavePlayerUIs()
        {
            if (debugging == true) Debug.Log("Save Player UIs");
            GameObject[] invectorUI = GameObject.FindGameObjectsWithTag("PlayerUI");
            _playerUIs.AddRange(invectorUI);
        }
        public virtual void ClearPlayerUIs()
        {
            if (debugging == true) Debug.Log("Remove all saved player uis.");
            _playerUIs.Clear();
        }
        public virtual void RefreshPlayerUIs()
        {
            ClearPlayerUIs();
            SavePlayerUIs();
        }
        #endregion

        #region Mouse/Keyboard Settings
        public virtual void EnableMouseMovement(bool isEnabled)
        {
            Cursor.lockState = (isEnabled == true) ? CursorLockMode.None : CursorLockMode.Locked;
        }
        public virtual void EnableMouseVisibility(bool isVisible)
        {
            Cursor.visible = isVisible;
        }
        public virtual void MouseSelect(GameObject target)
        {
            StartCoroutine(MouseSelectHandle(target));
        }
        IEnumerator MouseSelectHandle(GameObject target)
        {
            if (this.enabled)
            {
                yield return new WaitForEndOfFrame();
                MouseSelectTarget(target);
            }
        }
        protected virtual void MouseSelectTarget(GameObject target)
        {
            var pointer = new PointerEventData(EventSystem.current);
            ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, pointer, ExecuteEvents.pointerExitHandler);
            EventSystem.current.SetSelectedGameObject(target, new BaseEventData(EventSystem.current));
            ExecuteEvents.Execute(target, pointer, ExecuteEvents.selectHandler);
        }
        #endregion

        #region Scenes
        protected virtual void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            OnSceneLoaded.Invoke(scene);
        }
        #endregion

        #region Voice Chat
        public virtual int GetPlayerVoiceView(string UserId)
        {
            return (_playerVoiceChatViews.ContainsKey(UserId)) ? _playerVoiceChatViews[UserId] : 999999;
        }
        public virtual void SendUpdateVoiceView(string UserId, int ViewId)
        {
            object[] data = new object[] { UserId, ViewId };
            RaiseEventOptions options = new RaiseEventOptions
            {
                CachingOption = EventCaching.AddToRoomCacheGlobal,
                Receivers = ReceiverGroup.All
            };
            if (debugging == true) Debug.Log("Raising CB_EVENT_VOICEVIEW photon event with data { " + UserId + ", " + ViewId + " }...");
            PhotonNetwork.RaiseEvent(PhotonEventCodes.CB_EVENT_VOICEVIEW, data, options, SendOptions.SendReliable);
        }
        #endregion
    }
}