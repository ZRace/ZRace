using Invector.vCharacterController.AI;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
//using Photon.Pun;

public class SendScore : MonoBehaviourPun
{

	[Tooltip("Gets the transform of the player when it collides. This part is only to get the transform and then see what object is")]
	[HideInInspector] public Transform transformPlayer;
	[Tooltip("Get the GameObject from the transform. It is used to see if the object has the script to add score to the player")]
	[HideInInspector] public GameObject targetPlayer;
	[Tooltip("Add the variable amount to the player's score")]
	[HideInInspector] public int AddScore;

	[Tooltip("Array to see what tag can be detected when it collides")]
	[HideInInspector] public List<string> tagsToDetect;

	[Tooltip("Take the script to see if the zombie is dead. We do this to verify that the zombie is dead when it collides")]
	[HideInInspector] public v_AIController controllerToCheck;

	[Tooltip("Bool to verify that when it collides see if the zombie is dead")]
	[HideInInspector] public bool checkIsDead;

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

		if (checkIsDead)
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
