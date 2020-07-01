using UnityEngine;
using UnityEditor;
using marijnz.EditorCoroutines;
using System.Collections;
using CBGames.Core;
using CBGames.Player;
using System.Collections.Generic;
using System.Reflection;
using System;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using Invector.vItemManager;
using Invector.vMelee;
using Invector;
using CBGames.Objects;
using Invector.vCharacterController.AI;
using Invector.vCamera;
using UnityEngine.SceneManagement;
using CBGames.UI;

namespace CBGames.Editors
{
    public class DebugFormat
    {
        public string message;
        public UnityEngine.Object context;
        public DebugFormat(string Message, UnityEngine.Object Context)
        {
            message = Message;
            context = Context;
        }
    }

    public partial class E_TestScene : EditorWindow
    {
        
        #region Editor Variables
        GUISkin _skin = null;
        Color _titleColor;
        bool _runningTests = false;
        Color _lockColor;
        Color _convertBar;
        bool _ranTests = false;
        #endregion

        #region Partial Methods

        #region Shooter
        partial void SHOOTER_PerformvShooterManagerTests(ref int passed, ref int failed, ref List<DebugFormat> failures, ref List<DebugFormat> warnings);
        partial void SHOOTER_PerformItemCollectionTests(ref int passing, ref int failed, ref List<DebugFormat> failures, ref List<DebugFormat> warnings);
        partial void SHOOTER_PerformvShooterWeaponPrefabTests(ref int passing, ref int failed, ref List<DebugFormat> failures, ref List<DebugFormat> warnings);
        partial void SHOOTER_PerformvThrowCollectable(ref int passing, ref int failed, ref List<DebugFormat> failures, ref List<DebugFormat> warnings);
        partial void SHOOTER_PerformvLockOnShooterTests(ref int passed, ref int failed, ref List<DebugFormat> failures, ref List<DebugFormat> warnings);
        partial void SHOOTER_PerformItemListDataTests(ref int passed, ref int failed, ref List<DebugFormat> failures, ref List<DebugFormat> warnings);
        partial void SHOOTER_PerformvShooterMeleeInputTests(ref int passed, ref int failed, ref List<DebugFormat> failures, ref List<DebugFormat> warnings);
        #endregion

        #region AI
        partial void AI_GenericSyncTests(ref int passed, ref int failed, ref List<DebugFormat> failures, ref List<DebugFormat> warnings);
        partial void AI_ShooterManagerTests(ref int passed, ref int failed, ref List<DebugFormat> failures, ref List<DebugFormat> warnings);
        partial void AI_ControlAIShooterTests(ref int passed, ref int failed, ref List<DebugFormat> failures, ref List<DebugFormat> warnings);
        partial void AI_MPAITests(ref int passed, ref int failed, ref List<DebugFormat> failures, ref List<DebugFormat> warnings);
        partial void AI_AIHeadTrackTests(ref int passed, ref int failed, ref List<DebugFormat> failures, ref List<DebugFormat> warnings);
        #endregion

        #region Invector Modified Files
        partial void CORE_PerformMPHeadTrackTests(ref int passed, ref int failed, ref List<DebugFormat> failures, ref List<DebugFormat> warnings);
        #endregion
        #endregion

        #region Visual Window
        [MenuItem("CB Games/Testing/Perform Scene Tests", false, 0)]
        public static void CB_TestScene()
        {
            EditorWindow window = GetWindow<E_TestScene>("Core Objects");
            window.maxSize = new Vector2(500, 280);
            window.minSize = window.maxSize;
        }
        private void OnEnable()
        {
            if (!_skin) _skin = E_Helpers.LoadSkin(E_Core.e_guiSkinPath);

            //Set title bar colors
            _titleColor = new Color32(1, 9, 28, 255); //dark blue
            
            //Make window title
            this.titleContent = new GUIContent("Add Core Objects", null, "Adds the \"Core\" objects to the scene.");
        }
        private void OnGUI()
        {
            _lockColor = new Color32(158, 158, 158, 200);
            _convertBar = new Color32(95, 165, 245, 255);

            //Apply the gui skin
            GUI.skin = _skin;

            //Draw title bar
            EditorGUI.DrawRect(new Rect(5, 5, position.width - 10, 40), _titleColor);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Perform Scene Tests", _skin.GetStyle("Label"));
            EditorGUILayout.Space();

            //Draw Body
            EditorGUILayout.BeginHorizontal(_skin.box, GUILayout.ExpandHeight(false));
            EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(false));

            //Draw Helpful Text
            EditorGUILayout.LabelField("This will scan the entire scene. It will gather all the objects with " +
                "converted components then look at each component to make sure there are no " +
                "errors currently on them. Next it will make sure your network manager is setup " +
                "correctly and doesn't have any missing options. Finally after performing these and " +
                "a few other tests it will output the results for you to see.", _skin.GetStyle("TextField"));

            EditorGUILayout.BeginHorizontal(_skin.box, GUILayout.ExpandHeight(false));
            EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(false));
            EditorGUILayout.LabelField("NOTE: This does not make any changes, simply outputs a "+
                "log for you to review. To see the tests results look in the console window.", _skin.GetStyle("TextField"));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            //Draw test button
            if (GUI.Button(new Rect(30, 230, 430, 30), "Perform Tests Now!", _skin.GetStyle("Button")))
            {
                if (_runningTests == false)
                {
                    this.StartCoroutine(PerformTests());
                }
            }
            if (_runningTests == true)
            {
                EditorGUI.DrawRect(new Rect(0, 50, position.width, position.height - 30), _lockColor);
                EditorGUI.DrawRect(new Rect(0, position.height / 2 - 50, position.width, 100), _convertBar);
                EditorGUI.LabelField(new Rect(0, position.height / 2 - 50, position.width, 100), "Performing Tests, please wait...", _skin.GetStyle("Label"));
                EditorGUI.LabelField(new Rect(100, position.height / 2 + 10, position.width, 100), "Important Note: You can refresh this window by clicking into it.", _skin.GetStyle("TextField"));
            }
            if (_ranTests == true)
            {
                EditorGUILayout.BeginHorizontal(_skin.box, GUILayout.Height(40));
                EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(false));
                EditorGUI.LabelField(new Rect(90, 180, position.width - 15, 160), "-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-", _skin.GetStyle("TextField"));
                EditorGUI.LabelField(new Rect(70, 190, position.width - 15, 160), "Finished running all tests. Look in the console for the test results.", _skin.GetStyle("TextField"));
                EditorGUI.LabelField(new Rect(90, 200, position.width - 15, 160), "-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-", _skin.GetStyle("TextField"));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
        }
        #endregion

        string[] GetResourcePrefabs()
        {
            string[] temp = AssetDatabase.GetAllAssetPaths();
            List<string> result = new List<string>();
            foreach (string s in temp)
            {
                if (s.Contains(".prefab"))
                {
                    if (s.Contains("Assets/Resources"))
                    {
                        result.Add(s);
                    }
                }
            }
            return result.ToArray();
        }

        #region Running Order / Output Logic
        IEnumerator PerformTests()
        {
            _runningTests = true;

            int failed = 0;
            int passing = 0;
            List<DebugFormat> failures = new List<DebugFormat>();
            List<DebugFormat> warnings = new List<DebugFormat>();

            //Check for enabled packages
            SHOOTER_CheckAddonEnabled();

            //Run Tests
            CORE_PerformPreviewCamTests(ref passing, ref failed, ref failures, ref warnings);
            CORE_PerformNetworkManagerTests(ref passing, ref failed, ref failures, ref warnings);
            CORE_PerformSyncPlayerTests(ref passing, ref failed, ref failures, ref warnings);
            CORE_SpawnPointTesting(ref passing, ref failed, ref failures, ref warnings);
            CORE_DisablePlayersTest(ref passing, ref failed, ref failures, ref warnings);
            CORE_PerformThirdPersonControllerTests(ref passing, ref failed, ref failures, ref warnings);
            CORE_PerformMeleeCombatInputTests(ref passing, ref failed, ref failures, ref warnings);
            CORE_PerformGenericActionTests(ref passing, ref failed, ref failures, ref warnings);
            CORE_PerformLadderActionTests(ref passing, ref failed, ref failures, ref warnings);
            CORE_PerformItemManagerTests(ref passing, ref failed, ref failures, ref warnings);
            CORE_PerformMeleeManagerTests(ref passing, ref failed, ref failures, ref warnings);
            CORE_PerformHealthControllerTests(ref passing, ref failed, ref failures, ref warnings);
            CORE_PerformBreakableObjectTests(ref passing, ref failed, ref failures, ref warnings);
            CORE_PerformItemCollectionTests(ref passing, ref failed, ref failures, ref warnings);
            CORE_PerformPlayerNameBarTests(ref passing, ref failed, ref failures, ref warnings);
            CORE_PerformSyncObjectTests(ref passing, ref failed, ref failures, ref warnings);
            CORE_MissingRequiredComponents(ref passing, ref failed, ref failures, ref warnings);
            CORE_SceneInBuildScenes(ref passing, ref failed, ref failures, ref warnings);
            CORE_PerformChatBoxTests(ref passing, ref failed, ref failures, ref warnings);
            CORE_PerformSceneTransitionTests(ref passing, ref failed, ref failures, ref warnings);
            CORE_PerformVoiceChatTests(ref passing, ref failed, ref failures, ref warnings);
            CORE_PerformCallNetworkEventsTests(ref passing, ref failed, ref failures, ref warnings);
            CORE_PerformPlayerRespawnTests(ref passing, ref failed, ref failures, ref warnings);
            CORE_PerformPlayerListUITests(ref passing, ref failed, ref failures, ref warnings);
            CORE_PerformUICoreLogicTests(ref passing, ref failed, ref failures, ref warnings);
            CORE_PerformvInventoryTests(ref passing, ref failed, ref failures, ref warnings);
            CORE_PerformSetLoadingScreenTests(ref passing, ref failed, ref failures, ref warnings);
            CORE_PerformPhotonViewTests(ref passing, ref failed, ref failures, ref warnings);
            CORE_PerformItemListDataTests(ref passing, ref failed, ref failures, ref warnings);
            CORE_PerformMPHeadTrackTests(ref passing, ref failed, ref failures, ref warnings);

            SHOOTER_PerformItemCollectionTests(ref passing, ref failed, ref failures, ref warnings);
            SHOOTER_PerformvShooterWeaponPrefabTests(ref passing, ref failed, ref failures, ref warnings);
            SHOOTER_PerformvThrowCollectable(ref passing, ref failed, ref failures, ref warnings);
            SHOOTER_PerformItemListDataTests(ref passing, ref failed, ref failures, ref warnings);
            SHOOTER_PerformvShooterManagerTests(ref passing, ref failed, ref failures, ref warnings);
            SHOOTER_PerformvShooterMeleeInputTests(ref passing, ref failed, ref failures, ref warnings);

            AI_GenericSyncTests(ref passing, ref failed, ref failures, ref warnings);
            AI_ShooterManagerTests(ref passing, ref failed, ref failures, ref warnings);
            AI_ControlAIShooterTests(ref passing, ref failed, ref failures, ref warnings);
            AI_MPAITests(ref passing, ref failed, ref failures, ref warnings);
            AI_AIHeadTrackTests(ref passing, ref failed, ref failures, ref warnings);

            //Display Results
            for (int i = 0; i < warnings.Count; i++)
            {
                Debug.LogWarning(warnings[i].message, warnings[i].context);
            }
            for(int i = 0; i < failures.Count; i++)
            {
                Debug.LogError(failures[i].message, failures[i].context);
            }
            Debug.Log("PASSED: " + passing + " FAILED: " + failed);

            _runningTests = false;
            _ranTests = true;
            yield return null;
        }
        #endregion
    }
}
