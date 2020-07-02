using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(PlayFabLogin), true)]
[CanEditMultipleObjects]
public class PlayFabLoginEditor : Editor
{
	SerializedProperty regEmail, regUsername, regPassword, logUsername, logPassword, errorText,
		contactEmail, playFabID, displayName, loadScene, isOnline;

	GUIStyle fieldBox;

	int toolbarInt = 0;
	string[] toolbarString = { "Register", "Log In", "Callbacks Errors" };

	private void OnEnable()
	{
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

		base.OnInspectorGUI();

		GUILayout.BeginVertical("HelpBox");



		GUI.color = new Color32(255, 163, 56, 255);
		GUILayout.BeginHorizontal("HelpBox");
		GUILayout.FlexibleSpace();
		GUILayout.Label("PlayFab Manager", EditorStyles.boldLabel);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUI.color = Color.white;



		const int PAD = 6;
		if (fieldBox == null)
			fieldBox = new GUIStyle("HelpBox") { padding = new RectOffset(PAD, PAD, PAD, PAD) };
		toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarString);

		EditorGUI.BeginChangeCheck();



		if (toolbarInt == 0)
		{
			Register();
		}



		if(toolbarInt == 1)
		{
			Log();
		}


		if(toolbarInt == 2)
		{
			Errors();
		}

		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.ApplyModifiedProperties();
		}


		GUILayout.Space(20);

		GUI.color = new Color32(83, 255, 252, 255);
		GUILayout.BeginHorizontal("HelpBox");
		GUILayout.FlexibleSpace();
		GUILayout.Label("Account Player Info", EditorStyles.boldLabel);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUI.color = Color.white;

		GetInfo();
		GUILayout.EndVertical();
	}

	private void Register()
	{
		GUILayout.BeginVertical("HelpBox");
		EditorGUILayout.PropertyField(regEmail, new GUIContent("Email:"));
		EditorGUILayout.PropertyField(regUsername, new GUIContent("Username:"));
		EditorGUILayout.PropertyField(regPassword, new GUIContent("Password:"));
		if(regEmail.objectReferenceValue == null)
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
		GUILayout.BeginVertical("HelpBox");
		EditorGUILayout.PropertyField(logUsername, new GUIContent("Username:"));
		EditorGUILayout.PropertyField(logPassword, new GUIContent("Password:"));
		EditorGUILayout.PropertyField(loadScene, new GUIContent("Name the scene to load: "));
		if (logUsername.objectReferenceValue == null)
		{
			EditorGUILayout.HelpBox("Select the input field for username.", MessageType.Error);
		}
		if (logPassword.objectReferenceValue == null)
		{
			EditorGUILayout.HelpBox("Select the input field for password.", MessageType.Error);
		}
		if(loadScene.stringValue.Length == 0)
		{
			EditorGUILayout.HelpBox("Write the scene to load.", MessageType.Error);
		}
		EditorGUILayout.HelpBox("In this section you only need to select the input fields of the log in panel (text mesh pro).", MessageType.None);
		GUILayout.EndVertical();
	}


	private void Errors()
	{
		GUILayout.BeginVertical("HelpBox");
		EditorGUILayout.PropertyField(errorText, new GUIContent("Callbacks Errors: "));
		if (errorText.objectReferenceValue == null)
		{
			EditorGUILayout.HelpBox("Select the input field for password.", MessageType.Error);
		}
		EditorGUILayout.HelpBox("In this section you only need to select the text of the error text object (text mesh pro).", MessageType.None);
		GUILayout.EndVertical();
	}

	private void GetInfo()
	{
		GUILayout.BeginVertical("HelpBox");
		EditorGUILayout.PropertyField(contactEmail, new GUIContent("Contact Email:"));
		EditorGUILayout.PropertyField(playFabID, new GUIContent("ID Username:"));
		EditorGUILayout.PropertyField(displayName, new GUIContent("Display Name:"));
		EditorGUILayout.PropertyField(isOnline, new GUIContent("Online Number"));
		EditorGUILayout.HelpBox("This section can't be changed.", MessageType.Warning);
		GUILayout.EndVertical();
	}
}
