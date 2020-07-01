using CBGames.Core;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using UnityEngine;

public class AutoConnect : MonoBehaviour
{
    public string roomToAutoConnectTo = "Testing_room";

    void Start()
    {
        StartCoroutine(AutoStart());
    }
    IEnumerator AutoStart()
    {
        GetComponent<NetworkManager>().JoinLobby();
        yield return new WaitUntil(() => PhotonNetwork.InLobby);
        RoomOptions options = new RoomOptions()
        {
            MaxPlayers = 10,
            PublishUserId = true,
            IsVisible = true,
            IsOpen = true,
            CleanupCacheOnLeave = true
        };
        PhotonNetwork.JoinOrCreateRoom(roomToAutoConnectTo, options, TypedLobby.Default);
    }
}
