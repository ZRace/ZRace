using UnityEditor;
using UnityEngine;
using CBGames.Editors;
using CBGames.UI;
using System.Collections.Generic;

namespace CBGames.Inspector
{
    [CustomEditor(typeof(PlayerList), true)]
    public class PlayerListInspector : Editor
    {
        #region CoreVariables
        GUISkin _skin = null;
        GUISkin _original = null;
        Color _titleColor;
        GUIContent eventContent;
        #endregion

        #region Properties
        SerializedProperty content;
        SerializedProperty rootObj;
        SerializedProperty playerJoinObject;
        SerializedProperty openWindow;
        SerializedProperty keyToPress;
        SerializedProperty autoCloseWithChatBox;
        SerializedProperty delayDisable;
        SerializedProperty anim;
        SerializedProperty openAnimation;
        SerializedProperty closeAnimation;
        SerializedProperty soundSource;
        SerializedProperty openSound;
        SerializedProperty closeSound;
        SerializedProperty openSoundVolume;
        SerializedProperty closeSoundVolume;
        SerializedProperty debugging;
        #endregion

        List<string> availableInputs = new List<string>();
        int openInt;

        private void OnEnable()
        {
            //Properties
            content = serializedObject.FindProperty("content");
            rootObj = serializedObject.FindProperty("rootObj");
            playerJoinObject = serializedObject.FindProperty("playerJoinObject");
            openWindow = serializedObject.FindProperty("openWindow");
            keyToPress = serializedObject.FindProperty("keyToPress");
            autoCloseWithChatBox = serializedObject.FindProperty("autoCloseWithChatBox");
            delayDisable = serializedObject.FindProperty("delayDisable");
            anim = serializedObject.FindProperty("anim");
            openAnimation = serializedObject.FindProperty("openAnimation");
            closeAnimation = serializedObject.FindProperty("closeAnimation");
            soundSource = serializedObject.FindProperty("soundSource");
            openSound = serializedObject.FindProperty("openSound");
            openSoundVolume = serializedObject.FindProperty("openSoundVolume");
            closeSound = serializedObject.FindProperty("closeSound");
            closeSoundVolume = serializedObject.FindProperty("closeSoundVolume");
            debugging = serializedObject.FindProperty("debugging");

            // Load Skin for reverence
            if (!_skin) _skin = E_Helpers.LoadSkin(E_Core.e_guiSkinPath);

            //Load all images
            _titleColor = new Color32(1, 9, 28, 255); //dark blue

            availableInputs = E_Helpers.GetAllInputAxis();
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
            GUI.DrawTexture(new Rect(rect.x + 10, rect.y + 15, 30, 30), E_Helpers.LoadTexture(E_Core.h_playerlistIcon, new Vector2(256, 256)));
            GUILayout.Space(5);
            GUILayout.Label("Player List UI", _skin.GetStyle("Label"));
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("This is a visual UI element that is populated with player data " +
                "from the chatbox. \n\n" +
                "Call \"AddPlayer\" function to add a player to the list.\n" +
                "Call \"RemovePlayer\" function to remove a player from the list.", MessageType.Info);
            #endregion

            #region Properties
            
            EditorGUILayout.LabelField("UI Parts");
            GUILayout.BeginHorizontal("box", GUILayout.ExpandHeight(false));
            GUILayout.BeginVertical(GUILayout.ExpandHeight(false));
            EditorGUILayout.PropertyField(content);
            EditorGUILayout.PropertyField(rootObj);
            EditorGUILayout.PropertyField(playerJoinObject);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            EditorGUILayout.LabelField("Key Press");
            GUILayout.BeginHorizontal("box", GUILayout.ExpandHeight(false));
            GUILayout.BeginVertical(GUILayout.ExpandHeight(false));
            EditorGUILayout.PropertyField(openWindow);
            if (openWindow.intValue != 2)
            {
                string titleText = (openWindow.intValue == 0) ? "Key To Hold" : "Key To Press";
                int index = availableInputs.IndexOf(keyToPress.stringValue);
                if (index == -1)
                {
                    EditorGUILayout.PropertyField(keyToPress, new GUIContent(titleText));
                    EditorGUILayout.HelpBox("This key doesn't exist in your project. Be sure to add it before you final build!", MessageType.Error);
                }
                else
                {
                    openInt = index;
                    openInt = EditorGUILayout.Popup(titleText, openInt, availableInputs.ToArray());
                    keyToPress.stringValue = availableInputs[openInt];
                }
                EditorGUILayout.PropertyField(autoCloseWithChatBox);
            }
            EditorGUILayout.PropertyField(delayDisable);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            EditorGUILayout.LabelField("Animations");
            GUILayout.BeginHorizontal("box", GUILayout.ExpandHeight(false));
            GUILayout.BeginVertical(GUILayout.ExpandHeight(false));
            EditorGUILayout.PropertyField(anim);
            EditorGUILayout.PropertyField(openAnimation);
            EditorGUILayout.PropertyField(closeAnimation);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            EditorGUILayout.LabelField("Sounds");
            GUILayout.BeginHorizontal("box", GUILayout.ExpandHeight(false));
            GUILayout.BeginVertical(GUILayout.ExpandHeight(false));
            EditorGUILayout.PropertyField(soundSource);
            EditorGUILayout.PropertyField(openSound);
            EditorGUILayout.PropertyField(openSoundVolume);
            EditorGUILayout.PropertyField(closeSound);
            EditorGUILayout.PropertyField(closeSoundVolume);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal("box", GUILayout.ExpandHeight(false));
            GUILayout.BeginVertical(GUILayout.ExpandHeight(false));
            EditorGUILayout.PropertyField(debugging);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            #endregion

            #region Core
            DrawPropertiesExcluding(serializedObject, E_Helpers.EditorGetVariables(typeof(PlayerList)));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
            #endregion

        }
    }
}