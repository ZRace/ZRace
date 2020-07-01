using UnityEditor;
using UnityEngine;
using CBGames.Editors;
using CBGames.Objects;

namespace CBGames.Inspector
{
    [CustomEditor(typeof(SyncItemCollection), true)]
    public class SyncItemCollectionInspector : Editor
    {
        #region CustomEditorVariables
        GUISkin _skin = null;
        GUISkin _original = null;
        Color _titleColor;
        #endregion

        #region Properties
        SerializedProperty syncCrossScenes;
        SerializedProperty syncCreateDestroy;
        SerializedProperty holder;
        SerializedProperty resourcesPrefab;
        SerializedProperty skipStartCheck;
        SerializedProperty onPressActionDelay;
        SerializedProperty OnPressActionInput;
        SerializedProperty onPressActionInputWithTarget;
        SerializedProperty OnSceneEnterUpdate;
        #endregion

        private void OnEnable()
        {
            if (!_skin) _skin = E_Helpers.LoadSkin(E_Core.e_guiSkinPath);
            _titleColor = new Color32(1, 9, 28, 255); //dark blue

            //Fields
            syncCrossScenes = serializedObject.FindProperty("syncCrossScenes");
            syncCreateDestroy = serializedObject.FindProperty("syncCreateDestroy");
            holder = serializedObject.FindProperty("holder");
            resourcesPrefab = serializedObject.FindProperty("resourcesPrefab");
            skipStartCheck = serializedObject.FindProperty("skipStartCheck");
            onPressActionDelay = serializedObject.FindProperty("onPressActionDelay");
            OnPressActionInput = serializedObject.FindProperty("OnPressActionInput");
            onPressActionInputWithTarget = serializedObject.FindProperty("onPressActionInputWithTarget");
            OnSceneEnterUpdate = serializedObject.FindProperty("OnSceneEnterUpdate");
        }
        public override void OnInspectorGUI()
        {
            #region Core
            // Core Requirements
            serializedObject.Update();
            SyncItemCollection sic = (SyncItemCollection)target;
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
            GUILayout.Label("Sync Item Collection", _skin.GetStyle("Label"));
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Used to sync events from collecting this item across the network. \n\n"+
                "Required Setup Actions:\n"+
                "1. Copy the settings on the following fields from \"vItemCollection\" to this component:\n"+
                " * \"OnPressActionDelay\"\n"+
                " * \"OnPressActionInput\"\n"+
                " * \"OnPressActionInputWithTarget\"\n\n"+
                "2. On \"vItemCollection\" do the following:\n" +
                " * Set \"OnPressActionDelay\" to zero\n"+
                " * Remove all events from \"OnPressActionInput\"\n"+
                " * Remove all events from \"OnPressActionInputWithTarget\"\n", MessageType.Info);
            #endregion

            //Properties
            GUILayout.BeginHorizontal(GUI.skin.box, GUILayout.ExpandHeight(false));
            GUILayout.BeginVertical(GUILayout.ExpandHeight(false));
            EditorGUILayout.PropertyField(syncCrossScenes);
            if (syncCrossScenes.boolValue == true)
            {
                EditorGUILayout.PropertyField(holder, new GUIContent("Track Position"));
                EditorGUILayout.PropertyField(syncCreateDestroy, new GUIContent("Is Dynamic Obj"));
                if (syncCreateDestroy.boolValue == true)
                {
                    EditorGUILayout.PropertyField(resourcesPrefab);
                }
            }
            EditorGUILayout.PropertyField(skipStartCheck, new GUIContent("Items In ItemCollection"));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal(GUI.skin.box, GUILayout.ExpandHeight(false));
            GUILayout.BeginVertical(GUILayout.ExpandHeight(false));
            EditorGUILayout.PropertyField(onPressActionDelay);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal(GUI.skin.box, GUILayout.ExpandHeight(false));
            GUILayout.BeginVertical(GUILayout.ExpandHeight(false));
            GUI.skin = _original;
            EditorGUILayout.HelpBox("These should be copied exactly from the vItemColletion component.", MessageType.None);
            EditorGUILayout.PropertyField(OnPressActionInput);
            EditorGUILayout.PropertyField(onPressActionInputWithTarget);

            EditorGUILayout.HelpBox("This is called when this object was actived by another player " +
                "when previously in another scene. When you enter this scene this is called.", MessageType.None);
            EditorGUILayout.PropertyField(OnSceneEnterUpdate);

            GUI.skin = _skin;
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            DrawPropertiesExcluding(serializedObject, E_Helpers.EditorGetVariables(typeof(SyncItemCollection)));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
