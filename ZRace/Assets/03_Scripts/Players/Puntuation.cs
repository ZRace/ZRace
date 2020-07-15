using Photon.Pun;
using UnityEngine;

public class Puntuation : MonoBehaviourPun
{
	[SerializeField] public int scorePlayer;
	private PhotonView _photonView;
	private Puntuation _photonPuntuation;

	private void Awake()
	{
		_photonView = GetComponent<PhotonView>();
		_photonPuntuation = GetComponent<Puntuation>();

		_photonView.ObservedComponents.Add(_photonPuntuation);
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
	}

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
}
