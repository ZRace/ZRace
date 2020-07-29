using System;
using UnityEngine;

public class IsOnlinePlayer : MonoBehaviour
{
	public void Start()
	{
		Application.quitting += Application_quitting;
	}

	private void Application_quitting()
	{
		PlayFabCloudScripts.SetUserOnlineState(false);
	}
}
