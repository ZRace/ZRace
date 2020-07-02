/*
using UnityEditor;
using UnityEngine;
using CBGames.Editors;
using CBGames.Objects;

namespace CBGames.Inspector
{
    [CustomEditor(typeof(MP_vSimpleTrigger), true)]
    public class MP_vSimpleTriggerInspector : Editor
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
            GUILayout.Label("Network Simple Trigger", _skin.GetStyle("Label"));
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("This will watch when a target tagged/layered collider enters/exits the trigger. " +
                "If on your vSimpleTrigger component you have actions in either of these it will trigger " +
                "it over the network ONLY if the collider that entered the trigger has a PhotonView component " +
                "attached to it. If no PhotonView is found on the object that entered/exited the trigger then " +
                "it will NOT Invoke these UnityEvents over the network.", MessageType.Info);
            #endregion

            #region End Core
            DrawPropertiesExcluding(serializedObject, E_Helpers.EditorGetVariables(typeof(MP_vSimpleTrigger)));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
            #endregion
        }
    }
}
*/
