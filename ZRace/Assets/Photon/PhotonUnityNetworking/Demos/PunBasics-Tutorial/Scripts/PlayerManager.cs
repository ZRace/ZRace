
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerManager.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Networking Demos
// </copyright>
// <summary>
//  Used in PUN Basics Tutorial to deal with the networked player instance
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------
using UnityEngine;
using UnityEditor;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using System.Text;
using Photon.Pun.UtilityScripts;
//using WebSocketSharp;

namespace Photon.Pun.Demo.PunBasics
{
#pragma warning disable 649

    /// <summary>
    /// Player manager.
    /// Handles fire Input and Beams.
    /// </summary>
    public class PlayerManager : MonoBehaviourPunCallbacks, IInRoomCallbacks
    {
        #region Public Fields

        [Tooltip("The current Health of our player")]
        [HideInInspector] public float Health = 1f;

        [Tooltip("El objeto del jugador local. Usar para saber si el jugador local esta en la escena.")]
        [HideInInspector] public static GameObject LocalPlayerInstance;

        [Tooltip("Array de GameObject. Usala para desativar todos los objetos de los jugadores que no son locales.")]
        [HideInInspector] public GameObject[] componentsToDisable;
        [Tooltip("Array de Scripts. Usala para desativar todos los scripts de los jugadores que no son locales.")]
        [HideInInspector] public Behaviour[] scriptsDisable;
        public GameObject scoreBoard;




        #endregion

        #region Private Fields

        //[Tooltip("The Player's UI GameObject Prefab")]
        //[SerializeField]
        //private GameObject playerUiPrefab;

        //[Tooltip("The Beams GameObject to control")]
        //[SerializeField]
        //private GameObject beams;

        //True, when the user is firing
        bool IsFiring;

        #endregion

        #region MonoBehaviour CallBacks

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        public void Awake()
        {
            //if (this.beams == null)
            //{
            //    Debug.LogError("<Color=Red><b>Missing</b></Color> Beams Reference.", this);
            //}
            //else
            //{
            //    this.beams.SetActive(false);
            //}

            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instanciation when levels are synchronized
            if (photonView.IsMine)
            {
                LocalPlayerInstance = gameObject;
                scoreBoard = GameObject.Find("CanvasPlayersRoom");
                scoreBoard.SetActive(false);
                this.gameObject.layer = 17;
            }
            else
            {
                this.gameObject.layer = 18;
            }

            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(gameObject);
        }
        void Start()
        {


            if (!PhotonNetwork.LocalPlayer.IsLocal)
            {
                DisableComponents();
            }
        }
		public void Update()
		{
            if(photonView.IsMine)
			{
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    scoreBoard.SetActive(true);
                    UpdateScoreBoard();
                }
                if (Input.GetKeyUp(KeyCode.Tab))
                {
                    scoreBoard.SetActive(false);
                }
                if (PhotonNetwork.LocalPlayer.NickName == "")
                {
                    PhotonNetwork.LocalPlayer.NickName = "Player #";
                }
            }
			
		}
        public void UpdateScoreBoard()
		{
            var playerNames = new StringBuilder();

   //         foreach(var player in PhotonNetwork.PlayerList)
			//{
   //             playerNames.Append(player.NickName + "\n");
			//}

            foreach(Player p in PhotonNetwork.PlayerList)
			{
                playerNames.Append(p.NickName + " Score:" + p.GetScore() + "\n");
			}

            string output = playerNames.ToString();
            scoreBoard.GetComponentInChildren<Text>().text = output;
		}
		public void DisableComponents()
        {
            foreach (GameObject go in componentsToDisable)
            {
                go.SetActive(false);
            }

            for (int i = 0; i < scriptsDisable.Length; i++)
            {
                scriptsDisable[i].enabled = false;
            }

        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("<color=#e30008>" + cause + "</color>");
        }
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            Debug.Log("<color=#bde7ff>" + otherPlayer + "</color>" + " has joined the room");
        }
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            Debug.Log("<color=#bde7ff>" + newPlayer + "</color>" + " has left the room");
        }
        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        //      public void Start()
        //      {
        //          CameraWork _cameraWork = gameObject.GetComponent<CameraWork>();

        //          if (_cameraWork != null)
        //          {
        //              if (photonView.IsMine)
        //              {
        //                  _cameraWork.OnStartFollowing();
        //              }
        //          }
        //          else
        //          {
        //              Debug.LogError("<Color=Red><b>Missing</b></Color> CameraWork Component on player Prefab.", this);
        //          }

        //          // Create the UI
        //          if (this.playerUiPrefab != null)
        //          {
        //              GameObject _uiGo = Instantiate(this.playerUiPrefab);
        //              _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        //          }
        //          else
        //          {
        //              Debug.LogWarning("<Color=Red><b>Missing</b></Color> PlayerUiPrefab reference on player Prefab.", this);
        //          }

        //          #if UNITY_5_4_OR_NEWER
        //          // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
        //	UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        //          #endif
        //      }


        //public override void OnDisable()
        //{
        //	// Always call the base to remove callbacks
        //	base.OnDisable ();

        //	#if UNITY_5_4_OR_NEWER
        //	UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        //	#endif
        //}


        //      /// <summary>
        //      /// MonoBehaviour method called on GameObject by Unity on every frame.
        //      /// Process Inputs if local player.
        //      /// Show and hide the beams
        //      /// Watch for end of game, when local player health is 0.
        //      /// </summary>
        //      public void Update()
        //      {
        //          // we only process Inputs and check health if we are the local player
        //          if (photonView.IsMine)
        //          {
        //              this.ProcessInputs();

        //              if (this.Health <= 0f)
        //              {
        //                  GameManager.Instance.LeaveRoom();
        //              }
        //          }

        //          if (this.beams != null && this.IsFiring != this.beams.activeInHierarchy)
        //          {
        //              this.beams.SetActive(this.IsFiring);
        //          }
        //      }

        //      /// <summary>
        //      /// MonoBehaviour method called when the Collider 'other' enters the trigger.
        //      /// Affect Health of the Player if the collider is a beam
        //      /// Note: when jumping and firing at the same, you'll find that the player's own beam intersects with itself
        //      /// One could move the collider further away to prevent this or check if the beam belongs to the player.
        //      /// </summary>
        //      public void OnTriggerEnter(Collider other)
        //      {
        //          if (!photonView.IsMine)
        //          {
        //              return;
        //          }


        //          // We are only interested in Beamers
        //          // we should be using tags but for the sake of distribution, let's simply check by name.
        //          if (!other.name.Contains("Beam"))
        //          {
        //              return;
        //          }

        //          this.Health -= 0.1f;
        //      }

        //      /// <summary>
        //      /// MonoBehaviour method called once per frame for every Collider 'other' that is touching the trigger.
        //      /// We're going to affect health while the beams are interesting the player
        //      /// </summary>
        //      /// <param name="other">Other.</param>
        //      public void OnTriggerStay(Collider other)
        //      {
        //          // we dont' do anything if we are not the local player.
        //          if (!photonView.IsMine)
        //          {
        //              return;
        //          }

        //          // We are only interested in Beamers
        //          // we should be using tags but for the sake of distribution, let's simply check by name.
        //          if (!other.name.Contains("Beam"))
        //          {
        //              return;
        //          }

        //          // we slowly affect health when beam is constantly hitting us, so player has to move to prevent death.
        //          this.Health -= 0.1f*Time.deltaTime;
        //      }


        //      #if !UNITY_5_4_OR_NEWER
        //      /// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
        //      void OnLevelWasLoaded(int level)
        //      {
        //          this.CalledOnLevelWasLoaded(level);
        //      }
        //      #endif


        //      /// <summary>
        //      /// MonoBehaviour method called after a new level of index 'level' was loaded.
        //      /// We recreate the Player UI because it was destroy when we switched level.
        //      /// Also reposition the player if outside the current arena.
        //      /// </summary>
        //      /// <param name="level">Level index loaded</param>
        //      void CalledOnLevelWasLoaded(int level)
        //      {
        //          // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
        //          if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        //          {
        //              transform.position = new Vector3(0f, 5f, 0f);
        //          }

        //          GameObject _uiGo = Instantiate(this.playerUiPrefab);
        //          _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        //      }

        //      #endregion

        //      #region Private Methods


        //#if UNITY_5_4_OR_NEWER
        //void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
        //{
        //	this.CalledOnLevelWasLoaded(scene.buildIndex);
        //}
        //#endif

        //      /// <summary>
        //      /// Processes the inputs. This MUST ONLY BE USED when the player has authority over this Networked GameObject (photonView.isMine == true)
        //      /// </summary>
        //      void ProcessInputs()
        //      {
        //          if (Input.GetButtonDown("Fire1"))
        //          {
        //              // we don't want to fire when we interact with UI buttons for example. IsPointerOverGameObject really means IsPointerOver*UI*GameObject
        //              // notice we don't use on on GetbuttonUp() few lines down, because one can mouse down, move over a UI element and release, which would lead to not lower the isFiring Flag.
        //              if (EventSystem.current.IsPointerOverGameObject())
        //              {
        //                  //	return;
        //              }

        //              if (!this.IsFiring)
        //              {
        //                  this.IsFiring = true;
        //              }
        //          }

        //          if (Input.GetButtonUp("Fire1"))
        //          {
        //              if (this.IsFiring)
        //              {
        //                  this.IsFiring = false;
        //              }
        //          }
        //      }

        //      #endregion

        //      #region IPunObservable implementation

        //      public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        //      {
        //          if (stream.IsWriting)
        //          {
        //              // We own this player: send the others our data
        //              stream.SendNext(this.IsFiring);
        //              stream.SendNext(this.Health);
        //          }
        //          else
        //          {
        //              // Network player, receive data
        //              this.IsFiring = (bool)stream.ReceiveNext();
        //              this.Health = (float)stream.ReceiveNext();
        //          }
        //      }

        //      #endregion
        //  }
        #endregion
    }
#if UNITY_EDITOR
	[CustomEditor(typeof(PlayerManager))]
	[CanEditMultipleObjects]

	public class PlayerManagerEditor : Editor
	{
		SerializedProperty LocalPlayerInstance, componentsToDisable, scriptsDisable;
		GUIStyle fieldBox;

		private void OnEnable()
		{
			LocalPlayerInstance = serializedObject.FindProperty("LocalPlayerInstance");
			componentsToDisable = serializedObject.FindProperty("componentsToDisable");
			scriptsDisable = serializedObject.FindProperty("scriptsDisable");
		}



		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			const int PAD = 6;
			if (fieldBox == null)
				fieldBox = new GUIStyle("HelpBox") { padding = new RectOffset(PAD, PAD, PAD, PAD) };

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.BeginVertical(fieldBox);

			EditorGUILayout.BeginVertical(fieldBox);
			GUI.backgroundColor = new Color32(39, 46, 166, 255);
			GUILayout.BeginHorizontal("box");

			GUILayout.FlexibleSpace();
			GUILayout.Label("List of Arrays", EditorStyles.boldLabel);
			GUILayout.FlexibleSpace();

			GUILayout.EndHorizontal();
			GUI.backgroundColor = Color.white;
			GUI.backgroundColor = new Color32(223, 19, 12, 255);
			EditorGUILayout.HelpBox("Don't edit the script if you have no  idea to use it.", MessageType.Warning);
			GUI.backgroundColor = Color.white;

			GUILayout.Space(10);
			EditorGUILayout.BeginVertical("box");
			EditableReferenceList(componentsToDisable, new GUIContent(componentsToDisable.displayName, componentsToDisable.tooltip), fieldBox);
			GUILayout.Label("Array list to disable all GameObjects for remote players in room.", EditorStyles.helpBox);
			EditorGUILayout.EndVertical();

			GUILayout.Space(10);

			EditorGUILayout.BeginVertical("box");
			EditableReferenceList(scriptsDisable, new GUIContent(scriptsDisable.displayName, scriptsDisable.tooltip), fieldBox);
			GUILayout.Label("Array list to disable all Scripts for remote players in room.", EditorStyles.helpBox);
			EditorGUILayout.EndVertical();

			EditorGUILayout.EndVertical();
			GUILayout.Space(10);

			EditorGUILayout.EndVertical();
			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
			}
		}



		public void EditableReferenceList(SerializedProperty list, GUIContent gc, GUIStyle style = null)
		{
			EditorGUILayout.LabelField(gc);

			if (style == null)
				style = new GUIStyle("HelpBox") { padding = new RectOffset(6, 6, 6, 6) };

			EditorGUILayout.BeginVertical(style);

			int count = list.arraySize;

			if (count == 0)
			{
				if (GUI.Button(EditorGUILayout.GetControlRect(GUILayout.MaxWidth(20)), "+", (GUIStyle)"minibutton"))
				{
					int newindex = list.arraySize;
					list.InsertArrayElementAtIndex(0);
					list.GetArrayElementAtIndex(0).objectReferenceValue = null;
				}
			}
			else
				// List Elements and Delete buttons
				for (int i = 0; i < list.arraySize; ++i)
				{
					EditorGUILayout.BeginHorizontal();
					bool add = (GUI.Button(EditorGUILayout.GetControlRect(GUILayout.MaxWidth(20)), "+", (GUIStyle)"minibutton"));
					EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), GUIContent.none);
					bool remove = (GUI.Button(EditorGUILayout.GetControlRect(GUILayout.MaxWidth(20)), "x", (GUIStyle)"minibutton"));

					EditorGUILayout.EndHorizontal();

					if (add)
					{
						int newindex = list.arraySize;
						list.InsertArrayElementAtIndex(i);
						list.GetArrayElementAtIndex(i).objectReferenceValue = null;
						EditorGUILayout.EndHorizontal();
						break;
					}

					if (remove)
					{
						list.DeleteArrayElementAtIndex(i);
						EditorGUILayout.EndHorizontal();
						break;
					}
				}

			EditorGUILayout.EndVertical();
		}
	}
#endif
}