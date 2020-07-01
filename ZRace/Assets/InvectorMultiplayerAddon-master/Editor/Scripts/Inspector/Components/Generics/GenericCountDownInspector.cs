using UnityEngine;
using UnityEditor;
using CBGames.Editors;
using CBGames.UI;

namespace CBGames.Inspector
{
    [CustomEditor(typeof(GenericCountDown))]
    public class GenericCountDownInspector : Editor
    {
        #region Core
        GUISkin _skin = null;
        GUISkin _original = null;
        Color _titleColor;
        #endregion

        #region Properties
        SerializedProperty useRoomOwnerShip;
        SerializedProperty ifIsOwner;
        SerializedProperty startTime;
        SerializedProperty countSpeed;
        SerializedProperty startType;
        SerializedProperty numberType;
        SerializedProperty texts;
        SerializedProperty soundSource;
        SerializedProperty tickClip;
        SerializedProperty OnStartCounting;
        SerializedProperty OnStopCounting;
        SerializedProperty OnNumberChange;
        SerializedProperty OnZero;
        SerializedProperty _time;
        SerializedProperty syncWithPhotonServer;
        #endregion

        private void OnEnable()
        {
            // Load Skin for reverence
            if (!_skin) _skin = E_Helpers.LoadSkin(E_Core.e_guiSkinPath);

            //Load all images
            _titleColor = new Color32(1, 9, 28, 255); //dark blue

            #region Properties
            useRoomOwnerShip = serializedObject.FindProperty("useRoomOwnerShip");
            ifIsOwner = serializedObject.FindProperty("ifIsOwner");
            startTime = serializedObject.FindProperty("startTime");
            countSpeed = serializedObject.FindProperty("countSpeed");
            startType = serializedObject.FindProperty("startType");
            numberType = serializedObject.FindProperty("numberType");
            texts = serializedObject.FindProperty("texts");
            soundSource = serializedObject.FindProperty("soundSource");
            tickClip = serializedObject.FindProperty("tickClip");
            OnStartCounting = serializedObject.FindProperty("OnStartCounting");
            OnStopCounting = serializedObject.FindProperty("OnStopCounting");
            OnNumberChange = serializedObject.FindProperty("OnNumberChange");
            OnZero = serializedObject.FindProperty("OnZero");
            _time = serializedObject.FindProperty("_time");
            syncWithPhotonServer = serializedObject.FindProperty("syncWithPhotonServer");
            #endregion
        }

        public override void OnInspectorGUI()
        {
            #region Core
            // Core Requirements
            serializedObject.Update();
            var rect = GUILayoutUtility.GetRect(1, 1);
            GenericCountDown countdown = (GenericCountDown)target;
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
            GUILayout.Label("Generic Count Down", _skin.GetStyle("Label"));
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("This component will execute a series of UnityEvents based on the countdown time.", MessageType.Info);
            #endregion

            #region Properties
            EditorGUILayout.PropertyField(useRoomOwnerShip);
            if (useRoomOwnerShip.boolValue == true)
            {
                EditorGUILayout.PropertyField(ifIsOwner);
            }
            EditorGUILayout.PropertyField(startType);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(startTime);
            EditorGUILayout.PropertyField(syncWithPhotonServer);
            if (syncWithPhotonServer.boolValue == false)
            {
                EditorGUILayout.PropertyField(countSpeed);
            }
            EditorGUILayout.PropertyField(numberType);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(texts, true);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(soundSource);
            EditorGUILayout.PropertyField(tickClip);
            GUI.enabled = false;
            EditorGUILayout.PropertyField(_time);
            GUI.enabled = true;
            EditorGUILayout.Space();
            countdown.showUnityEvents = EditorGUILayout.Foldout(countdown.showUnityEvents, "Unity Events");
            if (countdown.showUnityEvents)
            {
                GUI.skin = _original;
                EditorGUILayout.PropertyField(OnStartCounting);
                EditorGUILayout.PropertyField(OnStopCounting);
                EditorGUILayout.PropertyField(OnNumberChange);
                EditorGUILayout.PropertyField(OnZero);
                GUI.skin = _skin;
            }
            #endregion

            #region Core
            DrawPropertiesExcluding(serializedObject, E_Helpers.EditorGetVariables(typeof(GenericCountDown)));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
            #endregion
        }
    }
}