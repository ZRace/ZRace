using UnityEditor;
using UnityEngine;
using CBGames.Core;
using CBGames.Editors;
using CBGames.Objects;

namespace CBGames.Inspector
{
    [CustomEditor(typeof(SyncObject), true)]
    public class SyncObjectInspector : Editor
    {
        #region CustomEditorVariables
        GUISkin _skin = null;
        GUISkin _original = null;
        Color _titleColor;
        #endregion

        #region Properties
        SerializedProperty view;
        SerializedProperty syncEnable;
        SerializedProperty syncDisable;
        SerializedProperty syncDestroy;
        SerializedProperty syncImmediateChildren;
        SerializedProperty isLeftHanded;
        SerializedProperty isWeaponHolder;
        SerializedProperty debugging;
        #endregion

        private void OnEnable()
        {
            // Load Skin for reverence
            if (!_skin) _skin = E_Helpers.LoadSkin(E_Core.e_guiSkinPath);

            //Load all images
            _titleColor = new Color32(1, 9, 28, 255); //dark blue

            //Fields
            view = serializedObject.FindProperty("view");
            syncEnable = serializedObject.FindProperty("syncEnable");
            syncDisable = serializedObject.FindProperty("syncDisable");
            syncDestroy = serializedObject.FindProperty("syncDestroy");
            syncImmediateChildren = serializedObject.FindProperty("syncImmediateChildren");
            isLeftHanded = serializedObject.FindProperty("isLeftHanded");
            isWeaponHolder = serializedObject.FindProperty("isWeaponHolder");
            debugging = serializedObject.FindProperty("debugging");
        }
        public override void OnInspectorGUI()
        {
            #region Core
            // Core Requirements
            serializedObject.Update();
            SyncObject nm = (SyncObject)target;
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
            GUILayout.Label("Network Sync Object", _skin.GetStyle("Label"));
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Component used to sync actions that happen to this object across the network. Objects must be instantiated with this component, cannot be added a runtime. This is generally used for player equipment.", MessageType.Info);
            #endregion

            //Properties
            GUILayout.BeginHorizontal("box", GUILayout.ExpandHeight(false));
            GUILayout.BeginVertical(GUILayout.ExpandHeight(false));
            GUILayout.Label("PhotonView For RPC Calls (Optional)", _skin.box);
            EditorGUILayout.PropertyField(view);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal("box", GUILayout.ExpandHeight(false));
            GUILayout.BeginVertical(GUILayout.ExpandHeight(false));
            GUILayout.Label("Sync Options", _skin.box);
            EditorGUILayout.PropertyField(syncEnable);
            EditorGUILayout.PropertyField(syncDisable);
            EditorGUILayout.PropertyField(syncDestroy);
            EditorGUILayout.PropertyField(syncImmediateChildren);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            if (syncImmediateChildren.boolValue == true)
            {
                GUILayout.BeginHorizontal("box", GUILayout.ExpandHeight(false));
                GUILayout.BeginVertical(GUILayout.ExpandHeight(false));
                GUILayout.Label("Instantiation Options", _skin.box);
                EditorGUILayout.PropertyField(isLeftHanded);
                EditorGUILayout.PropertyField(isWeaponHolder);
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            }

            GUILayout.BeginHorizontal("box", GUILayout.ExpandHeight(false));
            GUILayout.BeginVertical(GUILayout.ExpandHeight(false));
            EditorGUILayout.PropertyField(debugging);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            DrawPropertiesExcluding(serializedObject, E_Helpers.EditorGetVariables(typeof(SyncObject)));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }
    }
}