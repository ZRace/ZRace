using UnityEngine;
using UnityEditor;
using CBGames.UI;
using CBGames.Editors;

namespace CBGames.Inspector
{
    [CustomEditor(typeof(FloatingBar), true)]
    public class FloatingBarInspector : Editor
    {
        #region Core
        GUISkin _skin = null;
        GUISkin _original = null;
        Color _titleColor;
        #endregion

        #region Properties
        SerializedProperty type;
        SerializedProperty displayType;
        SerializedProperty realTimeTracking;
        SerializedProperty allTexts;
        SerializedProperty allImages;
        SerializedProperty coloredBar;
        SerializedProperty colorBarFillOffset;
        SerializedProperty fillBar;
        SerializedProperty fillDelay;
        SerializedProperty fillSpeed;
        SerializedProperty displayBarNumber;
        SerializedProperty controller;
        SerializedProperty startHidden;
        SerializedProperty displayTime;
        SerializedProperty fadeOut;
        SerializedProperty fadeSpeed;
        SerializedProperty onlyEnableForNoneOwner;
        #endregion

        private void OnEnable()
        {
            #region Core
            // Load Skin for reverence
            if (!_skin) _skin = E_Helpers.LoadSkin(E_Core.e_guiSkinPath);

            //Load all images
            _titleColor = new Color32(1, 9, 28, 255); //dark blue
            #endregion

            #region Properties
            type = serializedObject.FindProperty("type");
            displayType = serializedObject.FindProperty("displayType");
            realTimeTracking = serializedObject.FindProperty("realTimeTracking");
            allTexts = serializedObject.FindProperty("allTexts");
            allImages = serializedObject.FindProperty("allImages");
            coloredBar = serializedObject.FindProperty("coloredBar");
            colorBarFillOffset = serializedObject.FindProperty("colorBarFillOffset");
            fillBar = serializedObject.FindProperty("fillBar");
            fillDelay = serializedObject.FindProperty("fillDelay");
            fillSpeed = serializedObject.FindProperty("fillSpeed");
            displayBarNumber = serializedObject.FindProperty("displayBarNumber");
            controller = serializedObject.FindProperty("controller");
            startHidden = serializedObject.FindProperty("startHidden");
            displayTime = serializedObject.FindProperty("displayTime");
            fadeOut = serializedObject.FindProperty("fadeOut");
            fadeSpeed = serializedObject.FindProperty("fadeSpeed");
            onlyEnableForNoneOwner = serializedObject.FindProperty("onlyEnableForNoneOwner");
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
            GUI.DrawTexture(new Rect(rect.x + 10, rect.y + 15, 30, 30), E_Helpers.LoadTexture(E_Core.h_floatingBarIcon, new Vector2(256, 256)));
            GUILayout.Space(5);
            GUILayout.Label("Floating Bar", _skin.GetStyle("Label"));
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Use to visually display the status of your tracked number in text and progress bar format.", MessageType.Info);
            #endregion

            #region Properties
            #region Others
            EditorGUILayout.LabelField("Core Settings", _skin.textField);
            GUILayout.BeginHorizontal(_skin.box);
            GUILayout.BeginVertical();
            EditorGUILayout.PropertyField(type);
            EditorGUILayout.PropertyField(displayType);
            EditorGUILayout.PropertyField(realTimeTracking);
            EditorGUILayout.PropertyField(controller);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            #endregion

            #region Contents
            EditorGUILayout.LabelField("Contents", _skin.textField);
            GUILayout.BeginHorizontal(_skin.box);
            GUILayout.BeginVertical();
            EditorGUILayout.PropertyField(allTexts, true);
            EditorGUILayout.PropertyField(allImages, true);
            EditorGUILayout.PropertyField(coloredBar);
            EditorGUILayout.PropertyField(fillBar);
            EditorGUILayout.PropertyField(displayBarNumber);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            #endregion

            #region Bar Settings
            EditorGUILayout.LabelField("Bar Fill Settings", _skin.textField);
            GUILayout.BeginHorizontal(_skin.box);
            GUILayout.BeginVertical();
            EditorGUILayout.PropertyField(colorBarFillOffset);
            EditorGUILayout.PropertyField(fillDelay);
            EditorGUILayout.PropertyField(fillSpeed);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            #endregion

            #region Display Options
            EditorGUILayout.LabelField("Display Options", _skin.textField);
            GUILayout.BeginHorizontal(_skin.box);
            GUILayout.BeginVertical();
            EditorGUILayout.PropertyField(startHidden);
            EditorGUILayout.PropertyField(displayTime);
            EditorGUILayout.PropertyField(fadeOut);
            EditorGUILayout.PropertyField(fadeSpeed);
            EditorGUILayout.PropertyField(onlyEnableForNoneOwner);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            #endregion
            #endregion

            #region Core
            DrawPropertiesExcluding(serializedObject, E_Helpers.EditorGetVariables(typeof(FloatingBar)));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
            #endregion
        }
    }
}
