using UnityEditor;
using UnityEngine;
using CBGames.Editors;
using CBGames.UI;
using System.Collections.Generic;

namespace CBGames.Inspector
{
    [CustomEditor(typeof(DeathCamera), true)]
    public class DeathCameraInspector : Editor
    {
        #region Core Variables
        GUISkin _skin = null;
        GUISkin _original = null;
        Color _titleColor;
        GUIContent eventContent;
        #endregion

        #region Properties
        SerializedProperty keyToSwitchPrevious;
        SerializedProperty keyToSwitchNext;
        SerializedProperty deathVisual;
        SerializedProperty OnEnableSwitching;
        SerializedProperty OnDisableSwitching;
        #endregion

        List<string> availableInputs = new List<string>();
        int prevInt;
        int nextInt;

        private void OnEnable()
        {
            //Properties
            keyToSwitchPrevious = serializedObject.FindProperty("keyToSwitchPrevious");
            keyToSwitchNext = serializedObject.FindProperty("keyToSwitchNext");
            deathVisual = serializedObject.FindProperty("deathVisual");
            OnEnableSwitching = serializedObject.FindProperty("OnEnableSwitching");
            OnDisableSwitching = serializedObject.FindProperty("OnDisableSwitching");

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
            GUI.DrawTexture(new Rect(rect.x + 10, rect.y + 15, 30, 30), E_Helpers.LoadTexture(E_Core.h_deathCameraIcon, new Vector2(256, 256)));
            GUILayout.Space(5);
            GUILayout.Label("Death Camera", _skin.GetStyle("Label"));
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Call \"EnableSwitching\" function to turn on/off " +
                "the functionality to allow the player to switch between other players. " +
                "A helper function \"DeadEnableDeathCamera\" function is availabe in the \"SyncPlayer\" " +
                "component that can be tied to the \"vThirdPersonController\" component.\n\n" +
                "vThirdPersonController (OnDead) -> SyncPlayer(DeadEnableDeathCamera) -> DeathCamera(EnableSwitching)", MessageType.Info);
            #endregion

            #region Properties
            int pre_index = availableInputs.IndexOf(keyToSwitchPrevious.stringValue);
            if (pre_index == -1)
            {
                EditorGUILayout.PropertyField(keyToSwitchPrevious, new GUIContent("Previous Player Key"));
                EditorGUILayout.HelpBox("This key doesn't exist in your project. Be sure to add it before you final build!", MessageType.Error);
            }
            else
            {
                prevInt = pre_index;
                prevInt = EditorGUILayout.Popup("Previous Player Key", prevInt, availableInputs.ToArray());
                keyToSwitchPrevious.stringValue = availableInputs[prevInt];
            }

            int next_index = availableInputs.IndexOf(keyToSwitchNext.stringValue);
            if (next_index == -1)
            {
                EditorGUILayout.PropertyField(keyToSwitchNext, new GUIContent("Next Player Key"));
                EditorGUILayout.HelpBox("This key doesn't exist in your project. Be sure to add it before you final build!", MessageType.Error);
            }
            else
            {
                nextInt = next_index;
                nextInt = EditorGUILayout.Popup("Next Player Key", nextInt, availableInputs.ToArray());
                keyToSwitchNext.stringValue = availableInputs[nextInt];
            }
            EditorGUILayout.PropertyField(deathVisual);
            GUI.skin = _original;
            EditorGUILayout.PropertyField(OnEnableSwitching);
            EditorGUILayout.PropertyField(OnDisableSwitching);
            GUI.skin = _skin;
            #endregion

            #region Core
            DrawPropertiesExcluding(serializedObject, E_Helpers.EditorGetVariables(typeof(DeathCamera)));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
            #endregion
        }
    }
}