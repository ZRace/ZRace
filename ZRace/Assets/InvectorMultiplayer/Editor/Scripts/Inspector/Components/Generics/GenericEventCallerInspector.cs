using UnityEngine;
using UnityEditor;
using CBGames.Editors;

namespace CBGames.Inspector
{
    [CustomEditor(typeof(GenericEventCaller), true)]
    public class GenericEventCallerInspector : Editor
    {
        #region Core
        GUISkin _skin = null;
        GUISkin _original = null;
        Color _titleColor;
        #endregion

        #region Properties
        SerializedProperty onAwake;
        SerializedProperty onStart;
        SerializedProperty onEnable;
        SerializedProperty onDisable;
        SerializedProperty EventsToCall;
        #endregion

        private void OnEnable()
        {
            // Load Skin for reverence
            if (!_skin) _skin = E_Helpers.LoadSkin(E_Core.e_guiSkinPath);

            //Load all images
            _titleColor = new Color32(1, 9, 28, 255); //dark blue

            #region Properties
            onAwake = serializedObject.FindProperty("onAwake");
            onStart = serializedObject.FindProperty("onStart");
            onEnable = serializedObject.FindProperty("onEnable");
            onDisable = serializedObject.FindProperty("onDisable");
            EventsToCall = serializedObject.FindProperty("EventsToCall");
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
            GUILayout.Label("Generic Event Caller", _skin.GetStyle("Label"));
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("This component will simply execute this UnityEvent on the given events.", MessageType.Info);
            #endregion

            #region Properties
            EditorGUILayout.PropertyField(onAwake);
            EditorGUILayout.PropertyField(onStart);
            EditorGUILayout.PropertyField(onEnable);
            EditorGUILayout.PropertyField(onDisable);
            GUI.skin = _original;
            EditorGUILayout.PropertyField(EventsToCall);
            GUI.skin = _skin;
            #endregion

            #region Core
            DrawPropertiesExcluding(serializedObject, E_Helpers.EditorGetVariables(typeof(GenericEventCaller)));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
            #endregion
        }
    }
}