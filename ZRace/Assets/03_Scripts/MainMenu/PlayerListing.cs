using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using Photon.Voice.Unity.UtilityScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListing : MonoBehaviour
{
    public int score;
    public Player PhotonPlayer { get; private set; }

    [SerializeField]
    private TextMeshProUGUI _playerName;
    private TextMeshProUGUI PlayerName
    {
        get { return _playerName; }
    }
	public void ApplyPhotonPlayer(Player photonPlayer)
    {
        photonPlayer.SetScore(score);
        PhotonPlayer = photonPlayer;
        PlayerName.text = photonPlayer.NickName + " Score:";
    }
}
