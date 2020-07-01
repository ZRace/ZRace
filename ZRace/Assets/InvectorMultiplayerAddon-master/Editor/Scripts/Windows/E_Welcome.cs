using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using marijnz.EditorCoroutines;

namespace CBGames.Editors
{
    public class E_Welcome : EditorWindow
    {
        #region Core Variables
        Vector2 _minrect = new Vector2(500, 250);
        Vector2 _maxrect = new Vector2(500, 250);
        #endregion

        private const string k_ProjectOpened = "ProjectOpened";

        [InitializeOnLoadMethod]
        public static void DisplayWelcomeScreneOnLoad()
        {
            if (!SessionState.GetBool(k_ProjectOpened, false))
            {
                SessionState.SetBool(k_ProjectOpened, true);
                CB_ConvertPrefabs();
            }
        }

        [MenuItem("CB Games/Welcome Page", false, 300)]
        public static void CB_ConvertPrefabs()
        {
            GetWindow<E_Welcome>("Welcome To The Invector Multiplayer Addon!");
        }

        private void OnEnable()
        {
            //Force window size
            this.minSize = _minrect;
            this.maxSize = _maxrect;

            //Make window title
            this.titleContent = new GUIContent("Welcome!", null, "Welcome and getting started page.");
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Welcome To The Invector Multiplayer Addon", GUI.skin.GetStyle("Label"));
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal(GUI.skin.box, GUILayout.ExpandHeight(true));
            EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));
            EditorGUILayout.LabelField("How Do I Use This Addon?", GUI.skin.label);
            EditorGUILayout.LabelField("First wait for this package to finish importing...", GUI.skin.GetStyle("Label"));
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("1. Open a scene you want to convert to support multiplayer.", GUI.skin.textArea);
            EditorGUILayout.LabelField("2. Select the new toolbar menu item added to the unity edtior: \nCB Games > Main Menu", GUI.skin.textArea);
            EditorGUILayout.LabelField("3. Follow the detailed instructions on the main menu and every selected sub menu item.", GUI.skin.textArea);
            EditorGUILayout.LabelField("4. After all of that be sure to look at the other cool things to add under the CB Games Menu! " +
                "EX: Text Chat, Voice Chat, Lobby Camera, Scene Transitions, etc.", GUI.skin.textArea);
            EditorGUILayout.LabelField("5. If you need more help follow the YouTube tutorials posted at the \"Cyber Bullet Games\" channel by " +
                "clicking on CB Games > YouTube Tutorials.", GUI.skin.textArea);
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
    }
}