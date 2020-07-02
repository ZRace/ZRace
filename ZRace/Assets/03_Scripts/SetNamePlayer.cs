using UnityEngine;
using Photon.Pun;
using PlayFab.ClientModels;
using PlayFab;
using TMPro;
using Photon.Pun.UtilityScripts;
using System;

public class SetNamePlayer : MonoBehaviourPun
{
    [SerializeField]
    private TMP_InputField nickname;

    private const string PlayerPrefsNameKey = "PlayerName";

    public GameObject buttonInterectable;


    private void Start()
    {
        buttonInterectable = GameObject.FindGameObjectWithTag("GameController");
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
            buttonInterectable.GetComponent<AutoLobby>().ButtonInterectable();
            SavePlayerName();
        }
    }
    public void SavePlayerName()
    {
        string playerName = nickname.text;

        PhotonNetwork.NickName = playerName;
    }
}
