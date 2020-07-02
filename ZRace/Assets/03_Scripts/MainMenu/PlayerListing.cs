using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListing : MonoBehaviour
{
    public Player PhotonPlayer { get; private set; }

    [SerializeField]
    private TextMeshProUGUI _playerName;
    private TextMeshProUGUI PlayerName
    {
        get { return _playerName; }
    }


    public void ApplyPhotonPlayer(Player photonPlayer)
    {
        PhotonPlayer = photonPlayer;
        PlayerName.text = photonPlayer.NickName;
    }
}
