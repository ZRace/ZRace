using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Photon.Pun.UtilityScripts
{
    public class PlayerLayoutGroup : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private GameObject _playerListingPrefab;
        private GameObject PlayerListingPrefab
        {
            get { return _playerListingPrefab; }
        }

        private List<PlayerListing> _playerListing = new List<PlayerListing>();
        private List<PlayerListing> PlayerListings
        {
            get { return _playerListing; }
        }


        public void Start()
        {
            if (PhotonNetwork.InRoom == true)
            {
                OnJoinedRoom();
            }
            else
            {
                Debug.LogError("No hay rooms");
            }

        }

        public override void OnJoinedRoom()
        {
            Player[] photonPlayers = PhotonNetwork.PlayerList;
            for (int i = 0; i < photonPlayers.Length; i++)
            {
                PlayerJoinedRoom(photonPlayers[i]);
            }
        }
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            PlayerJoinedRoom(newPlayer);
        }
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            PlayerLeftRoom(otherPlayer);
        }



        private void PlayerJoinedRoom(Player photonPlayer)
        {
            if (photonPlayer == null)
                return;

            PlayerLeftRoom(photonPlayer);

            GameObject playerListingObject = Instantiate(PlayerListingPrefab);
            playerListingObject.transform.SetParent(transform, false);

            PlayerListing playerListing = playerListingObject.GetComponent<PlayerListing>();
            playerListing.ApplyPhotonPlayer(photonPlayer);

            PlayerListings.Add(playerListing);
        }

        private void PlayerLeftRoom(Player photonPlayer)
        {
            int index = PlayerListings.FindIndex(x => x.PhotonPlayer == photonPlayer);
            if (index != -1)
            {
                Destroy(PlayerListings[index].gameObject);
                PlayerListings.RemoveAt(index);
            }
        }
    }
}
