using Invector.vCharacterController.AI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections.Generic;
using UnityEngine;
//using Photon.Pun;

public class SendScore : MonoBehaviour
{
	public int scoreAdd;

	public Transform transformPlayer;
	public GameObject targetPlayer;
	public int AddScore;

	public List<string> tagsToDetect;

	public v_AIController controllerToCheck;

	public bool checkIsDead;

	public void OnCollisionEnter(Collision collision)
	{
		if(tagsToDetect.Contains(collision.transform.tag.ToString()))
		{
			transformPlayer = collision.transform;
			checkIsDead = true;
		}

	}

	private void Update()
	{
		if(checkIsDead)
		{
			if (controllerToCheck.isDead == true)
			{
				targetPlayer = transformPlayer.gameObject;
				targetPlayer.GetComponent<PhotonPuntuation>().scorePlayer += AddScore;
				targetPlayer.GetComponent<PhotonPuntuation>().SendScoreTargets();
				checkIsDead = false;
			}
			else
			{
				checkIsDead = false;
			}
		}

	}
}
