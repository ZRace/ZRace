using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBGames.UI
{
    public class VisualizeRooms : MonoBehaviour
    {
        [Tooltip("The gameobject that will act as the parent. Will replace all child objects according to results.")]
        [SerializeField] protected Transform parentObj = null;
        [Tooltip("Each room found will spawn this as a child of the parentObj.")]
        [SerializeField] protected GameObject roomButton = null;
        [Tooltip("Watch for any room changes and auto update this list.")]
        [SerializeField] protected bool autoUpate = false;
        public bool canDisplaySessionRooms = false;
        public string onlyDisplaySessionRooms = "";
        [SerializeField] protected string filterRooms = "";
        [SerializeField] protected bool debugging = false;

        protected UICoreLogic logic;
        protected Dictionary<string, RoomInfo> _roomList = new Dictionary<string, RoomInfo>();

        protected virtual void Start()
        {
            logic = FindObjectOfType<UICoreLogic>();
        }
        public virtual void ManullayUpdateList(Dictionary<string, RoomInfo> roomList)
        {
            if (debugging == true) Debug.Log("Manually updating list...");
            _roomList = roomList;
            RefreshList();
        }

        public virtual void GetRoomListFromUI()
        {
            _roomList = logic.GetRoomList();
            if (debugging == true) Debug.Log("Found: " + _roomList.Count + " rooms.");
            RefreshList();
        }

        public virtual void WaitForListChange()
        {
            StartCoroutine(WaitForChange());
        }
        protected virtual IEnumerator WaitForChange()
        {
            if (debugging == true) Debug.Log("Waiting for a room list update");
            logic = (logic == null) ? FindObjectOfType<UICoreLogic>() : logic;
            yield return new WaitUntil(() => logic.GetRoomList().Count > 0);
            GetRoomListFromUI();
        }
        protected virtual void Update()
        {
            if (autoUpate == false) return;
            if (_roomList.Count != logic.GetRoomList().Count)
            {
                GetRoomListFromUI();
            }
        }

        public virtual void SetFilter(string filter)
        {
            if (debugging == true) Debug.Log("Setting filter: " + filter);
            filterRooms = filter;
            WaitForListChange();
        }

        protected virtual void RefreshList()
        {
            if (debugging == true) Debug.Log("Refreshing room list...");
            foreach (Transform child in parentObj)
            {
                Destroy(child.gameObject);
            }
            List<string> lockedRooms = new List<string>();
            foreach(KeyValuePair<string, RoomInfo> room in _roomList)
            {
                if (!string.IsNullOrEmpty(filterRooms) && !room.Value.Name.Contains(filterRooms) ||
                    canDisplaySessionRooms == false && room.Value.Name.Contains("_") ||
                    !string.IsNullOrEmpty(onlyDisplaySessionRooms) && 
                    canDisplaySessionRooms == true && room.Value.Name.Contains("_") && 
                    room.Value.Name.Split('_')[0] != onlyDisplaySessionRooms
                )
                {
                    if (debugging == true) Debug.Log("Skipping room because of filter value");
                    continue;
                }
                else if (room.Value.IsVisible == false)
                {
                    if (debugging == true) Debug.Log("Skipping room because invisible.");
                    continue;
                }

                if (room.Value.Name.Contains("_") && canDisplaySessionRooms == true)
                {
                    if (debugging == true) Debug.Log("Creating session room button.");
                    GenerateRoomButton(room.Value.Name, room.Value);
                }
                else
                {
                    if (debugging == true) Debug.Log("Creating room button.");
                    GenerateRoomButton(room.Value.Name, room.Value);
                }
            }
        }

        protected virtual void GenerateRoomButton(string roomName, RoomInfo roomInfo)
        {
            GameObject roomBtn = Instantiate(roomButton);
            roomBtn.transform.SetParent(parentObj);
            roomBtn.transform.localScale = new Vector3(1, 1, 1);
            roomBtn.transform.localPosition = Vector3.zero;
            if (roomBtn.GetComponent<RoomButton>())
            {
                roomBtn.GetComponent<RoomButton>().SetRoomValues(roomInfo);
                roomBtn.GetComponent<RoomButton>().SetRoomName(roomName);
            }
        }
    }
}