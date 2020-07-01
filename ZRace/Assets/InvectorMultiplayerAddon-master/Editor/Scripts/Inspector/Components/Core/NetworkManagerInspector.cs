using UnityEditor;
using UnityEngine;
using CBGames.Core;
using CBGames.Editors;
using System.Collections.Generic;

namespace CBGames.Inspector
{
    [CustomEditor(typeof(NetworkManager), true)]
    public class NetworkManagerInspector : Editor
    {
        #region Properties
        SerializedProperty replayScenes;
        SerializedProperty gameVersion;
        SerializedProperty maxPlayerPerRoom;
        SerializedProperty playerPrefab;
        SerializedProperty defaultSpawnPoint;
        SerializedProperty spawnPointsTag;
        SerializedProperty connectStatus;
        SerializedProperty syncScenes;
        SerializedProperty database;
        SerializedProperty debugging;
        SerializedProperty displayDebugWindow;
        SerializedProperty teamName;
        SerializedProperty allowTeamDamaging;
        SerializedProperty initalTeamSpawnPointNames;
        SerializedProperty autoSpawnPlayer;

        SerializedProperty _lobbyEvents;
        SerializedProperty onJoinedLobby;
        SerializedProperty onLeftLobby;

        SerializedProperty _roomEvents;
        SerializedProperty onJoinedRoom;
        SerializedProperty onLeftRoom;
        SerializedProperty onCreatedRoom;
        SerializedProperty onCreateRoomFailed;
        SerializedProperty onJoinRoomFailed;

        SerializedProperty _playerEvents;
        SerializedProperty onPlayerEnteredRoom;
        SerializedProperty onPlayerLeftRoom;

        SerializedProperty _miscEvents;
        SerializedProperty onMasterClientSwitched;
        SerializedProperty onDisconnected;
        SerializedProperty onConnectedToMaster;
        SerializedProperty onFailedToConnectToPhoton;
        SerializedProperty onConnectionFail;

        SerializedProperty cameraPoints;
        SerializedProperty moveCameraPriorToJoin;
        SerializedProperty cameraMoveSpeed;
        SerializedProperty cameraCloseEnough;
        #endregion

        #region CustomEditorVariables
        GUISkin _skin = null;
        GUISkin _original = null;
        Color _titleColor;
        GUIContent eventContent;
        bool _displayLobbyEvents = true;
        bool _displayRoomEvents = false;
        bool _displayPlayerEvents = false;
        bool _displayMiscEvents = false;
        bool _addNewTeam = false;
        bool _displayTeams = false;
        string _newTeamName = "";
        string _newTeamSpawn = "";
        #endregion

        private void OnEnable()
        {
            #region Properties
            replayScenes = serializedObject.FindProperty("replayScenes");
            gameVersion = serializedObject.FindProperty("gameVersion");
            maxPlayerPerRoom = serializedObject.FindProperty("maxPlayerPerRoom");
            playerPrefab = serializedObject.FindProperty("playerPrefab");
            defaultSpawnPoint = serializedObject.FindProperty("defaultSpawnPoint");
            spawnPointsTag = serializedObject.FindProperty("spawnPointsTag");
            connectStatus = serializedObject.FindProperty("_connectStatus");
            syncScenes = serializedObject.FindProperty("syncScenes");
            database = serializedObject.FindProperty("database");
            debugging = serializedObject.FindProperty("debugging");
            displayDebugWindow = serializedObject.FindProperty("displayDebugWindow");
            teamName = serializedObject.FindProperty("teamName");
            allowTeamDamaging = serializedObject.FindProperty("allowTeamDamaging");
            initalTeamSpawnPointNames = serializedObject.FindProperty("initalTeamSpawnPointNames");
            autoSpawnPlayer = serializedObject.FindProperty("autoSpawnPlayer");

            // Unity Events
            // // Lobby Events
            _lobbyEvents = serializedObject.FindProperty("lobbyEvents");
            onJoinedLobby = _lobbyEvents.FindPropertyRelative("_onJoinedLobby");
            onLeftLobby = _lobbyEvents.FindPropertyRelative("_onLeftLobby");

            // // Room Events
            _roomEvents = serializedObject.FindProperty("roomEvents");
            onJoinedRoom = _roomEvents.FindPropertyRelative("_onJoinedRoom");
            onLeftRoom = _roomEvents.FindPropertyRelative("_onLeftRoom");
            onCreatedRoom = _roomEvents.FindPropertyRelative("_OnCreatedRoom");
            onCreateRoomFailed = _roomEvents.FindPropertyRelative("_onCreateRoomFailed");
            onJoinRoomFailed = _roomEvents.FindPropertyRelative("_onJoinRoomFailed");

            // // Player Events
            _playerEvents = serializedObject.FindProperty("playerEvents");
            onPlayerEnteredRoom = _playerEvents.FindPropertyRelative("_onPlayerEnteredRoom");
            onPlayerLeftRoom = _playerEvents.FindPropertyRelative("_onPlayerLeftRoom");

            // // Misc Events
            _miscEvents = serializedObject.FindProperty("otherEvents");
            onMasterClientSwitched = _miscEvents.FindPropertyRelative("_onMasterClientSwitched");
            onDisconnected = _miscEvents.FindPropertyRelative("_onDisconnected");
            onConnectedToMaster = _miscEvents.FindPropertyRelative("_onConnectedToMaster");
            onFailedToConnectToPhoton = _miscEvents.FindPropertyRelative("_onFailedToConnectToPhoton");
            onConnectionFail = _miscEvents.FindPropertyRelative("_onConnectionFail");

            //Camera
            cameraPoints = serializedObject.FindProperty("cameraPoints");
            moveCameraPriorToJoin = serializedObject.FindProperty("moveCameraPriorToJoin");
            cameraMoveSpeed = serializedObject.FindProperty("cameraMoveSpeed");
            cameraCloseEnough = serializedObject.FindProperty("cameraCloseEnough");
            #endregion

            #region Core
            // Load Skin for reverence
            if (!_skin) _skin = E_Helpers.LoadSkin(E_Core.e_guiSkinPath);
            
            //Load all images
            _titleColor = new Color32(1, 9, 28, 255); //dark blue
            #endregion

            //Unity Event GUIContent
            eventContent = new GUIContent("Unspecified");
        }

        public override void OnInspectorGUI()
        {
            #region Core
            // Core Requirements
            serializedObject.Update();
            NetworkManager nm = (NetworkManager)target;
            var rect = GUILayoutUtility.GetRect(1, 1);

            //Apply the gui skin
            _original = GUI.skin;
            GUI.skin = _skin;

            //Draw Background Box
            GUILayout.BeginHorizontal("box", GUILayout.ExpandHeight(false));

            GUILayout.BeginVertical(GUILayout.ExpandHeight(false));

            // Title
            EditorGUI.DrawRect(new Rect(rect.x + 5, rect.y + 10, rect.width - 10, 40), _titleColor);
            GUI.DrawTexture(new Rect(rect.x + 10, rect.y + 15, 30, 30), E_Helpers.LoadTexture(E_Core.h_networkIcon, new Vector2(256, 256)));
            GUILayout.Space(5);
            GUILayout.Label("Network Manager", _skin.GetStyle("Label"));
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Persistant component that is used to control network events like players joining, disconnects, lobby management, etc.", MessageType.Info);
            #endregion

            #region Universal Settings
            // Universal Settings
            GUILayout.BeginHorizontal();
            GUILayout.Label(EditorGUIUtility.FindTexture("BuildSettings.Web.Small"), GUILayout.ExpandWidth(false));
            GUILayout.BeginVertical();
            GUILayout.Space(9);
            GUILayout.Label("Universal Settings", _skin.GetStyle("TextField"));
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(gameVersion, new GUIContent("Game Version"));
            GUILayout.BeginHorizontal();
            if (maxPlayerPerRoom.intValue < 2)
            {
                GUILayout.Label(EditorGUIUtility.FindTexture("CollabError"), GUILayout.ExpandWidth(false), GUILayout.Height(15));
            }
            EditorGUILayout.IntSlider(maxPlayerPerRoom, 0, 255, new GUIContent("Players Per Room"));
            GUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(syncScenes);
            EditorGUILayout.PropertyField(replayScenes);
            EditorGUILayout.PropertyField(database);
            #endregion

            #region Player Settings
            // Player Settings
            GUILayout.BeginHorizontal();
            GUILayout.Label(EditorGUIUtility.FindTexture("SoftlockInline"), GUILayout.ExpandWidth(false));
            GUILayout.BeginVertical();
            GUILayout.Space(10);
            GUILayout.Label("Player Settings", _skin.GetStyle("TextField"));
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (nm.playerPrefab == null)
            {
                GUILayout.Label(EditorGUIUtility.FindTexture("CollabError"), GUILayout.ExpandWidth(false), GUILayout.Height(15));
            }
            EditorGUILayout.PropertyField(playerPrefab, new GUIContent("Player Prefab"));
            GUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(allowTeamDamaging);
            EditorGUILayout.HelpBox("Read the tooltip before setting the team name.", MessageType.Info);
            EditorGUILayout.PropertyField(teamName);

            GUI.skin = _original;
            _displayTeams = EditorGUILayout.Foldout(_displayTeams, "Initial Team Spawn Point Names");
            GUI.skin = _skin;
            if (_displayTeams == true)
            {
                EditorGUILayout.BeginHorizontal(_skin.box);
                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Team Name", GUILayout.Width(73));
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField("Starting Spawn Name", GUILayout.Width(130));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUI.skin = _original;
                if (GUILayout.Button(new GUIContent("+")))
                {
                    _addNewTeam = true;
                }
                GUI.skin = _skin;
                _newTeamName = EditorGUILayout.TextField(_newTeamName);
                _newTeamSpawn = EditorGUILayout.TextField(_newTeamSpawn);
                EditorGUILayout.EndHorizontal();

                if (_addNewTeam == true)
                {
                    _addNewTeam = false;
                    
                    nm.initalTeamSpawnPointNames.Add(_newTeamName, _newTeamSpawn);
                    _newTeamName = "";
                    _newTeamSpawn = "";
                } 

                EditorGUILayout.BeginHorizontal(_skin.box);
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("Initial Team Spawn Names", _skin.textField);
                GUI.skin = _original;
                Color org = EditorStyles.label.normal.textColor;
                EditorStyles.label.normal.textColor = Color.white;
                foreach (KeyValuePair<string, string> item in nm.initalTeamSpawnPointNames)
                {
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button(new GUIContent("-"), GUILayout.Width(20)))
                    {
                        nm.initalTeamSpawnPointNames.Remove(item.Key);
                        break;
                    }
                    EditorGUILayout.LabelField(item.Key, GUILayout.MinWidth(73));
                    EditorGUILayout.LabelField(item.Value, GUILayout.MinWidth(110));
                    EditorGUILayout.EndHorizontal();
                }
                EditorStyles.label.normal.textColor = org;
                GUI.skin = _skin;
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
            #endregion

            #region Spawn Settings
            // Spawn Settings
            GUILayout.BeginHorizontal();
            GUILayout.Label(EditorGUIUtility.FindTexture("AvatarPivot"), GUILayout.ExpandWidth(false));
            GUILayout.BeginVertical();
            GUILayout.Space(10);
            GUILayout.Label("Spawning Settings", _skin.GetStyle("TextField"));
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(autoSpawnPlayer);
            GUILayout.BeginHorizontal();
            if (nm.defaultSpawnPoint == null && nm.spawnPointsTag == "")
            {
                GUILayout.Label(EditorGUIUtility.FindTexture("CollabError"), GUILayout.ExpandWidth(false), GUILayout.Height(15));
            }
            else if (nm.defaultSpawnPoint == null && nm.spawnPointsTag != "")
            {
                GUILayout.Label(EditorGUIUtility.FindTexture("d_console.warnicon.sml"), GUILayout.ExpandWidth(false), GUILayout.Height(19));
            }
            EditorGUILayout.PropertyField(defaultSpawnPoint, new GUIContent("Default Spawn Point"));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (nm.spawnPointsTag == "" && nm.defaultSpawnPoint != null)
            {
                GUILayout.Label(EditorGUIUtility.FindTexture("d_console.warnicon.sml"), GUILayout.ExpandWidth(false), GUILayout.Height(19));
            }
            else if (nm.spawnPointsTag == "" && nm.defaultSpawnPoint == null)
            {
                GUILayout.Label(EditorGUIUtility.FindTexture("CollabError"), GUILayout.ExpandWidth(false), GUILayout.Height(15));
            }
            EditorGUILayout.PropertyField(spawnPointsTag, new GUIContent("Available Spawn Tag"));
            GUILayout.EndHorizontal();
            #endregion

            #region Debug Settings
            // Debugging Setttings
            GUILayout.BeginHorizontal();
            GUILayout.Label(EditorGUIUtility.FindTexture("d_Settings"), GUILayout.ExpandWidth(false));
            GUILayout.BeginVertical();
            GUILayout.Space(9);
            GUILayout.Label("Debug Settings", _skin.GetStyle("TextField"));
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(debugging, new GUIContent("Verbose Console Logging"));
            EditorGUILayout.PropertyField(connectStatus, new GUIContent("Connection Status"));
            EditorGUILayout.PropertyField(displayDebugWindow);
            #endregion

            #region Network Settings
            // Network Settings
            GUILayout.BeginHorizontal();
            GUILayout.Label(EditorGUIUtility.FindTexture("vcs_branch"), GUILayout.ExpandWidth(false));
            GUILayout.BeginVertical();
            GUILayout.Space(9);
            GUILayout.Label("Network Events", _skin.GetStyle("TextField"));
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Lobby", _skin.GetStyle("Button")))
            {
                _displayLobbyEvents = true;
                _displayMiscEvents = false;
                _displayPlayerEvents = false;
                _displayRoomEvents = false;
            }
            if (GUILayout.Button("Room", _skin.GetStyle("Button")))
            {
                _displayLobbyEvents = false;
                _displayMiscEvents = false;
                _displayPlayerEvents = false;
                _displayRoomEvents = true;
            }
            if (GUILayout.Button("Player", _skin.GetStyle("Button")))
            {
                _displayLobbyEvents = false;
                _displayMiscEvents = false;
                _displayPlayerEvents = true;
                _displayRoomEvents = false;
            }
            if (GUILayout.Button("Misc", _skin.GetStyle("Button")))
            {
                _displayLobbyEvents = false;
                _displayMiscEvents = true;
                _displayPlayerEvents = false;
                _displayRoomEvents = false;
            }
            GUILayout.EndHorizontal();
            if (_displayLobbyEvents == true)
            {
                GUILayout.BeginVertical("window", GUILayout.ExpandHeight(false));
                GUI.skin = _original;
                EditorGUILayout.PropertyField(onJoinedLobby);
                EditorGUILayout.PropertyField(onLeftLobby);
                GUI.skin = _skin;
                GUILayout.EndHorizontal();
            }
            if (_displayRoomEvents == true)
            {
                GUILayout.BeginVertical("window");
                GUI.skin = _original;
                EditorGUILayout.PropertyField(onJoinedRoom);
                EditorGUILayout.PropertyField(onLeftRoom);
                EditorGUILayout.PropertyField(onCreatedRoom);
                EditorGUILayout.PropertyField(onCreateRoomFailed);
                EditorGUILayout.PropertyField(onJoinRoomFailed);
                GUI.skin = _skin;
                GUILayout.EndVertical();
            }
            if (_displayPlayerEvents == true)
            {
                GUILayout.BeginVertical("window");
                GUI.skin = _original;
                EditorGUILayout.PropertyField(onPlayerEnteredRoom);
                EditorGUILayout.PropertyField(onPlayerLeftRoom);
                GUI.skin = _skin;
                GUILayout.EndVertical();
            }
            if (_displayMiscEvents == true)
            {
                GUILayout.BeginVertical("window");
                GUI.skin = _original;
                EditorGUILayout.PropertyField(onMasterClientSwitched);
                EditorGUILayout.PropertyField(onDisconnected);
                EditorGUILayout.PropertyField(onConnectedToMaster);
                EditorGUILayout.PropertyField(onFailedToConnectToPhoton);
                EditorGUILayout.PropertyField(onConnectionFail);
                GUI.skin = _skin;
                GUILayout.EndVertical();
            }
            #endregion

            #region Core
            DrawPropertiesExcluding(serializedObject, E_Helpers.EditorGetVariables(typeof(NetworkManager)));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
            //base.OnInspectorGUI();
            #endregion
        }
    }
}
