#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace BrainFailProductions.PolyFew
{
    public class PolyfewMenu : MonoBehaviour
    {


        [MenuItem("Window/Brainfail Products/PolyFew/Enable Auto UI Attaching", false, 0)]
        static void EnableAutoUIAttaching()
        {
            //EditorPrefs.DeleteKey("polyfewAutoAttach");return;
            EditorPrefs.SetBool("polyfewAutoAttach", true);
            InspectorAttacher.AttachInspector();
        }

        
        [MenuItem("Window/Brainfail Products/PolyFew/Disable Auto UI Attaching", false, 1)]
        static void DisableAutoUIAttaching()
        {
            EditorPrefs.SetBool("polyfewAutoAttach", false);
        }


        [MenuItem("Window/Brainfail Products/PolyFew/Attach PolyFew to Object", false, 2)]
        static void AttachPolyFewToObject()
        {
            EditorPrefs.SetBool("polyfewAutoAttach", false);
            InspectorAttacher.AttachInspector();
        }


        public static bool IsAutoAttachEnabled()
        {
            bool isAutoAttach;

            if (!EditorPrefs.HasKey("polyfewAutoAttach"))
            {
                EditorPrefs.SetBool("polyfewAutoAttach", true);
                isAutoAttach = true;
            }
            else
            {
                isAutoAttach = EditorPrefs.GetBool("polyfewAutoAttach");
            }

            return isAutoAttach;
        }


        #region VALIDATORS

        [MenuItem("Window/Brainfail Products/PolyFew/Enable Auto UI Attaching", true)]
        static bool CheckEnableAttachingButton()
        {
            bool isAutoAttach = IsAutoAttachEnabled();

            if (isAutoAttach) { return false; }
            else { return true; }
        }

        [MenuItem("Window/Brainfail Products/PolyFew/Disable Auto UI Attaching", true)]
        static bool CheckDisableAttachingButton()
        {
            bool isEnableButtOn  = CheckEnableAttachingButton();

            if (isEnableButtOn) { return false; }
            else { return true; }
        }

        #endregion VALIDATORS
    }

}

#endif
