using UnityEditor;
using UnityEngine;
using CBGames.Core;
using CBGames.Editors;
using CBGames.Player;

namespace CBGames.Inspector
{
    [CustomEditor(typeof(PlayerRespawn), true)]
    public class PlayerRespawnInspector : Editor
    {
        #region CoreVariables
        GUISkin _skin = null;
        GUISkin _original = null;
        Color _titleColor;
        GUIContent eventContent;
        #endregion

        #region Properties
        SerializedProperty respawnDelay;
        SerializedProperty visualCountdown;
        SerializedProperty respawnType;
        SerializedProperty respawnPoint;
        SerializedProperty broadcastDeathMessage;
        SerializedProperty deathMessage;
        SerializedProperty teams;
        SerializedProperty debugging;
        #endregion

        private void OnEnable()
        {
            #region Properties
            respawnDelay = serializedObject.FindProperty("respawnDelay");
            visualCountdown = serializedObject.FindProperty("visualCountdown");
            respawnType = serializedObject.FindProperty("respawnType");
            respawnPoint = serializedObject.FindProperty("respawnPoint");
            broadcastDeathMessage = serializedObject.FindProperty("broadcastDeathMessage");
            deathMessage = serializedObject.FindProperty("deathMessage");
            teams = serializedObject.FindProperty("teams");
            debugging = serializedObject.FindProperty("debugging");
            #endregion

            #region Core
            // Load Skin for reverence
            if (!_skin) _skin = E_Helpers.LoadSkin(E_Core.e_guiSkinPath);

            //Load all images
            _titleColor = new Color32(1, 9, 28, 255); //dark blue
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
            GUI.DrawTexture(new Rect(rect.x + 10, rect.y + 15, 30, 30), E_Helpers.LoadTexture(E_Core.h_respawnPointIcon, new Vector2(256, 256)));
            GUILayout.Space(5);
            GUILayout.Label("Network Respawning", _skin.GetStyle("Label"));
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Call \"Respawn\" function on this component to respawn the player across the network. \n\n" +
                "Best if called from the \"Respawn\" function found in  the \"Sync Player\" component. " +
                "Can have the \"OnDead\" event on the \"vThirdPersonController\" component call the " +
                "\"Respawn\" function of the \"SyncPlayer\" component. \n\n" +
                "Will respawn the player (destroying the old one) based on the settings here. Will respawn " +
                "the player with max health but all the previous settings that were originally on the player, " +
                "including their inventory settings.", MessageType.Info);
            #endregion

            #region Properties
            if (E_Helpers.InspectorTagExists("RespawnPoint") == false)
            {
                EditorGUILayout.HelpBox("There is no tag\"RespawnPoint\" in this project! \n\n" +
                    "To add a new object that is tagged with \"RespawnPoint\" go to CB Games > Network Manager > Respawn > Add Respawn Point", MessageType.Error);
            }
            else if(GameObject.FindGameObjectWithTag("RespawnPoint") == null)
            {
                EditorGUILayout.HelpBox("There is no gameobject with the \"RespawnPoint\" tag in this scene! \n\n" +
                    "To add a new object that is tagged with \"RespawnPoint\" go to CB Games > Network Manager > Respawn > Add Respawn Point", MessageType.Error);
            }
            else
            { 
                EditorGUILayout.PropertyField(respawnDelay);
                EditorGUILayout.PropertyField(visualCountdown);
                GUILayout.Space(10);
                EditorGUILayout.PropertyField(respawnType);
                if (respawnType.enumValueIndex == 1)
                {
                    EditorGUILayout.PropertyField(respawnPoint);
                    if (respawnPoint.objectReferenceValue == null)
                    {
                        EditorGUILayout.HelpBox("There must always be a transform here otherwise this player will not respawn correctly.", MessageType.Error);
                    }
                }
                if (respawnType.enumValueIndex == 3)
                {
                    EditorGUILayout.PropertyField(teams, true);
                }
                GUILayout.Space(10);
                EditorGUILayout.PropertyField(broadcastDeathMessage);
                if (broadcastDeathMessage.boolValue == true)
                {
                    EditorGUILayout.PropertyField(deathMessage);
                }
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(debugging);
            }
            #endregion

            #region Core
            DrawPropertiesExcluding(serializedObject, E_Helpers.EditorGetVariables(typeof(PlayerRespawn)));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
            #endregion
        }
    }
}