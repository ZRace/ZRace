using UnityEditor;
using UnityEngine;
using CBGames.Editors;
using CBGames.Objects;
using UnityEditorInternal;

namespace CBGames.Inspector
{
    [CustomEditor(typeof(PreviewCamera), true, isFallback = true)]
    public class PreviewCameraInspector : Editor
    {
        #region Properties
        SerializedProperty cameraPoints;
        SerializedProperty moveImmediatly;
        SerializedProperty cameraMoveSpeed;
        SerializedProperty cameraCloseEnough;
        SerializedProperty stopOnJoinRoom;
        SerializedProperty targetCam;
        SerializedProperty networkManager;
        #endregion

        #region CustomEditorVariables
        GUISkin _skin = null;
        GUISkin _original = null;
        Color _titleColor;
        PreviewCamera _target;
        private ReorderableList _cameraPointsList;
        #endregion

        private void OnEnable()
        {
            _target = (PreviewCamera)target;
            _cameraPointsList = new ReorderableList(serializedObject, serializedObject.FindProperty("cameraPoints"), true, true, true, true);
            _cameraPointsList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Camera Transition Points", EditorStyles.boldLabel);
            };
            _cameraPointsList.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var element = _cameraPointsList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
                };

            #region Properties
            //Get Properties
            cameraPoints = serializedObject.FindProperty("cameraPoints");
            moveImmediatly = serializedObject.FindProperty("moveImmediatly");
            cameraMoveSpeed = serializedObject.FindProperty("cameraMoveSpeed");
            cameraCloseEnough = serializedObject.FindProperty("cameraCloseEnough");
            stopOnJoinRoom = serializedObject.FindProperty("stopOnJoinRoom");
            targetCam = serializedObject.FindProperty("targetCam");
            networkManager = serializedObject.FindProperty("networkManager");
            #endregion

            #region Core
            // Load Skin for reverence
            if (!_skin) _skin = E_Helpers.LoadSkin(E_Core.e_guiSkinPath);

            //Load all images
            _titleColor = new Color32(1, 9, 28, 255); //dark blue
            #endregion
        }
        public override void OnInspectorGUI()
        {
            #region Core
            // Core Requirements
            serializedObject.Update();
            PreviewCamera pc = (PreviewCamera)target;
            var rect = GUILayoutUtility.GetRect(1, 1);

            //Apply the gui skin
            _original = GUI.skin;
            GUI.skin = _skin;

            //Draw Background Box
            GUILayout.BeginHorizontal(_skin.box, GUILayout.ExpandHeight(false));

            GUILayout.BeginVertical(GUILayout.ExpandHeight(false));

            // Title
            EditorGUI.DrawRect(new Rect(rect.x + 5, rect.y + 10, rect.width - 10, 40), _titleColor);
            GUI.DrawTexture(new Rect(rect.x + 10, rect.y + 15, 30, 30), E_Helpers.LoadTexture(E_Core.h_cameraPath, new Vector2(256, 256)));
            GUILayout.Space(5);
            GUILayout.Label("Preview Camera", _skin.GetStyle("Label"));
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Component thats used to make a target camera follow a series of points. This is designed specifically for a lobby preview camera however this does contain a start/stop function for outside components to call.", MessageType.Info);
            #endregion

            #region Camera
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(moveImmediatly);
            EditorGUILayout.PropertyField(stopOnJoinRoom);
            GUILayout.BeginHorizontal(_skin.customStyles[1], GUILayout.ExpandHeight(false));
            GUILayout.BeginVertical(GUILayout.ExpandHeight(false));
            GUI.skin = _original;
            _cameraPointsList.DoLayoutList();
            GUI.skin = _skin;
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            EditorGUILayout.PropertyField(cameraMoveSpeed);
            EditorGUILayout.PropertyField(cameraCloseEnough);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Optional Fields", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(targetCam);
            EditorGUILayout.PropertyField(networkManager);
            #endregion

            #region Core
            DrawPropertiesExcluding(serializedObject, E_Helpers.EditorGetVariables(typeof(PreviewCamera)));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
            #endregion
        }
    }
}