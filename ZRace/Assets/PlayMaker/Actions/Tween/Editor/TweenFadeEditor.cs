// (c) Copyright HutongGames, all rights reserved.
// See also: EasingFunctionLicense.txt

using HutongGames.PlayMaker.Actions;
using UnityEditor;

namespace HutongGames.PlayMakerEditor
{
    [CustomActionEditor(typeof(PlayMaker.Actions.TweenFade))]
    public class TweenFadeEditor : TweenEditorBase
    {
        public override bool OnGUI()
        {
            EditorGUI.BeginChangeCheck();

            EditField("gameObject");
            EditField("tweenDirection", "Fade");
            EditField("value");

            DoEasingUI();

            return EditorGUI.EndChangeCheck();
        }


    }
}