using UnityEngine;
using UnityEditor;
using CBGames.Editors;



[CustomEditor(typeof(SetNamePlayerV2), true)]
[CanEditMultipleObjects]
public class SetNamePlayerV2Editor : Editor
{
	SerializedProperty nickname, setNamePlayer, OnNameResult;


	GUISkin _skin = null;
	GUISkin _original = null;
	Color _titleColor;


	private void OnEnable()
	{
		if (!_skin) _skin = E_Helpers.LoadSkin(E_Core.e_guiSkinPathZRace);
		_titleColor = new Color32(1, 9, 28, 255);

		nickname = serializedObject.FindProperty("nickname");
		setNamePlayer = serializedObject.FindProperty("setNamePlayer");
		OnNameResult = serializedObject.FindProperty("OnNameResult");
	}


	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		_original = GUI.skin;
		GUI.skin = _skin;
		var rect = GUILayoutUtility.GetRect(1, 1);



		GUILayout.BeginVertical(_skin.customStyles[0]);

		GUILayout.Label("Set Name Player", _skin.GetStyle("Label"));

		GUILayout.Space(15);


		GUILayout.BeginVertical(_skin.customStyles[1]);

		GUILayout.BeginVertical(_skin.customStyles[3]);
		EditorGUI.BeginChangeCheck();



		EditorGUILayout.PropertyField(nickname, new GUIContent("Input Field Name"));
		GUILayout.Space(5);
		EditorGUILayout.PropertyField(setNamePlayer, new GUIContent("Script To Call Name"));
		GUILayout.Space(5);
		GUI.skin = _original;
		EditorGUILayout.PropertyField(OnNameResult);



		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.ApplyModifiedProperties();
		}
		GUILayout.EndVertical();
		GUILayout.EndVertical();
		GUILayout.EndVertical();


	}
}
