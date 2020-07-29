using System.Collections;
using UnityEngine;
using UnityEngine.UI;
//using Photon.Pun;
using Photon.Realtime;
using TMPro;

using UnityEditor;
using System;

namespace Photon.Pun.UtilityScripts
{

    public class AutoLobby : MonoBehaviourPunCallbacks
    {
        // Variables para el sistema de lobby.
        [Tooltip("Sirve para conectar automáticamente a los servicios de Photon y asi poder acceder al juego sin pulsar ninguna tecla")]
        [HideInInspector] public bool AutoConnect = true;


        [Tooltip("Version de photon que usara el juego, siempre se usará la versión 1.")]
        [HideInInspector] public byte Version = 1;


        [Tooltip("El numero máximo de jugadores en una sala, si supera el límite creará otra sala.")]
        [HideInInspector] public byte MaxPlayers = 4;

        [Tooltip("Variable para obtener el prefab de las conexiones. Con el se va creando el texto.")]
        [HideInInspector] public GameObject connections;
        [Tooltip("Lugar donde se va a emparentar el prefab de " + "<color=#3d47c0>" + "connections" + "</color>")]
        [HideInInspector] public Transform parentText;
        [Tooltip("Componente de texto, en este componente asignaremos el contador de cuanto tardará en unirse al juego")]
        [HideInInspector] public Text countDown;
        [Tooltip("Variable para obtener el prefab del contador. Con el se va creando el texto.")]
        [HideInInspector] public GameObject countdownInstance;
        [Tooltip("Variable que usaremos para guardar el valor de " + "<color=#3d47c0>" + "countdownInstance" + "</color>")]
        [HideInInspector] public GameObject countdownTimer;
        [Tooltip("Componente de texto, en este componente asignaremos cuantos jugadores hay en la sala")]
        [HideInInspector] public TextMeshProUGUI textPlayers;
        [Tooltip("Boton que con el le asignaremos si se puede pulsar o no")]
        [HideInInspector] public Button buttonInteract;
        [Tooltip("Nombre que tendra el jugador")]
        [HideInInspector] public GameObject nickName;
        [Tooltip("Canvas de la lista de los jugadores de la sala")]
        [HideInInspector] public GameObject gameObjectListPlayers;

        [Tooltip("El numero de jugadores que hay en la sala")]
        [HideInInspector] public int playersCount;

        [Tooltip("El numero minimo para iniciar el juego en una sala, lo ponemos en BYTE porque es el parametro que utiliza photon en las salas")]
        [HideInInspector] public byte minPlayersPerRoom = 2;
        [HideInInspector] public string NameScene;
        [Tooltip("Velocidad a la que irá el objeto de carga")]
        [HideInInspector] public float rotSpeed;
        [Tooltip("Comparativo, compara si ha iniciado photon o no")]
        [HideInInspector] public bool isLoading = false;
        [Tooltip("Comparativo, compara para ver si el objeto de carga puede girar o no")]
        [HideInInspector] public bool canRotate = false;


        public void Start()
        {
            Application.quitting += Application_quitting;


            if (this.AutoConnect)
            {
                this.ConnectNow();
            }
        }

		private void Application_quitting()
		{
            PlayFabCloudScripts.SetUserOnlineState(false);
		}

		// Conecta a photon con los ajustes y la version que le asignamos
		public void ConnectNow()
        {
            Debug.Log("ConnectAndJoinRandom.ConnectNow() will now call: PhotonNetwork.ConnectUsingSettings().");


            // Creamos una variable local y esta variable sera iguala la creacion del prefab connections
            // Lo emparentamos
            // Escribimos el texto segun la conexion
            GameObject GO = Instantiate(connections);
            GO.transform.SetParent(parentText);
            GO.GetComponent<TextMeshProUGUI>().text = "<color=#ebe724>" + "Connecting to server" + "</color>";


            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = this.Version + "." + SceneManagerHelper.ActiveSceneBuildIndex;
        }
        public void ButtonInterectable()
        {
            buttonInteract.interactable = true;
        }

        // Cuando se conecta a los servicios de Photon
        public override void OnConnectedToMaster()
        {
            PlayFabCloudScripts.SetUserOnlineState(true);

            // Creamos una variable local y esta variable sera iguala la creacion del prefab connections
            // Lo emparentamos
            // Escribimos el texto segun la conexion
            GameObject GO = Instantiate(connections);
            GO.transform.SetParent(parentText);
            GO.GetComponent<TextMeshProUGUI>().text = "<color=#0d9139>" + "Connected to server" + "</color>";


            GameObject nickNameGO = Instantiate(nickName);
            nickNameGO.transform.SetParent(parentText);


            Debug.Log("OnConnectedToMaster() was called by PUN. This client is now connected to Master Server in region [" + PhotonNetwork.CloudRegion +
                    "] and can join a room. Calling: PhotonNetwork.JoinRandomRoom();");
            PhotonNetwork.AutomaticallySyncScene = false;

        }
        public void ConnectToLobby()

        {
            buttonInteract.interactable = false;
            PhotonNetwork.JoinLobby();
        }
        // Cuando se una una lobby.
        public override void OnJoinedLobby()
        {
            // Creamos una variable local y esta variable sera iguala la creacion del prefab connections
            // Lo emparentamos
            // Escribimos el texto segun la conexion
            GameObject GO = Instantiate(connections);
            GO.transform.SetParent(parentText);
            GO.GetComponent<TextMeshProUGUI>().text = "<color=#0d9139>" + "Joined lobby" + "</color>";


            Debug.Log("OnJoinedLobby(). This client is now connected to Relay in region [" + PhotonNetwork.CloudRegion + "]. This script now calls: PhotonNetwork.JoinRandomRoom();");
            PhotonNetwork.JoinRandomRoom();


            // Creamos una variable local y esta variable sera iguala la creacion del prefab connections
            // Lo emparentamos
            // Escribimos el texto segun la conexion
            GameObject GOT = Instantiate(connections);
            GOT.transform.SetParent(parentText);
            GOT.GetComponent<TextMeshProUGUI>().text = "<color=#ebe724>" + "Joining in room" + "</color>";

        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            // Creamos un variable local int para tener el numero de hijos
            // Borramos todos los hijos que haya dentro del padre.
            int childs = parentText.transform.childCount;
            for(int i = childs -1; i > 0; i--)
            {
                GameObject.Destroy(parentText.transform.GetChild(i).gameObject);
            }

            // Creamos una variable local y esta variable sera iguala la creacion del prefab connections
            // Lo emparentamos
            // Escribimos el texto segun la conexion
            GameObject GO = Instantiate(connections);
            GO.transform.SetParent(parentText);
            GO.GetComponent<TextMeshProUGUI>().text = "<color=#9a182c>" + "Cause of disconnection: " + cause + "</color>";
            ConnectNow();
        }

        // Cuando te intentas unir y falla.
        public override void OnJoinRandomFailed(short returnCode, string message)
        {

            // Creamos una variable local y esta variable sera iguala la creacion del prefab connections
            // Lo emparentamos
            // Escribimos el texto segun la conexion
            GameObject GO = Instantiate(connections);
            GO.transform.SetParent(parentText);
            GO.GetComponent<TextMeshProUGUI>().text = "<color=#9a182c>" +  "Failed to connect in room" +"</color>";


            Debug.Log("OnJoinRandomFailed() was called by PUN. No random room available in region [" + PhotonNetwork.CloudRegion + "], so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
            PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = this.MaxPlayers }, null);


            // Creamos una variable local y esta variable sera iguala la creacion del prefab connections
            // Lo emparentamos
            // Escribimos el texto segun la conexion
            GameObject GOT = Instantiate(connections);
            GOT.transform.SetParent(parentText);
            GOT.GetComponent<TextMeshProUGUI>().text = "<color=#ebe724>" + "Creating a room" + "</color>";
        }

        [PunRPC]
        private void RPC_LoadScene()
        {
            PhotonNetwork.LoadLevel(NameScene);
        }


        // Cuando se una a una room
        public override void OnJoinedRoom()
        {
            // Creamos una variable local y esta variable sera iguala la creacion del prefab connections
            // Lo emparentamos
            // Escribimos el texto segun la conexion
            GameObject GO = Instantiate(connections);
            GO.transform.SetParent(parentText);
            GO.GetComponent<TextMeshProUGUI>().text = "<color=#0d9139>" + "Joined room" + "</color>";


            gameObjectListPlayers.SetActive(true);
            Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room in region [" + PhotonNetwork.CloudRegion + "]. Game is now running.");
            isLoading = true;


            // Creamos una variable local y esta variable sera iguala la creacion del prefab connections
            // Lo emparentamos
            // Escribimos el texto segun la conexion
            GameObject GOT = Instantiate(connections);
            GOT.transform.SetParent(parentText);
            GOT.GetComponent<TextMeshProUGUI>().text = "<color=#ebe724>" + "Waiting for more players" + "</color>";


            textPlayers.gameObject.SetActive(true);
            textPlayers.gameObject.transform.SetParent(parentText);

        }

 

        public void Update()
        {
            // Cuando el comparativo sea true, lanzara programación de girar del objeto de carga.
            if (canRotate)
            {
                countdownTimer.transform.GetChild(0).gameObject.transform.Rotate(Vector3.forward * Time.deltaTime * rotSpeed);
            }

            // Obtenemos el valor de los jugadores que hay en la sala.
            if (PhotonNetwork.CurrentRoom != null)
                playersCount = PhotonNetwork.CurrentRoom.PlayerCount;

            textPlayers.text = "<color=#100d91>" + playersCount + "</color>" + "<color=#a81091>" + "/" + "</color>" + "<color=#100d91>" + MaxPlayers + "</color>";


            // Compara si ya estamos en la pantalla de carga para entrar a jugar.
            // Y mira si el numero de jugadores en la sala es mayor al minimo
            if (isLoading && playersCount >= minPlayersPerRoom)
            {
                StartCoroutine(WaitingToScene());
                //PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }


        // Enumerador para hacer una cuenta atras
        IEnumerator WaitingToScene()
        {
            // Creamos una variable local y esta variable sera iguala la creacion del prefab connections
            // Lo emparentamos
            // Escribimos el texto segun la conexion
            GameObject GO = Instantiate(connections);
            GO.transform.SetParent(parentText);
            GO.GetComponent<TextMeshProUGUI>().text = "<color=#0d9139>" + "The game will start soon" + "</color>";


            textPlayers.gameObject.transform.SetAsLastSibling();


            // Creamos una variable local y esta variable sera iguala la creacion del prefab connections
            // Lo emparentamos
            // Escribimos el texto segun la conexion
            GameObject GOC = Instantiate(countdownInstance);
            GOC.transform.SetParent(parentText);
            countdownTimer = GOC;
            GOC.transform.GetChild(0).gameObject.SetActive(true);


            canRotate = true;
            isLoading = false;
            int countdown = 5; // Variable de tiempo.
            while (countdown > 0) // Parametro que sirve para que cuando la variable sea mayor que 0 cumplirá el codigo. 
            {
                // Componente de texto para saber por donde va.
                countdownTimer.GetComponent<Text>().text = countdown.ToString();
                // Descontara 1 segundo del enumerador
                yield return new WaitForSeconds(1);

                countdown--; // Restamos 1 a la variable de tiempo.
            }

            // Cuando pasan los segundos del enumerador
            RPC_LoadScene();
        }
    }
}