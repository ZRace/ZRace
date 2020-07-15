using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;

public class PhotonPuntuation : MonoBehaviourPun, IPunObservable
{
	[SerializeField] public int scorePlayer;


	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext(scorePlayer);
		}
		else if (stream.IsReading)
		{
			scorePlayer = (int)stream.ReceiveNext();
		}
	}
	public void SendScoreTargets()
	{
		if(photonView.IsMine)
		{
			photonView.RPC("SetScore", RpcTarget.All);
		}
	}

	[PunRPC]
	void SetScore()
	{
		int _score;
		_score = scorePlayer;
		PhotonNetwork.LocalPlayer.AddScore(_score);
	}


}
