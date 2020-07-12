using UnityEngine;
using UnityEditor;
using CBGames.Editors;

[CustomEditor(typeof(HealthColorAnimation), true)]
[CanEditMultipleObjects]

public class HealthColorAnimationEditor : Editor
{
	#region Color Property
	SerializedProperty colorHealthHigh, colorHealthMidHigh, colorHealthMid, colorHealthMidLow,
		colorHealthLow;
	#endregion
	#region Float Property
	SerializedProperty valueHealthHigh, valueHealthMidHigh, valueHealthMid, valueHealthMidLow,
		valueHealthLow;
	#endregion

	SerializedProperty healthValue, imageColor;

	GUISkin _skin = null;
	GUISkin _original = null;
	Color _titleColor;

	private void OnEnable()
	{
		if (!_skin) _skin = E_Helpers.LoadSkin(E_Core.e_guiSkinPathZRace);
		colorHealthHigh = serializedObject.FindProperty("colorHealthHigh");
		colorHealthMidHigh = serializedObject.FindProperty("colorHealthMidHigh");
		colorHealthMid = serializedObject.FindProperty("colorHealthMid");
		colorHealthMidLow = serializedObject.FindProperty("colorHealthMidLow");
		colorHealthLow = serializedObject.FindProperty("colorHealthLow");

		valueHealthHigh = serializedObject.FindProperty("valueHealthHigh");
		valueHealthMidHigh = serializedObject.FindProperty("valueHealthMidHigh");
		valueHealthMid = serializedObject.FindProperty("valueHealthMid");
		valueHealthMidLow = serializedObject.FindProperty("valueHealthMidLow");
		valueHealthLow = serializedObject.FindProperty("valueHealthLow");

		healthValue = serializedObject.FindProperty("healthValue");
		imageColor = serializedObject.FindProperty("imageColor");


	}

	public override void OnInspectorGUI()
	{
		//Apply the gui skin
		_original = GUI.skin;
		GUI.skin = _skin;
		var rect = GUILayoutUtility.GetRect(1, 1);


		GUILayout.BeginVertical(_skin.customStyles[0]);

		GUILayout.Label("Health Animation", _skin.GetStyle("photonTitle"));

		GUILayout.BeginVertical(_skin.customStyles[7]);

		EditorGUI.BeginChangeCheck();

		ColorHealth();
		FloatHealth();
		Components();



		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.ApplyModifiedProperties();
		}
		GUILayout.EndVertical();
		GUILayout.EndVertical();
	}


	public void ColorHealth()
	{
		GUILayout.BeginVertical(_skin.customStyles[3]);

		GUILayout.Label("Color of Health", _skin.GetStyle("textSubTitlePhoton"));
		GUILayout.Space(10);

		EditorGUILayout.PropertyField(colorHealthHigh);
		EditorGUILayout.PropertyField(colorHealthMidHigh);
		EditorGUILayout.PropertyField(colorHealthMid);
		EditorGUILayout.PropertyField(colorHealthMidLow);
		EditorGUILayout.PropertyField(colorHealthLow);



		GUILayout.EndHorizontal();
	}

	public void FloatHealth()
	{
		GUILayout.BeginVertical(_skin.customStyles[3]);

		GUILayout.Label("Value Limit Changer", _skin.GetStyle("textSubTitlePhoton"));
		GUILayout.Space(10);

		EditorGUILayout.PropertyField(valueHealthHigh);
		EditorGUILayout.PropertyField(valueHealthMidHigh);
		EditorGUILayout.PropertyField(valueHealthMid);
		EditorGUILayout.PropertyField(valueHealthMidLow);
		EditorGUILayout.PropertyField(valueHealthLow);

		EditorGUILayout.HelpBox("The order of this numbers it's 0 to 1. Number 0 is " +
			"the highest and 1 is the lowest. You need to set the value limit.", MessageType.Info);

		GUILayout.EndVertical();
	}

	public void Components()
	{
		GUILayout.BeginVertical(_skin.customStyles[3]);
		GUILayout.Label("Components", _skin.GetStyle("textSubTitlePhoton"));
		GUILayout.Space(10);

		EditorGUILayout.PropertyField(healthValue, new GUIContent("GUI Controller"));
		EditorGUILayout.PropertyField(imageColor, new GUIContent("Image"));

		GUILayout.EndVertical();
	}
}
