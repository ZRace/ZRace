using CBGames.Editors;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(SendScore))]
[CanEditMultipleObjects]


public class SendScoreEditor : Editor
{

	SerializedProperty transformPlayer, targetPlayer, AddScore, tagsToDetect,
		 controllerToCheck, checkIsDead;

	GUISkin _skin = null;
	GUISkin _original = null;
	Color _titleColor;

	private void OnEnable()
	{
		if (!_skin) _skin = E_Helpers.LoadSkin(E_Core.e_guiSkinPathZRace);
		transformPlayer = serializedObject.FindProperty("transformPlayer");
		targetPlayer = serializedObject.FindProperty("targetPlayer");
		AddScore = serializedObject.FindProperty("AddScore");
		tagsToDetect = serializedObject.FindProperty("tagsToDetect");
		controllerToCheck = serializedObject.FindProperty("controllerToCheck");
		checkIsDead = serializedObject.FindProperty("checkIsDead");

	}

	public override void OnInspectorGUI()
	{
		//Apply the gui skin
		_original = GUI.skin;
		GUI.skin = _skin;
		var rect = GUILayoutUtility.GetRect(1, 1);

		// Crea la caja contenedora que puede ser en vertical u horizontal
		GUILayout.BeginVertical(_skin.customStyles[0]);

		GUILayout.Label("Score Vehicle", _skin.GetStyle("Label"));

		GUILayout.BeginVertical(_skin.customStyles[1]);

		// Asignamos en BeginChangeChek para poder cambiar los valores de los parametros.
		EditorGUI.BeginChangeCheck();


		GUILayout.BeginVertical(_skin.customStyles[3]);




		EditableReferenceList(tagsToDetect, new GUIContent(tagsToDetect.displayName, tagsToDetect.tooltip), _skin.customStyles[3]);
		Components();

		GUILayout.EndVertical();



		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.ApplyModifiedProperties();
		}
		GUILayout.EndVertical();
		GUILayout.EndVertical();
	}


	private void Components()
	{
		EditorGUILayout.PropertyField(transformPlayer);
		EditorGUILayout.PropertyField(targetPlayer);
		EditorGUILayout.PropertyField(AddScore);
		EditorGUILayout.PropertyField(controllerToCheck);
		EditorGUILayout.PropertyField(checkIsDead);

	}







	//Array
	public void EditableReferenceList(SerializedProperty list, GUIContent gc, GUIStyle style = null)
	{
		EditorGUILayout.LabelField(gc);

		if (style == null)
			style = new GUIStyle("HelpBox") { padding = new RectOffset(6, 6, 6, 6) };

		EditorGUILayout.BeginVertical(style);

		int count = list.arraySize;

		if (count == 0)
		{
			if (GUI.Button(EditorGUILayout.GetControlRect(GUILayout.MaxWidth(20)), "+"))
			{
				int newindex = list.arraySize;
				list.InsertArrayElementAtIndex(0);
				list.GetArrayElementAtIndex(0).objectReferenceValue = null;
			}
		}
		else
			// List Elements and Delete buttons
			for (int i = 0; i < list.arraySize; ++i)
			{
				EditorGUILayout.BeginHorizontal();
				bool add = (GUI.Button(EditorGUILayout.GetControlRect(GUILayout.MaxWidth(20)), "+"));
				EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), GUIContent.none);
				bool remove = (GUI.Button(EditorGUILayout.GetControlRect(GUILayout.MaxWidth(20)), "x"));

				EditorGUILayout.EndHorizontal();

				if (add)
				{
					int newindex = list.arraySize;
					list.InsertArrayElementAtIndex(i);
					list.GetArrayElementAtIndex(i).objectReferenceValue = null;
					EditorGUILayout.EndHorizontal();
					break;
				}

				if (remove)
				{
					list.DeleteArrayElementAtIndex(i);
					EditorGUILayout.EndHorizontal();
					break;
				}
			}

		EditorGUILayout.EndVertical();
	}
}
