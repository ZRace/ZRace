using UnityEngine;
using UnityEditor;
using Photon.Realtime;
using TMPro;
using System.Collections;

namespace Photon.Pun.Demo.PunBasics
{
    public class notificationsRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
    {
        public GameObject notifications;


        //public override void OnPlayerEnteredRoom(Player newPlayer)
        //{
        //    base.OnPlayerEnteredRoom(newPlayer);
        //    GameObject go = Instantiate(notifications);
        //    go.transform.parent = gameObject.transform;
        //    go.GetComponent<TextMeshProUGUI>().text = "<color=#ff6908>" + newPlayer.NickName + "</color>" + "<color=#000000>" + " has joined in the room" + "</color>";
        //}

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerLeftRoom(newPlayer);
            GameObject go = Instantiate(notifications, transform.position, Quaternion.identity);
            go.transform.parent = gameObject.transform;
            go.GetComponent<TextMeshProUGUI>().text = "<color=#007bc2>" + newPlayer.NickName + "</color>" + "<color=#000000>" + " has joined the room" + "</color>";
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            GameObject go = Instantiate(notifications, transform.position, Quaternion.identity);
            go.transform.SetParent(gameObject.transform);
            go.GetComponent<TextMeshProUGUI>().text = "<color=#c70000>" + otherPlayer.NickName + "</color>" + "<color=#000000>" + " has left the room" + "</color>";
        }
    }
}
