using UnityEngine;
using UnityEditor;
using CBGames.Editors;
using CBGames.Player;

namespace CBGames.Inspector
{
    [CustomEditor(typeof(PlayerNameBar),true)]
    public class PlayerNameBarInspector : Editor
    {
        #region CustomEditorVariables
        GUISkin _skin = null;
        GUISkin _original = null;
        Color _titleColor;
        #endregion

        #region Properties
        SerializedProperty playerName;
        SerializedProperty playerBar;
        #endregion

        private void OnEnable()
        {
            // Load Skin for reverence
            if (!_skin) _skin = E_Helpers.LoadSkin(E_Core.e_guiSkinPath);

            //Load all images
            _titleColor = new Color32(1, 9, 28, 255); //dark blue

            //feilds
            playerName = serializedObject.FindProperty("playerName");
            playerBar = serializedObject.FindProperty("playerBar");
        }

        public override void OnInspectorGUI()
        {
            #region Core
            // Core Requirements
            serializedObject.Update();
            PlayerNameBar nb = (PlayerNameBar)target;
            var rect = GUILayoutUtility.GetRect(1, 1);

            //Apply the gui skin
            _original = GUI.skin;
            GUI.skin = _skin;

            //Draw Background Box
            GUILayout.BeginHorizontal(_skin.box, GUILayout.ExpandHeight(false));

            GUILayout.BeginVertical(GUILayout.ExpandHeight(false));

            // Title
            EditorGUI.DrawRect(new Rect(rect.x + 5, rect.y + 10, rect.width - 10, 40), _titleColor);
            GUI.DrawTexture(new Rect(rect.x + 10, rect.y + 15, 30, 30), E_Helpers.LoadTexture(E_Core.h_playerIcon, new Vector2(256, 256)));
            GUILayout.Space(5);
            GUILayout.Label("Network Player Name Bar", _skin.GetStyle("Label"));
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Component that belongs on each player. Will set the PlayerName text to be what the network nickname is set to. The nickname can be set via the NetworkManager component.", MessageType.Info);
            #endregion

            //Properties
            EditorGUILayout.PropertyField(playerName);
            EditorGUILayout.PropertyField(playerBar);

            DrawPropertiesExcluding(serializedObject, E_Helpers.EditorGetVariables(typeof(PlayerNameBar)));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
