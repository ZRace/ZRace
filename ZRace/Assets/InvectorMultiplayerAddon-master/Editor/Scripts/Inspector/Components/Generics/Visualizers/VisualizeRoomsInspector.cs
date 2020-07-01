using UnityEngine;
using UnityEditor;
using CBGames.Editors;
using CBGames.UI;

namespace CBGames.Inspector
{
    [CustomEditor(typeof(VisualizeRooms), true)]
    public class VisualizeRoomsInspector : Editor
    {
        #region Core
        GUISkin _skin = null;
        GUISkin _original = null;
        Color _titleColor;
        #endregion

        #region Properties
        SerializedProperty parentObj;
        SerializedProperty roomButton;
        SerializedProperty autoUpate;
        SerializedProperty canDisplaySessionRooms;
        SerializedProperty onlyDisplaySessionRooms;
        SerializedProperty filterRooms;
        SerializedProperty debugging;
        #endregion

        private void OnEnable()
        {
            // Load Skin for reverence
            if (!_skin) _skin = E_Helpers.LoadSkin(E_Core.e_guiSkinPath);

            //Load all images
            _titleColor = new Color32(1, 9, 28, 255); //dark blue

            #region Properties
            parentObj = serializedObject.FindProperty("parentObj");
            roomButton = serializedObject.FindProperty("roomButton");
            autoUpate = serializedObject.FindProperty("autoUpate");
            canDisplaySessionRooms = serializedObject.FindProperty("canDisplaySessionRooms");
            onlyDisplaySessionRooms = serializedObject.FindProperty("onlyDisplaySessionRooms");
            filterRooms = serializedObject.FindProperty("filterRooms");
            debugging = serializedObject.FindProperty("debugging");
            #endregion
        }

        public override void OnInspectorGUI()
        {
            #region Core
            // Core Requirements
            serializedObject.Update();
            VisualizeRooms vr = (VisualizeRooms)target;
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
            GUILayout.Label("Visualize Photon Rooms", _skin.GetStyle("Label"));
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("This component requires to have a \"UICoreLogic\" somewhere in your scene.\n\n" +
                "This component will spawn child objects under your selected parentObj " +
                "for every photon room that it finds. If that child object holds a component called \"RoomButton\" " +
                "it will send the data about that photon room to that component.", MessageType.Info);
            #endregion

            #region Properties
            EditorGUILayout.PropertyField(parentObj);
            EditorGUILayout.PropertyField(roomButton);
            EditorGUILayout.PropertyField(autoUpate);
            EditorGUILayout.PropertyField(canDisplaySessionRooms);
            EditorGUILayout.PropertyField(onlyDisplaySessionRooms);
            EditorGUILayout.PropertyField(filterRooms);
            EditorGUILayout.PropertyField(debugging);
            #endregion

            #region Core
            DrawPropertiesExcluding(serializedObject, E_Helpers.EditorGetVariables(typeof(VisualizeRooms)));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
            #endregion
        }
    }
}