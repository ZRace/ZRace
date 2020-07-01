using UnityEngine;
using UnityEditor;
using CBGames.Editors;
using CBGames.Objects;

namespace CBGames.Inspector
{
    [CustomEditor(typeof(NetworkBreakObject), true)]
    public class NetworkBreakObjectInspector : Editor
    {
        #region CustomEditorVariables
        GUISkin _skin = null;
        GUISkin _original = null;
        Color _titleColor;
        #endregion

        SerializedProperty syncCrossScenes;
        SerializedProperty holder;
        SerializedProperty dropPrefab;
        SerializedProperty prefabName;
        SerializedProperty dropPoint;

        private void OnEnable()
        {
            // Load Skin for reverence
            if (!_skin) _skin = E_Helpers.LoadSkin(E_Core.e_guiSkinPath);

            //Load all images
            _titleColor = new Color32(1, 9, 28, 255); //dark blue

            syncCrossScenes = serializedObject.FindProperty("syncCrossScenes");
            holder = serializedObject.FindProperty("holder");

            dropPrefab = serializedObject.FindProperty("dropPrefab");
            prefabName = serializedObject.FindProperty("prefabName");
            dropPoint = serializedObject.FindProperty("dropPoint");
        }

        public override void OnInspectorGUI()
        {
            #region Core
            // Core Requirements
            serializedObject.Update();
            NetworkBreakObject sp = (NetworkBreakObject)target;
            var rect = GUILayoutUtility.GetRect(1, 1);

            //Apply the gui skin
            _original = GUI.skin;
            GUI.skin = _skin;

            //Draw Background Box
            GUILayout.BeginHorizontal("box", GUILayout.ExpandHeight(false));

            GUILayout.BeginVertical(GUILayout.ExpandHeight(false));

            // Title
            EditorGUI.DrawRect(new Rect(rect.x + 5, rect.y + 10, rect.width - 10, 40), _titleColor);
            GUI.DrawTexture(new Rect(rect.x + 10, rect.y + 15, 30, 30), E_Helpers.LoadTexture(E_Core.h_genericIcon, new Vector2(256, 256)));
            GUILayout.Space(5);
            GUILayout.Label("Network Break Object", _skin.GetStyle("Label"));
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Component that offers an RPC call \"BreakObject\" that will sync the break object action across the network using the attached PhotonView. On your \"vBreakableObject\" event, add the \"NetworkBreakObject.BreakObject\" to trigger breaking this object across the network.", MessageType.Info);
            #endregion

            EditorGUILayout.PropertyField(syncCrossScenes);
            if (syncCrossScenes.boolValue == true)
            {
                EditorGUILayout.PropertyField(holder);
            }
            EditorGUILayout.PropertyField(dropPrefab);
            if (dropPrefab.boolValue == true)
            {
                EditorGUILayout.PropertyField(prefabName);
                EditorGUILayout.PropertyField(dropPoint);
            }

            DrawPropertiesExcluding(serializedObject, E_Helpers.EditorGetVariables(typeof(NetworkBreakObject)));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }
    }
}