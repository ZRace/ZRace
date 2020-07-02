using UnityEngine;
using UnityEditor;
using CBGames.Editors;
using CBGames.UI;

namespace CBGames.Inspector
{
    [CustomEditor(typeof(TrackPlayerCount), true)]
    public class TrackPlayerCountInspector : Editor
    {
        #region Core
        GUISkin _skin = null;
        GUISkin _original = null;
        Color _titleColor;
        #endregion

        #region Properties
        SerializedProperty useRoomOwnerShip;
        SerializedProperty isOwner;
        SerializedProperty texts;
        SerializedProperty teamName;
        SerializedProperty executeEventAtPlayerCount;
        SerializedProperty reachPlayerCount;
        SerializedProperty fallBelowCount;
        SerializedProperty ReachedPlayerCount;
        SerializedProperty ReachedFallPlayerCount;
        SerializedProperty OnCountChanged;
        #endregion

        private void OnEnable()
        {
            // Load Skin for reverence
            if (!_skin) _skin = E_Helpers.LoadSkin(E_Core.e_guiSkinPath);

            //Load all images
            _titleColor = new Color32(1, 9, 28, 255); //dark blue

            #region Properties
            useRoomOwnerShip = serializedObject.FindProperty("useRoomOwnerShip");
            isOwner = serializedObject.FindProperty("isOwner");
            texts = serializedObject.FindProperty("texts");
            teamName = serializedObject.FindProperty("teamName");
            executeEventAtPlayerCount = serializedObject.FindProperty("executeEventAtPlayerCount");
            reachPlayerCount = serializedObject.FindProperty("reachPlayerCount");
            fallBelowCount = serializedObject.FindProperty("fallBelowCount");
            ReachedPlayerCount = serializedObject.FindProperty("ReachedPlayerCount");
            ReachedFallPlayerCount = serializedObject.FindProperty("ReachedFallPlayerCount");
            OnCountChanged = serializedObject.FindProperty("OnCountChanged");
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
            GUILayout.BeginHorizontal(_skin.box, GUILayout.ExpandHeight(false));
            GUILayout.BeginVertical(GUILayout.ExpandHeight(false));

            // Title
            EditorGUI.DrawRect(new Rect(rect.x + 5, rect.y + 10, rect.width - 10, 40), _titleColor);
            GUI.DrawTexture(new Rect(rect.x + 10, rect.y + 15, 30, 30), E_Helpers.LoadTexture(E_Core.h_uiIcon, new Vector2(256, 256)));
            GUILayout.Space(5);
            GUILayout.Label("Track Player Count", _skin.GetStyle("Label"));
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("This component requires to have a \"UICoreLogic\" somewhere in your scene.\n\n" +
                "This component will modify the targets Text components to display what the current player count is.", MessageType.Info);
            #endregion

            #region Properties
            EditorGUILayout.PropertyField(texts, true);
            EditorGUILayout.PropertyField(teamName);
            GUI.skin = _original;
            EditorGUILayout.PropertyField(OnCountChanged);
            GUI.skin = _skin; 
            EditorGUILayout.PropertyField(executeEventAtPlayerCount);
            if (executeEventAtPlayerCount.boolValue == true)
            {
                EditorGUILayout.PropertyField(useRoomOwnerShip);
                if (useRoomOwnerShip.boolValue == true)
                {
                    EditorGUILayout.PropertyField(isOwner);
                }
                EditorGUILayout.PropertyField(reachPlayerCount);
                EditorGUILayout.PropertyField(fallBelowCount);
                GUI.skin = _original;
                EditorGUILayout.PropertyField(ReachedPlayerCount);
                EditorGUILayout.PropertyField(ReachedFallPlayerCount);
                GUI.skin = _skin;
            }
            #endregion

            #region Core
            DrawPropertiesExcluding(serializedObject, E_Helpers.EditorGetVariables(typeof(TrackPlayerCount)));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
            #endregion
        }
    }
}