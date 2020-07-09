using UnityEngine;
using UnityEditor;
using System.IO;
using CBGames.Editors;

[CustomEditor(typeof(PlayFabLogin), true)]
[CanEditMultipleObjects]
public class PlayFabLoginEditor : Editor
{


	SerializedProperty regEmail, regUsername, regPassword, logUsername, logPassword, errorText,
		contactEmail, playFabID, displayName, loadScene, isOnline;

	int toolbarInt = 0;
	string[] toolbarString = { "Register", "Log In", "Callbacks Errors" };

	GUISkin _skin = null;
	GUISkin _original = null;
	Color _titleColor;

	private void OnEnable()
	{
		if (!_skin) _skin = E_Helpers.LoadSkin(E_Core.e_guiSkinPathZRace);
		_titleColor = new Color32(1, 9, 28, 255); //dark blue
		regEmail = serializedObject.FindProperty("regEmail");
		regUsername = serializedObject.FindProperty("regUsername");
		regPassword = serializedObject.FindProperty("regPassword");
		contactEmail = serializedObject.FindProperty("contactEmail");

		logUsername = serializedObject.FindProperty("logUsername");
		logPassword = serializedObject.FindProperty("logPassword");
		isOnline = serializedObject.FindProperty("isOnline");
		playFabID = serializedObject.FindProperty("playFabID");
		displayName = serializedObject.FindProperty("displayName");
		loadScene = serializedObject.FindProperty("loadScene");

		errorText = serializedObject.FindProperty("errorText");
	}


	public override void OnInspectorGUI()
	{
		//Apply the gui skin
		_original = GUI.skin;
		GUI.skin = _skin;
		var rect = GUILayoutUtility.GetRect(1, 1);

		GUILayout.BeginVertical(_skin.customStyles[0]);

		GUILayout.Label("PlayFab Manager", _skin.GetStyle("Label"));

		GUILayout.Space(15);

		GUILayout.BeginVertical(_skin.customStyles[1]);
		toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarString, _skin.customStyles[2]);

		EditorGUI.BeginChangeCheck();

		if (toolbarInt == 0)
		{
			Register();
		}

		if (toolbarInt == 1)
		{
			Log();
		}

		if (toolbarInt == 2)
		{
			Errors();
		}
		GUILayout.EndVertical();

		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.ApplyModifiedProperties();
		}

		GUILayout.Space(20);

		GetInfo();
		GUILayout.EndVertical();
	}

	private void Register()
	{
		GUILayout.BeginVertical(_skin.customStyles[3]);
		EditorGUILayout.PropertyField(regEmail, new GUIContent("Email:"));
		GUILayout.Space(5);
		EditorGUILayout.PropertyField(regUsername, new GUIContent("Username:"));
		GUILayout.Space(5);
		EditorGUILayout.PropertyField(regPassword, new GUIContent("Password:"));
		GUILayout.Space(5);
		if (regEmail.objectReferenceValue == null)
		{
			EditorGUILayout.HelpBox("Select the input field for email.", MessageType.Error);
		}
		if (regUsername.objectReferenceValue == null)
		{
			EditorGUILayout.HelpBox("Select the input field for username.", MessageType.Error);
		}
		if (regPassword.objectReferenceValue == null)
		{
			EditorGUILayout.HelpBox("Select the input field for password.", MessageType.Error);
		}
		EditorGUILayout.HelpBox("In this section you only need to select the input fields of the registration panel (text mesh pro).", MessageType.None);

		GUILayout.EndVertical();
	}


	private void Log()
	{
		GUILayout.BeginVertical(_skin.customStyles[3]);
		EditorGUILayout.PropertyField(logUsername, new GUIContent("Username:"));
		GUILayout.Space(5);
		EditorGUILayout.PropertyField(logPassword, new GUIContent("Password:"));
		GUILayout.Space(5);
		EditorGUILayout.PropertyField(loadScene, new GUIContent("Name the scene to load: "));
		GUILayout.Space(5);
		if (logUsername.objectReferenceValue == null)
		{
			EditorGUILayout.HelpBox("Select the input field for username.", MessageType.Error);
		}
		if (logPassword.objectReferenceValue == null)
		{
			EditorGUILayout.HelpBox("Select the input field for password.", MessageType.Error);
		}
		if (loadScene.stringValue.Length == 0)
		{
			EditorGUILayout.HelpBox("Write the scene to load.", MessageType.Error);
		}
		EditorGUILayout.HelpBox("In this section you only need to select the input fields of the log in panel (text mesh pro).", MessageType.None);

		GUILayout.EndVertical();
	}


	private void Errors()
	{
		GUILayout.BeginVertical(_skin.customStyles[3]);
		EditorGUILayout.PropertyField(errorText, new GUIContent("Callbacks Errors: "));
		GUILayout.Space(5);
		if (errorText.objectReferenceValue == null)
		{
			EditorGUILayout.HelpBox("Select the input field for password.", MessageType.Error);
		}
		EditorGUILayout.HelpBox("In this section you only need to select the text of the error text object (text mesh pro).", MessageType.None);
		GUILayout.EndVertical();
	}

	private void GetInfo()
	{
		GUILayout.BeginVertical(_skin.customStyles[4]);
		GUILayout.Label("Get Info Player", _skin.GetStyle("textInfoPlayer"));
		GUILayout.BeginVertical(_skin.customStyles[3]);
		EditorGUILayout.PropertyField(contactEmail, new GUIContent("Contact Email:"));
		EditorGUILayout.PropertyField(playFabID, new GUIContent("ID Username:"));
		EditorGUILayout.PropertyField(displayName, new GUIContent("Display Name:"));
		EditorGUILayout.PropertyField(isOnline, new GUIContent("Online Number"));
		EditorGUILayout.HelpBox("This section can't be changed.", MessageType.Warning);
		GUILayout.EndVertical();
		GUILayout.EndVertical();
	}
}
