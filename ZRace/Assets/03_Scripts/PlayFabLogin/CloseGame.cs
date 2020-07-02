using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class CloseGame : MonoBehaviour
{
	private void OnApplicationQuit()
	{
		int onlineValue = 0;
		var request = new UpdatePlayerStatisticsRequest();

		request.Statistics = new List<StatisticUpdate>();

		var stat = new StatisticUpdate { StatisticName = "isOnline", Value = onlineValue };

		request.Statistics.Add(stat);

		PlayFabClientAPI.UpdatePlayerStatistics(request, OnResult, OnError);

	}

	private void OnError(PlayFabError obj)
	{
		print("<color=#f51c00>" + "The game has closed." + "</color>");
	}

	private void OnResult(UpdatePlayerStatisticsResult obj)
	{
		
	}
}
