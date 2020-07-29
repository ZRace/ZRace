using UnityEngine;
using Photon.Pun;
using PlayFab.ClientModels;
using PlayFab;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;
using System;
using CBGames.UI;
using UnityEngine.Events;

public class SetNamePlayerV2 : MonoBehaviourPun
{
    [SerializeField]
    [HideInInspector]private InputField nickname;

    [HideInInspector] public UICoreLogic setNamePlayer;

    [HideInInspector] public UnityEvent OnNameResult;


    private void Start()
    {
        GetAccountInfo();
    }


    public void GetAccountInfo()
    {
        var request = new GetAccountInfoRequest();
        PlayFabClientAPI.GetAccountInfo(request, OnInfoResult, OnPlayFabError);

    }

    private void OnPlayFabError(PlayFabError obj)
    {
        
    }

    private void OnInfoResult(GetAccountInfoResult obj)
    {
        nickname.text = obj.AccountInfo.TitleInfo.DisplayName.ToString();
        print("<color=#00bc04>" + nickname.text + "</color>");
    }


    private void Update()
    {

        if (!string.IsNullOrEmpty(nickname.text))
        {
            SaveName();
        }
    }
    public void SaveName()
    {
        string playerName = nickname.text;

        setNamePlayer.SavePlayerName(playerName);
        OnNameResult.Invoke();
    }
}
