using UnityEngine;
using UnityEditor;
using CBGames.Editors;
using CBGames.UI;

namespace CBGames.Inspector
{
    [CustomEditor(typeof(VisualizePlayers), true)]
    public class VisualizePlayersInspector : Editor
    {
        #region Core
        GUISkin _skin = null;
        GUISkin _original = null;
        Color _titleColor;
        #endregion

        #region Properties
        SerializedProperty parentObj;
        SerializedProperty ownerPlayer;
        SerializedProperty otherPlayer;
        SerializedProperty teamName;
        SerializedProperty autoSetTeamIfNotSet;
        SerializedProperty allocateViewIds;
        SerializedProperty debugging;
        SerializedProperty joinedSound;
        SerializedProperty leftSound;
        SerializedProperty soundSource;
        #endregion

        private void OnEnable()
        {
            // Load Skin for reverence
            if (!_skin) _skin = E_Helpers.LoadSkin(E_Core.e_guiSkinPath);

            //Load all images
            _titleColor = new Color32(1, 9, 28, 255); //dark blue

            #region Properties
            parentObj = serializedObject.FindProperty("parentObj");
            ownerPlayer = serializedObject.FindProperty("ownerPlayer");
            otherPlayer = serializedObject.FindProperty("otherPlayer");
            teamName = serializedObject.FindProperty("teamName");
            autoSetTeamIfNotSet = serializedObject.FindProperty("autoSetTeamIfNotSet");
            allocateViewIds = serializedObject.FindProperty("allocateViewIds");
            debugging = serializedObject.FindProperty("debugging");
            joinedSound = serializedObject.FindProperty("joinedSound");
            leftSound = serializedObject.FindProperty("leftSound");
            soundSource = serializedObject.FindProperty("soundSource");
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
            GUILayout.Label("Visualize Players", _skin.GetStyle("Label"));
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("This component requires to have a \"UICoreLogic\" somewhere in your scene.\n\n" +
                "This component will spawn child objects under your selected parentObj for every player that connects to this Photon Room.", MessageType.Info);
            #endregion

            #region Properties
            EditorGUILayout.PropertyField(debugging);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(parentObj);
            EditorGUILayout.PropertyField(ownerPlayer);
            EditorGUILayout.PropertyField(otherPlayer);
            EditorGUILayout.PropertyField(teamName);
            EditorGUILayout.PropertyField(joinedSound, true);
            EditorGUILayout.PropertyField(leftSound, true);
            EditorGUILayout.PropertyField(soundSource);
            EditorGUILayout.PropertyField(allocateViewIds);
            EditorGUILayout.HelpBox("Read the tooltip before setting this value.", MessageType.Warning);
            EditorGUILayout.PropertyField(autoSetTeamIfNotSet);
            #endregion

            #region Core
            DrawPropertiesExcluding(serializedObject, E_Helpers.EditorGetVariables(typeof(VisualizePlayers)));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
            #endregion
        }
    }
}