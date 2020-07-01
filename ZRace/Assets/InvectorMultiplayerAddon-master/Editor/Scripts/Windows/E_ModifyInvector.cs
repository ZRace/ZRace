using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace CBGames.Editors
{
    public class E_ModifyInvector : EditorWindow
    {
        [MenuItem("CB Games/Convert/Invector Files", false, 0)]
        public static void CB_MENU_ModifyInvectorFiles()
        {
            CB_ModifyInvectorFiles(false);
        }
        [MenuItem("Window/Reset/Invector Files")]
        public static void CB_MENU_ResetInvectorFiles()
        {
            CB_ModifyInvectorFiles(true);
        }

        public static void CB_ModifyInvectorFiles(bool disable_files = false)
        {
            List<E_Helpers.CB_Additions> modifications = new List<E_Helpers.CB_Additions>();

            #region HeadTrack
            modifications.Add(new E_Helpers.CB_Additions(
                in_target: "private float _currentHeadWeight, _currentbodyWeight;",
                in_add: "protected float _currentHeadWeight, _currentbodyWeight;",
                in_nextline: "",
                in_type: E_Helpers.CB_FileAddtionType.Replace
            ));
            E_Helpers.ModifyFile(@"Assets/Invector-3rdPersonController/Basic Locomotion/Scripts/HeadTrack/Scripts/vHeadTrack.cs", modifications);
            Debug.Log("Modified file at: Assets/Invector-3rdPersonController/Basic Locomotion/Scripts/HeadTrack/Scripts/vHeadTrack.cs");
            #endregion

            #region vItemManagerUtilities_Shooter
            modifications.Clear();
            modifications.Add(new E_Helpers.CB_Additions(
                in_target: "if (equipPointL.onInstantiateEquiment.GetPersistentTarget(i).GetType().Equals(typeof(vShooterManager)) && equipPointL.onInstantiateEquiment.GetPersistentMethodName(i).Equals(\"SetLeftWeapon\"))",
                in_add: "if ((equipPointL.onInstantiateEquiment.GetPersistentTarget(i).GetType().Equals(typeof(vShooterManager)) || equipPointL.onInstantiateEquiment.GetPersistentTarget(i).GetType().IsSubclassOf(typeof(vShooterManager))) && equipPointL.onInstantiateEquiment.GetPersistentMethodName(i).Equals(\"SetLeftWeapon\"))",
                in_nextline: "",
                in_type: E_Helpers.CB_FileAddtionType.Replace
            ));
            modifications.Add(new E_Helpers.CB_Additions(
                in_target: "if (equipPointR.onInstantiateEquiment.GetPersistentTarget(i).GetType().Equals(typeof(vShooterManager)) && equipPointR.onInstantiateEquiment.GetPersistentMethodName(i).Equals(\"SetRightWeapon\"))",
                in_add: "if ((equipPointR.onInstantiateEquiment.GetPersistentTarget(i).GetType().Equals(typeof(vShooterManager)) || equipPointR.onInstantiateEquiment.GetPersistentTarget(i).GetType().IsSubclassOf(typeof(vShooterManager))) && equipPointR.onInstantiateEquiment.GetPersistentMethodName(i).Equals(\"SetRightWeapon\"))",
                in_nextline: "",
                in_type: E_Helpers.CB_FileAddtionType.Replace
            ));
            E_Helpers.ModifyFile(@"Assets/Invector-3rdPersonController/Shooter/Scripts/Shooter/Editor/vItemManagerUtilities_Shooter.cs", modifications);
            Debug.Log("Modified file at: Assets/Invector-3rdPersonController/Shooter/Scripts/Shooter/Editor/vItemManagerUtilities_Shooter.cs");
            #endregion

            #region E_ConvertPlayer
            E_Helpers.CommentOutRegionInFile("InvectorMultiplayer/Editor/Scripts/Windows/ConvertPlayer/E_ConvertPlayer.cs", "InvectorModification", disable_files);
            #endregion

            #region MP_HeadTrack
            string results = E_Helpers.CommentOutFile("InvectorMultiplayer/Scripts/Player/Basic/MP_HeadTrack.cs", disable_files);
            Debug.Log(results);
            #endregion

            #region vGenericAction
            modifications.Clear();
            modifications.Add(new E_Helpers.CB_Additions(
                in_target: "private float animationBehaviourDelay;",
                in_add: "protected float animationBehaviourDelay;",
                in_nextline: "",
                in_type: E_Helpers.CB_FileAddtionType.Replace
            ));
            E_Helpers.ModifyFile(@"Assets/Invector-3rdPersonController/Basic Locomotion/Scripts/CharacterController/Actions/vGenericAction.cs", modifications);
            Debug.Log("Modified file at: Assets/Invector-3rdPersonController/Basic Locomotion/Scripts/CharacterController/Actions/vGenericAction.cs");
            #endregion

            #region MP_vGenericActions
            results = E_Helpers.CommentOutFile("InvectorMultiplayer/Scripts/Player/Basic/MP_vGenericAction.cs", disable_files);
            Debug.Log(results);
            #endregion

            #region E_InvectorConvertPlayer
            results = E_Helpers.CommentOutFile("InvectorMultiplayer/Editor/Scripts/Windows/ConvertPlayer/E_InvectorConvertPlayer.cs", disable_files);
            Debug.Log(results);
            #endregion

            #region E_InvectorTests
            results = E_Helpers.CommentOutFile("InvectorMultiplayer/Editor/Scripts/Windows/SceneTests/E_InvectorTests.cs", disable_files);
            Debug.Log(results);
            #endregion

            if (results.Contains("Success"))
            {
                if (EditorUtility.DisplayDialog("Success!",
                            "You have successfully modified the invector files. You may have to click out of unity and back in (or close it " +
                            "and open it again) to re-compile the scripts. If you see the little gear icon in the lower right corner spin " +
                            "that means that unity is compiling.",
                                        "Ok, thanks"))
                { }
            }
        }
    }
}