using CBGames.Editors;
using UnityEditor;
using UnityEngine;

namespace CBGames.Inspector
{
    [CustomEditor(typeof(EnableIfTeam), true)]
    public class EnableIfTeamInspector : Editor
    {
        #region Core
        GUISkin _skin = null;
        GUISkin _original = null;
        Color _titleColor;
        #endregion

        #region Properties
        SerializedProperty checkType;
        SerializedProperty teamName;
        SerializedProperty items;
        #endregion

        private void OnEnable()
        {
            // Load Skin for reverence
            if (!_skin) _skin = E_Helpers.LoadSkin(E_Core.e_guiSkinPath);

            //Load all images
            _titleColor = new Color32(1, 9, 28, 255); //dark blue

            #region Properties
            checkType = serializedObject.FindProperty("checkType");
            teamName = serializedObject.FindProperty("teamName");
            items = serializedObject.FindProperty("items");
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
            GUILayout.Label("Enable If Team", _skin.GetStyle("Label"));
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Doesn't require a UICoreLogic component. Will look at the team set " +
                "in the NetworkManager component as see if it matches. If it does will enable all the targeted " +
                "items otherwise it will disable them.", MessageType.Info);
            #endregion

            #region Properties
            EditorGUILayout.PropertyField(checkType);
            EditorGUILayout.PropertyField(teamName);
            EditorGUILayout.PropertyField(items, true);
            #endregion

            #region Core
            DrawPropertiesExcluding(serializedObject, E_Helpers.EditorGetVariables(typeof(EnableIfTeam)));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
            #endregion
        }
    }
}