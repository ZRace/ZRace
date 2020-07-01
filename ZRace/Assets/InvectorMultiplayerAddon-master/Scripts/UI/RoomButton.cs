using CBGames.Core;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CBGames.UI
{
    public class RoomButton : MonoBehaviour
    {
        [Tooltip("The text that will display what the name of this room is.")]
        public Text roomName = null;
        [Tooltip("The text that will display the current number of players in this room")]
        public Text numberOfPlayers = null;
        [Tooltip("The text that will display the max number of players in this room")]
        public Text maxNumOfPlayers = null;
        [Tooltip("The text that will display if a player is in a lobby or not")]
        public Text isOpen = null;
        [Tooltip("The database that holds all the information about all given scenes.")]
        public SceneDatabase database;
        [Tooltip("The password required for this room. Do not set here unless used for testing.")]
        [SerializeField] private string _password = "";

        private string roomJoinName = "";
        public int indexToLoad = 0;
        private List<LobbyItem> scenes = new List<LobbyItem>();
        private RoomInfo _roomInfo;
        private UICoreLogic logic;

        private void Start()
        {
            logic = FindObjectOfType<UICoreLogic>();    
        }

        public void SetValues(string inputRoomName, int numOfPlayers, bool inputIsOpen, string roomDisplayName = null)
        {
            if (roomName != null) roomName.text = inputRoomName;
            if (numberOfPlayers != null) numberOfPlayers.text = numberOfPlayers.ToString();
            if (isOpen != null) isOpen.text = (inputIsOpen == true) ? "OPEN" : "CLOSED";
        }

        public void SetRoomName(string inputRoomName)
        {
            if (roomName != null)
            {
                roomName.text = inputRoomName;
            }
            roomJoinName = inputRoomName;
        }

        public void SetRoomValues(RoomInfo room, string openText = "OPEN", string closedText = "CLOSED")
        {
            _roomInfo = room;
            roomJoinName = room.Name;
            if (roomName != null)
            {
                roomName.text = room.Name;
            }
            if (numberOfPlayers != null)
            {
                numberOfPlayers.text = room.PlayerCount.ToString(); ;
            }
            if (maxNumOfPlayers != null)
            {
                maxNumOfPlayers.text = room.MaxPlayers.ToString();
            }
            if (isOpen != null)
            {
                isOpen.text = (room.IsOpen == true) ? openText : closedText;
            }
            if (room.CustomProperties.ContainsKey(RoomProperty.Password))
            {
                _password = (string)room.CustomProperties[RoomProperty.Password];
            }
        }

        public void JoinLobby()
        {
            logic.JoinRoom(roomJoinName);
        }

        public void JoinRoom()
        {
            if (scenes.Count > 1 && FindObjectOfType<ExampleUI>())
            {
                FindObjectOfType<ExampleUI>().DisplayRoomOptions(scenes);
            }
            else
            {
                try
                {
                    NetworkManager.networkManager.JoinRoom(roomJoinName);
                    if (SceneManager.GetActiveScene().buildIndex != indexToLoad)
                    {
                        NetworkManager.networkManager.NetworkLoadLevel(indexToLoad, null, false);
                    }
                }
                catch
                {
                    Debug.LogError("RoomButton - Failed To Load Room: " + roomJoinName +" or join scene index with: "+ indexToLoad);
                }
            }
        }

        public void AddAvailableScene(LobbyItem sceneItem)
        {
            scenes.Add(sceneItem);
            if (scenes.Count == 1)
            {
                roomJoinName = sceneItem.rawRoomName;
                if (string.IsNullOrEmpty(sceneItem.sceneName) || sceneItem.sceneName.Contains("Lobby"))
                {
                    indexToLoad = database.storedScenesData.Find(x => x.index == FindObjectOfType<ExampleUI>().lobbyIndex).index;
                }
                else
                {
                    indexToLoad = database.storedScenesData.Find(x => x.sceneName == sceneItem.sceneName).index;
                }
                SetValues(scenes[0].displayName, scenes[0].playerCount, scenes[0].isVisible, scenes[0].rawRoomName);
            }
            SetTotalPlayerCount();
        }

        public void SetTotalPlayerCount()
        {
            if (numberOfPlayers == null) return;
            int total = 0;
            foreach(LobbyItem scene in scenes)
            {
                total += scene.playerCount;
            }
            numberOfPlayers.text = total.ToString();
        }
    }
}