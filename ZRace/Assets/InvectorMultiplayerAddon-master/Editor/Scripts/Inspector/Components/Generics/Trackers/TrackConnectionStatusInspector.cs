﻿using UnityEngine;
using UnityEditor;
using CBGames.Editors;
using CBGames.UI;

namespace CBGames.Inspector
{
    [CustomEditor(typeof(TrackConnectionStatus), true)]
    public class TrackConnectionStatusInspector : Editor
    {
        #region Core
        GUISkin _skin = null;
        GUISkin _original = null;
        Color _titleColor;
        #endregion

        #region Properties
        SerializedProperty texts;
        SerializedProperty OnConnectedToLobby;
        SerializedProperty OnConnectedToRoom;
        #endregion

        private void OnEnable()
        {
            // Load Skin for reverence
            if (!_skin) _skin = E_Helpers.LoadSkin(E_Core.e_guiSkinPath);

            //Load all images
            _titleColor = new Color32(1, 9, 28, 255); //dark blue

            #region Properties
            texts = serializedObject.FindProperty("texts");
            OnConnectedToLobby = serializedObject.FindProperty("OnConnectedToLobby");
            OnConnectedToRoom = serializedObject.FindProperty("OnConnectedToRoom");
            #endregion
        }

        public override void OnInspectorGUI()
        {
            #region Core
            // Core Requirements
            serializedObject.Update();
            var rect = GUILayoutUtility.GetRect(1, 1);

            //Apply the gui skin
            _original = GUI.skin;
            GUI.skin = _skin;

            //Draw Background Box
            GUILayout.BeginHorizontal("box", GUILayout.ExpandHeight(false));
            GUILayout.BeginVertical(GUILayout.ExpandHeight(false));

            // Title
            EditorGUI.DrawRect(new Rect(rect.x + 5, rect.y + 10, rect.width - 10, 40), _titleColor);
            GUI.DrawTexture(new Rect(rect.x + 10, rect.y + 15, 30, 30), E_Helpers.LoadTexture(E_Core.h_uiIcon, new Vector2(256, 256)));
            GUILayout.Space(5);
            GUILayout.Label("Track Connection Status", _skin.GetStyle("Label"));
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("This component requires to have a \"UICoreLogic\" somewhere in your scene.\n\n" +
                "This component will change the targeted Text to match whatever the connection status is, from the NetworkManager.", MessageType.Info);
            #endregion

            #region Properties
            EditorGUILayout.PropertyField(texts, true);
            GUI.skin = _original;
            EditorGUILayout.HelpBox("Call when you successfully connect to the Photon Lobby.", MessageType.Info);
            EditorGUILayout.PropertyField(OnConnectedToLobby);
            EditorGUILayout.HelpBox("Call when you successfully connect to a Photon Room.", MessageType.Info);
            EditorGUILayout.PropertyField(OnConnectedToRoom);
            GUI.skin = _skin;
            #endregion

            #region Core
            DrawPropertiesExcluding(serializedObject, E_Helpers.EditorGetVariables(typeof(TrackConnectionStatus)));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
            #endregion
        }
    }
}