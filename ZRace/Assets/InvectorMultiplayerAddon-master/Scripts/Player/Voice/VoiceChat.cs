using CBGames.Core;
using Invector.vCharacterController;
using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CBGames.Player
{
    public class VoiceChat : MonoBehaviourPunCallbacks
    {
        // For Players
        // * PhotonVoiceView
        // * Speaker

        // For UI
        // * Recorder
        // * Photon Voice Network

        #region Modifiables
        [Tooltip("This component is for the player or not.")]
        [SerializeField] private bool isPlayer = false;
        [Tooltip("Will only enable the voice chat indicator if you're on the target team. " +
            "If left blank will always enable when talking across the network, no matter " +
            "what team your on.")]
        public string ifOnTeam = "";
        [Tooltip("The image to show when voice is being played through the speaker.")]
        public GameObject speakerImage = null;
        [Tooltip("The image to show when you are recording your voice")]
        public GameObject recordingImage = null;
        [Tooltip("Verbose console logging to help you while you debug.")]
        [SerializeField] private bool debugging = false;
        
        //For None Player only
        [Tooltip("Automatically record voice when speaking or require a button to be pushed.")]
        [SerializeField] private bool pushToTalk = false;
        [Tooltip("Button press to enable voice transmittion.")]
        [SerializeField] public string buttonToPress;

        [SerializeField] protected UnityEvent OnConnectedToServer = null;
        [SerializeField] protected UnityEvent OnDisconnect = null;
        [SerializeField] protected UnityEvent OnJoinRoom = null;
        [SerializeField] protected UnityEvent OnStart = null;
        #endregion

        [HideInInspector] public string connectionStatus = "";

        #region Internal Only Variables
        protected float originalAudioVolume = 0;
        protected byte[] registeredGroups;
        protected PhotonVoiceView view = null;
        protected Recorder recorder = null;
        protected Speaker speaker = null;
        protected bool record = false;
        protected bool connectedToRoom = false;
        #endregion
        public virtual void Awake()
        {
            if (recordingImage != null) recordingImage.SetActive(false);
            if (speakerImage != null) speakerImage.SetActive(false);
        }
        public virtual void Start()
        {
            if (!NetworkManager.networkManager)
            {
                Debug.LogError("VoiceChat - Disabling myself because I was unable to locate the Network Manager. Is it in the scene?");
                enabled = false;
                return;
            }
            if (isPlayer == true)
            {
                if (!GetComponent<PhotonVoiceView>() && isPlayer == true)
                {
                    Debug.LogError("This component is marked as a player but doesn't have a PhotonVoiceView component attached!");
                }
                view = GetComponent<PhotonVoiceView>();
                view.RecorderInUse = (view.RecorderInUse == null) ? NetworkManager.networkManager.voiceRecorder : view.RecorderInUse;
                view.UsePrimaryRecorder = true;
                view.SetupDebugSpeaker = debugging;
                view.SpeakerInUse = (view.SpeakerInUse == null) ? GetComponent<Speaker>() : view.SpeakerInUse;
                speaker = GetComponent<Speaker>();
            }
            else
            {
                if (!GetComponent<Recorder>() && isPlayer == false)
                {
                    Debug.LogError("This component is marked as not a player but doesn't have a Recorder component attached!");
                }
                if (!GetComponent<PhotonVoiceNetwork>() && isPlayer == false)
                {
                    Debug.LogError("This component is marked as not a player but doesn't have a PhotonVoiceNetwork component attached!");
                }
                originalAudioVolume = AudioListener.volume;
                recorder = GetComponent<Recorder>();
                recorder.TransmitEnabled = !pushToTalk;
                recorder.IsRecording = !pushToTalk;
                GetComponent<Recorder>().DebugEchoMode = debugging;
                PhotonVoiceNetwork.Instance.Client.StateChanged += VoiceClientStateChanged;
            }
            OnStart.Invoke();
        }
        protected virtual void Update()
        {
            if (isPlayer == true && speakerImage != null && (string.IsNullOrEmpty(ifOnTeam) || ifOnTeam == NetworkManager.networkManager.teamName))
            {
                speaker = (speaker == null) ? GetComponent<Speaker>() : speaker;
                speakerImage.SetActive(speaker.IsPlaying);
            }
            else if (connectedToRoom == true && pushToTalk == true)
            {
                if (Input.GetButton(buttonToPress))
                {
                    record = true;
                    StartRecording(true);
                }
                else if (record == true)
                {
                    record = false;
                    StartRecording(false);
                    if (recordingImage != null)
                    {
                        recordingImage.SetActive(false);
                    }
                }
            }
            else if (connectedToRoom == true && pushToTalk == false && record == false)
            {
                record = true;
                StartRecording(true);
                //if (recorder.LevelMeter.CurrentPeakAmp >= recorder.VoiceDetector.Threshold)
                //{
                //    record = true;
                //    StartRecording(true);
                //}
                //else if (record == true)
                //{
                //    record = false;
                //    StartRecording(false);
                //    if (recordingImage != null)
                //    {
                //        recordingImage.SetActive(false);
                //    }
                //}
            }
            if (record == true && recordingImage != null)
            {
                recordingImage.SetActive(recorder.LevelMeter.CurrentPeakAmp >= recorder.VoiceDetector.Threshold);
            }
        }
        
        #region Instance Options
        public virtual void ConnectToVoiceServer()
        {
            if (debugging == true) Debug.Log("Connecting to master server...");
            connectionStatus = "Connecting to master server...";
            PhotonVoiceNetwork.Instance.ConnectUsingSettings(PhotonVoiceNetwork.Instance.Settings);
        }
        public virtual void DisconnectFromVoiceServer()
        {
            if (debugging == true) Debug.Log("Called disconnect...");
            PhotonVoiceNetwork.Instance.Disconnect();
        }
        public virtual void SetSpeakerPrefab(GameObject target)
        {
            if (debugging == true) Debug.Log("Setting speaker");
            PhotonVoiceNetwork.Instance.SpeakerPrefab = target;
        }
        #endregion

        #region Recorder Options
        //PhotonVoiceNetwork.Instance.PrimaryRecorder.InterestGroup
        public virtual void InitializeRecorder()
        {
            if (PhotonVoiceNetwork.Instance.PrimaryRecorder.IsInitialized == false)
            {
                if (debugging == true) Debug.Log("Initialized recorder.");
                PhotonVoiceNetwork.Instance.PrimaryRecorder.Init(PhotonVoiceNetwork.Instance);
            }
        }

        public virtual bool RecorderIsRecording()
        {
            return PhotonVoiceNetwork.Instance.PrimaryRecorder.IsRecording;
        }
        public virtual bool RecorderIsTransmitting()
        {
            return PhotonVoiceNetwork.Instance.PrimaryRecorder.IsCurrentlyTransmitting;
        }

        public virtual void StartRecording(bool enabled)
        {
            if (enabled)
            {
                if (PhotonVoiceNetwork.Instance.PrimaryRecorder.IsInitialized == false)
                {
                    InitializeRecorder();
                }
                if (PhotonVoiceNetwork.Instance.PrimaryRecorder.IsRecording == false)
                {
                    if (debugging == true) Debug.Log("Started recording...");
                    PhotonVoiceNetwork.Instance.PrimaryRecorder.StartRecording();
                }
            }
            else
            {
                if (debugging == true) Debug.Log("Stopped recording.");
                PhotonVoiceNetwork.Instance.PrimaryRecorder.StopRecording();
            }
            recorder.TransmitEnabled = enabled;
        }

        public virtual void EnablePushToTalk(bool enabled)
        {
            StartRecording(false);
            pushToTalk = enabled;
        }
        public virtual void EnableLoopAudioPlayback(bool enabled)
        {
            if (debugging == true) Debug.Log("Enable looping playback: " + enabled);
            PhotonVoiceNetwork.Instance.PrimaryRecorder.LoopAudioClip = enabled;
        }
        public virtual void EnableRecorderReliableMode(bool enabled)
        {
            if (debugging == true) Debug.Log("Enabled reliable mode: " + enabled);
            PhotonVoiceNetwork.Instance.PrimaryRecorder.ReliableMode = enabled;
        }
        public virtual void EnableTransmitVoice(bool enabled)
        {
            if (PhotonVoiceNetwork.Instance.PrimaryRecorder.IsInitialized == false)
            {
                InitializeRecorder();
            }
            if (debugging == true) Debug.Log("Enable transmite voice: " + enabled);
            PhotonVoiceNetwork.Instance.PrimaryRecorder.TransmitEnabled = enabled;
        }
        public virtual void EnableVoiceDetection(bool enabled)
        {
            if (PhotonVoiceNetwork.Instance.PrimaryRecorder.IsInitialized == false)
            {
                InitializeRecorder();
            }
            if (debugging == true) Debug.Log("Enable voice detection: " + enabled);
            PhotonVoiceNetwork.Instance.PrimaryRecorder.VoiceDetection = enabled;
        }
        public virtual void EnableVoiceEcho(bool enabled)
        {
            if (PhotonVoiceNetwork.Instance.PrimaryRecorder.IsInitialized == false)
            {
                InitializeRecorder();
            }
            if (debugging == true) Debug.Log("Enable voice echo: " + enabled);
            PhotonVoiceNetwork.Instance.PrimaryRecorder.DebugEchoMode = enabled;
        }
        public virtual void EnableMuteSpeaker(bool enabled)
        {
            if (debugging == true) Debug.Log("Mute speaker: " + enabled);
            AudioListener.volume = (enabled == true) ? 0 : originalAudioVolume;
        }
        public virtual void EnableEncryptedAudioStream(bool enabled)
        {
            if (debugging == true) Debug.Log("Enable encrypted auto stream: " + enabled);
            PhotonVoiceNetwork.Instance.PrimaryRecorder.Encrypt = enabled;
        }

        public virtual void CalibrateVoiceDetector(int howLong)
        {
            if (PhotonVoiceNetwork.Instance.PrimaryRecorder.IsInitialized == false)
            {
                InitializeRecorder();
            }
            if (debugging == true) Debug.Log("Enabled voice calibration...");
            PhotonVoiceNetwork.Instance.PrimaryRecorder.VoiceDetectorCalibrate(howLong);
            StartCoroutine(WaitForCalibration());
        }
        protected virtual IEnumerator WaitForCalibration()
        {
            yield return new WaitUntil(() => PhotonVoiceNetwork.Instance.PrimaryRecorder.VoiceDetectorCalibrating == false);
            if (debugging == true) Debug.Log("Voice calibration stopped.");
        }
        
        public virtual string[] GetAllMicrophoneDevices()
        {
            return Microphone.devices;
        }
        public virtual string GetCurrentMicrophone()
        {
            return recorder.UnityMicrophoneDevice;
        }
        public virtual void SetMicrophoneDevice(string deviceName)
        {
            recorder.UnityMicrophoneDevice = deviceName;
        }
        
        public virtual byte GetIntresetGroup()
        {
            return recorder.InterestGroup;
        }
        public virtual void SetIntrestGroup(byte group)
        {
            recorder.InterestGroup = group;
        }
        #endregion

        #region Callbacks
        protected virtual void VoiceClientStateChanged(Photon.Realtime.ClientState fromState, Photon.Realtime.ClientState toState)
        {
            switch (toState)
            {
                case Photon.Realtime.ClientState.Authenticating:
                    connectionStatus = "Authenticating with master server...";
                    break;
                case Photon.Realtime.ClientState.Authenticated:
                    connectionStatus = "Successfully authenticated with master server.";
                    break;
                case Photon.Realtime.ClientState.ConnectedToGameServer:
                    connectionStatus = "Connected to game server.";
                    break;
                case Photon.Realtime.ClientState.ConnectedToMasterServer:
                    connectionStatus = "Connected to master server.";
                    foreach(vThirdPersonController controller in FindObjectsOfType<vThirdPersonController>())
                    {
                        if (controller.GetComponent<PhotonView>().IsMine)
                        {
                            if (controller)
                            {
                                SetSpeakerPrefab(controller.gameObject);
                            }
                            else if (GetComponent<Speaker>())
                            {
                                SetSpeakerPrefab(gameObject);
                            }
                            break;
                        }
                    }
                    OnConnectedToServer.Invoke();
                    break;
                case Photon.Realtime.ClientState.ConnectedToNameServer:
                    connectionStatus = "Connected to name server.";
                    break;
                case Photon.Realtime.ClientState.Disconnected:
                    connectionStatus = "Disconnected.";
                    connectedToRoom = false;
                    OnDisconnect.Invoke();
                    break;
                case Photon.Realtime.ClientState.Disconnecting:
                    connectionStatus = "Disconnecting...";
                    break;
                case Photon.Realtime.ClientState.DisconnectingFromGameServer:
                    connectionStatus = "Disconnecting from game server...";
                    break;
                case Photon.Realtime.ClientState.DisconnectingFromMasterServer:
                    connectionStatus = "Disconnecting from master server...";
                    break;
                case Photon.Realtime.ClientState.Joined:
                    connectionStatus = "Successfully joined a room.";
                    connectedToRoom = true;
                    OnJoinRoom.Invoke();
                    break;
                case Photon.Realtime.ClientState.JoinedLobby:
                    connectionStatus = "Succesffully joined the lobby.";
                    break;
                case Photon.Realtime.ClientState.Joining:
                    connectionStatus = "Joining room...";
                    break;
                case Photon.Realtime.ClientState.JoiningLobby:
                    connectionStatus = "Joining lobby...";
                    break;
                case Photon.Realtime.ClientState.Leaving:
                    connectionStatus = "Leaving room...";
                    connectedToRoom = false;
                    break;
                case Photon.Realtime.ClientState.PeerCreated:
                    connectionStatus = "Peer created.";
                    break;
            }
            if (debugging == true) Debug.Log(connectionStatus);
        }
        #endregion
    }
}