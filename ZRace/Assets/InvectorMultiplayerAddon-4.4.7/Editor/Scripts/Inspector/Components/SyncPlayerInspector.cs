using UnityEngine;
using UnityEditor;
using CBGames.Player;
using CBGames.Editors;

namespace CBGames.Inspector
{
    [CustomEditor(typeof(SyncPlayer), true)]
    public class SyncPlayerInspector : Editor
    {
        #region CustomEditorVariables
        GUISkin _skin = null;
        GUISkin _original = null;
        Color _titleColor;
        #endregion

        #region Serialized Properties
        SerializedProperty _syncAnimations;
        SerializedProperty _positionLerpRate;
        SerializedProperty _rotationLerpRate;
        SerializedProperty noneLocalTag;
        SerializedProperty _nonAuthoritativeLayer;
        SerializedProperty teamName;
        #endregion

        private void OnEnable()
        {
            // Load Skin for reverence
            if (!_skin) _skin = E_Helpers.LoadSkin(E_Core.e_guiSkinPath);

            //Load all images
            _titleColor = new Color32(1, 9, 28, 255); //dark blue

            //Serialized Properties 
            _syncAnimations = serializedObject.FindProperty("_syncAnimations");
            _positionLerpRate = serializedObject.FindProperty("_positionLerpRate");
            _rotationLerpRate = serializedObject.FindProperty("_rotationLerpRate");
            noneLocalTag = serializedObject.FindProperty("noneLocalTag");
            _nonAuthoritativeLayer = serializedObject.FindProperty("_nonAuthoritativeLayer");
            teamName = serializedObject.FindProperty("teamName");
        }

        public override void OnInspectorGUI()
        {
            #region Core
            // Core Requirements
            serializedObject.Update();
            SyncPlayer sp = (SyncPlayer)target;
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
            GUILayout.Label("Network Sync Player", _skin.GetStyle("Label"));
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Component that belongs on each player. Will send events over the network like animations, damage, etc.", MessageType.Info);
            #endregion

            //Properties 
            GUILayout.BeginHorizontal(_skin.customStyles[1]);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label(EditorGUIUtility.FindTexture("animationkeyframe"), GUILayout.ExpandWidth(false));
            GUILayout.BeginVertical();
            GUILayout.Space(9);
            GUILayout.Label("Sync Animations Settings", _skin.GetStyle("TextField"));
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(_skin.window);
            EditorGUILayout.PropertyField(_syncAnimations, new GUIContent("Sync Animations"));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.BeginHorizontal(_skin.customStyles[1]);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label(EditorGUIUtility.FindTexture("d_UnityEditor.AnimationWindow"), GUILayout.ExpandWidth(false));
            GUILayout.BeginVertical();
            GUILayout.Space(9);
            GUILayout.Label("Sync Position/Rotation Settings", _skin.GetStyle("TextField"));
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(_skin.window);
            GUILayout.BeginVertical();
            EditorGUILayout.Slider(_positionLerpRate, 0, 25, new GUIContent("Position Move Speed"));
            EditorGUILayout.Slider(_rotationLerpRate, 0, 25, new GUIContent("Rotation Move Speed"));
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.BeginHorizontal(_skin.customStyles[1]);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label(EditorGUIUtility.FindTexture("TerrainInspector.TerrainToolSettings"), GUILayout.ExpandWidth(false));
            GUILayout.BeginVertical();
            GUILayout.Space(9);
            GUILayout.Label("None Owner Settings", _skin.GetStyle("TextField"));
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(_skin.window);
            GUILayout.BeginVertical();
            EditorGUILayout.PropertyField(noneLocalTag, new GUIContent("None Owner Tag"));
            EditorGUILayout.PropertyField(_nonAuthoritativeLayer, new GUIContent("None Owner Layer"));
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            GUILayout.BeginHorizontal(_skin.customStyles[1]);
            GUILayout.Label(EditorGUIUtility.FindTexture("d_editcollision_16"), GUILayout.ExpandWidth(false));
            GUILayout.BeginVertical();
            GUILayout.Space(9);
            GUILayout.Label("Team Settings", _skin.textField);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(_skin.window);
            GUILayout.BeginVertical();
            GUI.enabled = false;
            EditorGUILayout.PropertyField(teamName);
            GUI.enabled = true;
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.Space(5);

            DrawPropertiesExcluding(serializedObject, E_Helpers.EditorGetVariables(typeof(SyncPlayer)));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }
    }
}