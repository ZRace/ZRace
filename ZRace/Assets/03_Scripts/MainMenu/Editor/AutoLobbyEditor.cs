using UnityEditor;
using UnityEngine;
using Photon.Pun.UtilityScripts;
using CBGames.Editors;


[CustomEditor(typeof(AutoLobby), true)]
[CanEditMultipleObjects]
public class AutoLobbyEditor : Editor
{
    // Ponemos las propiedades de las variables de los items para que aparezcan en el inspector.
    SerializedProperty nickName, textPlayers, connections, parentText, countDown, countdownInstance, buttonInteract;

    SerializedProperty isLoading, canRotate, rotSpeed;

    SerializedProperty MaxPlayers, minPlayersPerRoom, playersCount, gameObjectListPlayers, NameScene;

    SerializedProperty Version, AutoConnect;


    GUISkin _skin = null;
    GUISkin _original = null;
    Color _titleColor;

    int toolbarInt = 0;
    string[] toolbarString = { "Connections", "Comparatives", "Param. Room", "Param. Photon" };

    // Antes de poner la GUI en el inspector, asignaremos las variables su propiedad
    // los nombres tienen que concidir para que funcionen.
    private void OnEnable()
    {
        if (!_skin) _skin = E_Helpers.LoadSkin(E_Core.e_guiSkinPathZRace);
        _titleColor = new Color32(1, 9, 28, 255); //dark blue
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
        NameScene = serializedObject.FindProperty("NameScene");
        gameObjectListPlayers = serializedObject.FindProperty("gameObjectListPlayers");

        Version = serializedObject.FindProperty("Version");
        AutoConnect = serializedObject.FindProperty("AutoConnect");

    }


    // Toda esta parte es lo que se vera en el inspector.
    public override void OnInspectorGUI()
    {
        //Apply the gui skin
        _original = GUI.skin;
        GUI.skin = _skin;
        var rect = GUILayoutUtility.GetRect(1, 1);

        // Crea la caja contenedora que puede ser en vertical u horizontal
        GUILayout.BeginVertical(_skin.customStyles[0]);

        GUILayout.Label("Photon Manager", _skin.GetStyle("photonTitle"));

        GUILayout.BeginVertical(_skin.customStyles[7]);
        // Primero de todo asignamos un valor a una variable y luego ese valor
        // serán las proporciones de la caja contenedora
        toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarString, _skin.customStyles[2]);


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

        GUILayout.BeginVertical(_skin.customStyles[3]);

        // Prefabs

        GUILayout.Label("Prefabs Instances", _skin.GetStyle("textSubTitlePhoton"));

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
        GUILayout.Label("Text Components", _skin.GetStyle("textSubTitlePhoton"));
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
        GUILayout.Label("More Components", _skin.GetStyle("textSubTitlePhoton"));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        EditorGUILayout.PropertyField(parentText);
        EditorGUILayout.PropertyField(buttonInteract);
        if (parentText.objectReferenceValue == null || buttonInteract.objectReferenceValue == null)
            EditorGUILayout.HelpBox("Please select the object with the correct component", MessageType.Error);

        // Sirve para finalizar la caja contenedora.
        GUILayout.EndVertical();

    }

    private void Comparatives()
    {
        GUILayout.BeginVertical(_skin.customStyles[3]);
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
        GUILayout.EndVertical();
    }

    private void ParametersRoom()
    {
        GUILayout.BeginVertical(_skin.customStyles[3]);
        GUILayout.Label("You can edit the parameters for your room", _skin.GetStyle("textSubTitlePhoton"));
        EditorGUILayout.PropertyField(MaxPlayers, new GUIContent("Max Players for room"));
        EditorGUILayout.PropertyField(minPlayersPerRoom, new GUIContent("Min Players for room"));
        EditorGUILayout.PropertyField(playersCount);
        EditorGUILayout.PropertyField(NameScene);

        GUILayout.Space(5);

        EditorGUILayout.PropertyField(gameObjectListPlayers);
        if (gameObjectListPlayers.objectReferenceValue == null)
            EditorGUILayout.HelpBox("Please select the list for look all players in room", MessageType.Error);
        GUILayout.EndVertical();
    }

    private void ParametersPhoton()
    {
        GUILayout.BeginVertical(_skin.customStyles[3]);
        EditorGUILayout.PropertyField(AutoConnect, new GUIContent("Auto connect to Photon"));
        EditorGUILayout.PropertyField(Version);
        EditorGUILayout.HelpBox("Game version of Photon, Please don't touch", MessageType.Warning);
        GUILayout.EndVertical();
    }
}

