using UnityEngine;
using UnityEditor;
using CBGames.Editors;
using CBGames.UI;

namespace CBGames.Inspector
{
    [CustomEditor(typeof(TrackRandomSceneList), true)]
    public class TrackRandomSceneListInspector : Editor
    {
        #region Core
        GUISkin _skin = null;
        GUISkin _original = null;
        Color _titleColor;
        #endregion

        #region Properties
        SerializedProperty indexNumberToTrack;
        SerializedProperty texts;
        SerializedProperty images;
        #endregion

        private void OnEnable()
        {
            // Load Skin for reverence
            if (!_skin) _skin = E_Helpers.LoadSkin(E_Core.e_guiSkinPath);

            //Load all images
            _titleColor = new Color32(1, 9, 28, 255); //dark blue

            #region Properties
            indexNumberToTrack = serializedObject.FindProperty("indexNumberToTrack");
            texts = serializedObject.FindProperty("texts");
            images = serializedObject.FindProperty("images");
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
            GUILayout.Label("Track Random Scene List", _skin.GetStyle("Label"));
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("This component requires a \"UICoreLogic\" component somewhere in the scene. \n\n" +
                "This component will track the randomly generated scene list (from the original specified scenelist in " +
                "the UICoreLogic component) and display the image/scene name at the specified index. I will overwrite " +
                "the images sprite and texts text to be that is at the specified index of the random version.", MessageType.Info);
            #endregion

            #region Properties
            EditorGUILayout.PropertyField(indexNumberToTrack);
            EditorGUILayout.PropertyField(images, true);
            EditorGUILayout.PropertyField(texts, true);
            #endregion

            #region Core
            DrawPropertiesExcluding(serializedObject, E_Helpers.EditorGetVariables(typeof(TrackRandomSceneList)));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
            #endregion
        }
    }
}