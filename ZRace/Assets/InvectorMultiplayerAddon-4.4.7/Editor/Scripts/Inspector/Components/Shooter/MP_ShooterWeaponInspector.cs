/*
using CBGames.Objects;
using UnityEditor;
using UnityEngine;

namespace CBGames.Editors
{
    [CustomEditor(typeof(MP_ShooterWeapon), true)]
    public class MP_ShooterWeaponInspector : Editor
    {
        #region CustomEditorVariables
        GUISkin _skin = null;
        GUISkin _original = null;
        Color _titleColor;
        #endregion

        private void OnEnable()
        {
            if (!_skin) _skin = E_Helpers.LoadSkin(E_Core.e_guiSkinPath);
            _titleColor = new Color32(1, 9, 28, 255); //dark blue
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
            GUILayout.BeginHorizontal(GUI.skin.box, GUILayout.ExpandHeight(false));
            GUILayout.BeginVertical(GUILayout.ExpandHeight(false));

            // Title
            EditorGUI.DrawRect(new Rect(rect.x + 5, rect.y + 10, rect.width - 10, 40), _titleColor);
            GUI.DrawTexture(new Rect(rect.x + 10, rect.y + 15, 30, 30), E_Helpers.LoadTexture(E_Core.h_genericIcon, new Vector2(256, 256)));
            GUILayout.Space(5);
            GUILayout.Label("Network Shooter Weapon", _skin.GetStyle("Label"));
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("This contains a series of functions that is designed to be called by the UnityEvents of the shooter weapon.", MessageType.Info);
            #endregion

            #region End Core
            DrawPropertiesExcluding(serializedObject, E_Helpers.EditorGetVariables(typeof(MP_ShooterWeapon)));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
            #endregion
        }
    }
}
*/
