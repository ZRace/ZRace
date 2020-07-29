using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabCloudScripts : MonoBehaviour
{
	public static void SetUserOnlineState(bool state)
	{
		ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest()
		{
			FunctionName = "SetOnlineState",
			FunctionParameter = new { OnlineState = state },
			GeneratePlayStreamEvent = true

		};

		PlayFabClientAPI.ExecuteCloudScript(request, result => { }, error => { });
	}
}
