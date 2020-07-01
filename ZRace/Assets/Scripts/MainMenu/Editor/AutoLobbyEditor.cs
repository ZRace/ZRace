using UnityEditor;
using UnityEngine;
using Photon.Pun.UtilityScripts;


[CustomEditor(typeof(AutoLobby), true)]
[CanEditMultipleObjects]
public class AutoLobbyEditor : Editor
{
    // Ponemos las propiedades de las variables de los items para que aparezcan en el inspector.
    SerializedProperty nickName, textPlayers, connections, parentText, countDown, countdownInstance, buttonInteract;

    SerializedProperty isLoading, canRotate, rotSpeed;

    SerializedProperty MaxPlayers, minPlayersPerRoom, playersCount, gameObjectListPlayers;

    SerializedProperty Version, AutoConnect;


    // Esto es como va ser el estilo de la caja contenedora.
    GUIStyle fieldBox;

    int toolbarInt = 0;
    string[] toolbarString = { "Connections", "Comparatives", "Parameters Room", "Parameters Photon" };

    // Antes de poner la GUI en el inspector, asignaremos las variables su propiedad
    // los nombres tienen que concidir para que funcionen.
    private void OnEnable()
    {
        nickName = serializedObject.FindProperty("nickName");
        textPlayers = serializedObject.FindProperty("textPlayers");
        connections = serializedObject.FindProperty("connections");
        parentText = serializedObject.FindProperty("parentText");
        countDown = serializedObject.FindProperty("countDown");
        countdownInstance = serializedObject.FindProperty("countdownInstance");
        buttonInteract = serializedObject.FindProperty("buttonInteract");

        isLoading = serializedObject.FindProperty("isLoading");
        canRotate = serializedObject.FindProperty("canRotate");
        rotSpeed = serializedObject.FindProperty("rotSpeed");

        MaxPlayers = serializedObject.FindProperty("MaxPlayers");
        minPlayersPerRoom = serializedObject.FindProperty("minPlayersPerRoom");
        playersCount = serializedObject.FindProperty("playersCount");
        gameObjectListPlayers = serializedObject.FindProperty("gameObjectListPlayers");

        Version = serializedObject.FindProperty("Version");
        AutoConnect = serializedObject.FindProperty("AutoConnect");

    }


    // Toda esta parte es lo que se vera en el inspector.
    public override void OnInspectorGUI()
    {
        // Muestra el script que hemos usado
        base.OnInspectorGUI();

        const int PAD = 6;

        if (fieldBox == null)
            fieldBox = new GUIStyle("HelpBox") { padding = new RectOffset(PAD, PAD, PAD, PAD) };

        // Crea la caja contenedora que puede ser en vertical u horizontal
        GUILayout.BeginVertical(fieldBox);

        GUI.color = new Color32(83, 255, 252, 255);
        GUILayout.BeginHorizontal("box");
        GUILayout.FlexibleSpace();
        GUILayout.Label("Photon Manager", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUI.color = Color.white;

        GUILayout.BeginVertical("HelpBox");
        // Primero de todo asignamos un valor a una variable y luego ese valor
        // serán las proporciones de la caja contenedora
        toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarString);


        // Asignamos en BeginChangeChek para poder cambiar los valores de los parametros.
        EditorGUI.BeginChangeCheck();


        if(toolbarInt == 0)
        {
            ConnectionsComponents();
        }

        if(toolbarInt == 1)
        {
            Comparatives();
        }

        if(toolbarInt == 2)
        {
            ParametersRoom();
        }

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }

        if (toolbarInt == 3)
        {
            ParametersPhoton();
        }

        GUILayout.EndVertical();
        EditorGUILayout.EndVertical();

    }
    
    private void ConnectionsComponents()
    {

        // Para coger la propiedad de la variables usaremos el comando "PropertyField".


        // Prefabs
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Prefabs Instances", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        EditorGUILayout.PropertyField(nickName);
        EditorGUILayout.PropertyField(connections);
        EditorGUILayout.PropertyField(countdownInstance);
        if (nickName.objectReferenceValue == null)
            EditorGUILayout.HelpBox("Please select the prefab for your nickname", MessageType.Error);
        if(connections.objectReferenceValue == null)
            EditorGUILayout.HelpBox("Please select the prefab for your list of connections", MessageType.Error);
        if(countdownInstance.objectReferenceValue == null)
            EditorGUILayout.HelpBox("Please select the prefab for your countdown instance", MessageType.Error);

        GUILayout.Space(15);


        // Text
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Text Components", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        EditorGUILayout.PropertyField(textPlayers);
        EditorGUILayout.PropertyField(countDown);
        if (textPlayers.objectReferenceValue == null || countDown.objectReferenceValue == null)
            EditorGUILayout.HelpBox("Please select the object with the correct component", MessageType.Error);

        GUILayout.Space(15);

        // More
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("More Components", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        EditorGUILayout.PropertyField(parentText);
        EditorGUILayout.PropertyField(buttonInteract);
        if (parentText.objectReferenceValue == null || buttonInteract.objectReferenceValue == null)
            EditorGUILayout.HelpBox("Please select the object with the correct component", MessageType.Error);

        // Sirve para finalizar la caja contenedora.

    }

    private void Comparatives()
    {
        EditorGUILayout.PropertyField(isLoading);
        if (!isLoading.boolValue)
        {
            EditorGUILayout.HelpBox("Waiting to join a room", MessageType.None);
        }
        else
        {
            EditorGUILayout.HelpBox("You are inside the room", MessageType.Info);
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(canRotate);
        EditorGUILayout.PropertyField(rotSpeed, GUILayout.MinWidth(50f));
        EditorGUILayout.EndHorizontal();
    }

    private void ParametersRoom()
    {
        Rect windowRect = new Rect(20, 20, 120, 50);
        EditorGUILayout.BeginVertical(fieldBox);
        GUILayout.BeginHorizontal("box");
        GUILayout.FlexibleSpace();
        GUILayout.Label("You can edit the parameters for your room", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        EditorGUILayout.PropertyField(MaxPlayers, new GUIContent("Max Players for room"));
        EditorGUILayout.PropertyField(minPlayersPerRoom, new GUIContent("Min Players for room"));
        EditorGUILayout.PropertyField(playersCount);
        GUILayout.EndVertical();

        GUILayout.Space(5);

        GUILayout.BeginVertical("box");
        EditorGUILayout.PropertyField(gameObjectListPlayers);
        if (gameObjectListPlayers.objectReferenceValue == null)
            EditorGUILayout.HelpBox("Please select the list for look all players in room", MessageType.Error);
        GUILayout.EndVertical();
    }

    private void ParametersPhoton()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(AutoConnect, new GUIContent("Auto connect to Photon"));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.PropertyField(Version);
        EditorGUILayout.HelpBox("Game version of Photon, Please don't touch", MessageType.Warning);
    }
}

