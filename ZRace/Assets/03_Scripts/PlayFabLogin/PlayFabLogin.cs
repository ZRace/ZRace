﻿using UnityEngine;
using System.Collections.Generic;
using PlayFab.ClientModels;
using PlayFab;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class PlayFabLogin : MonoBehaviour
{
	[Tooltip("Input field of register email")]
	[HideInInspector] public TMP_InputField regEmail;
	[Tooltip("Input field of register username")]
	[HideInInspector] public TMP_InputField regUsername;
	[Tooltip("Input field of register password")]
	[HideInInspector] public TMP_InputField regPassword;


	[Tooltip("Input field of log in Username")]
	[HideInInspector] public TMP_InputField logUsername;
	[Tooltip("Input field of log in Password")]
	[HideInInspector] public TMP_InputField logPassword;
	[Tooltip("If number it's 0 = no online, else if number it's 1 = online")]
	[HideInInspector] public int isOnline;


	[Tooltip("Text for error messages")]
	[HideInInspector] public TextMeshProUGUI errorText;


	[Tooltip("String variable to save ID of user")]
	[HideInInspector] public string playFabID;
	[Tooltip("String variable to save contact email of user")]
	[HideInInspector] public string contactEmail;
	[Tooltip("String variable to save display name of user")]
	[HideInInspector] public string displayName;
	[Tooltip("Scene number to load")]
	[HideInInspector] public string loadScene;


	#region ConntectAPI
	private void Start()
	{
		// Crea una solicitud y lo almazena en una variable
		var request = new LoginWithCustomIDRequest { CustomId = "16175", CreateAccount = true };

		// Logeo del API
		PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
	}

	private void OnLoginFailure(PlayFabError obj)
	{
		Debug.Log("Type of error: " + "<color=#f51c00>" + obj.Error + "</color>");
	}

	private void OnLoginSuccess(LoginResult obj)
	{
		Debug.Log("<color=#00bc04>" + "Login success" + "</color>");
	}
	#endregion

	#region Register
	public void Register()
	{
		var request = new RegisterPlayFabUserRequest();

		request.TitleId = PlayFabSettings.TitleId;

		// Solicitudes de acceso
		request.Email = regEmail.text;
		request.Username = regUsername.text;
		request.Password = regPassword.text;
		request.DisplayName = request.Username;
		contactEmail = request.Email;


		// Entrega la solicitud de registro
		PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterResult, OnPlayFabError);
	}

	private void OnPlayFabError(PlayFabError obj)
	{
		print("Type of error: " + "<color=#f51c00>" + obj.Error + "</color>");
		errorText.text = obj.Error.ToString();

		string output = "";

		// Creamos todos los posibles casos de errores, con mensajes personalizados
		// Si no aparece ninguno de ellos, el mensaje de error sera el que viene definido por playfab
		switch(obj.Error)
		{
			case PlayFabErrorCode.InvalidParams:
				output = "Email is wrong";
				break;
			case PlayFabErrorCode.AccountBanned:
				output = "Your account it's banned";
				break;
			case PlayFabErrorCode.AccountDeleted:
				output = "Your account it's removed";
				break;
			case PlayFabErrorCode.AccountNotFound:
				output = "Please register to log in";
				break;
			case PlayFabErrorCode.InvalidUsernameOrPassword:
				output = "Username or password it's incorrect";
				break;
			case PlayFabErrorCode.InvalidUsername:
				output = "Username it's incorrect";
				break;
			case PlayFabErrorCode.UsernameNotAvailable:
				output = "This usernames not available";
				break;
			case PlayFabErrorCode.NameNotAvailable:
				output = "This usernames not available";
				break;
			case PlayFabErrorCode.EmailAddressNotAvailable:
				output = "This email not available";
				break;
		}

		errorText.text = output;
	}

	private void OnRegisterResult(RegisterPlayFabUserResult obj)
	{
		print("<color=#00bc04>" + "Registro aceptado" + "</color>");
		AddOrUpdateContactEmail(); // Llamamos a esta funcion para añadir o updatear el correo de contacto
	}
	#endregion

	#region LogIn
	public void LogIn()
	{
		var request = new LoginWithPlayFabRequest();

		request.TitleId = PlayFabSettings.TitleId;
		request.Username = logUsername.text;
		request.Password = logPassword.text;

		PlayFabClientAPI.LoginWithPlayFab(request, OnLoginResult, OnPlayFabError);

	}

	private void OnLoginResult(LoginResult obj)
	{
		print("<color=#00bc04>" + "Inicio de sesión válido" + "</color>");
		playFabID = obj.PlayFabId;

		var request = new GetPlayerStatisticsRequest();

		request.StatisticNames = new List<string>() { "isOnline" };


		PlayFabClientAPI.GetPlayerStatistics(request, OnOnlineResult, OnOnlineError);

	}

	private void OnOnlineResult(GetPlayerStatisticsResult obj)
	{
		foreach(var stat in obj.Statistics)
		{
			isOnline = stat.Value;
		}
		
		if (isOnline == 0)
		{
			isOnline = 1;
			var request = new UpdatePlayerStatisticsRequest();

			request.Statistics = new List<StatisticUpdate>();

			var stat = new StatisticUpdate { StatisticName = "isOnline", Value = isOnline };

			request.Statistics.Add(stat);
			PlayFabClientAPI.UpdatePlayerStatistics(request, Continue, OnPlayFabError);
			//Continue();
		}
		else
		{
			print("<color=#f51c00>" + "Esta cuenta esta en online" + "</color>");
			errorText.text = "This account is online.";
		}
	}

	private void OnOnlineError(PlayFabError obj)
	{
		
	}

	private void Continue(UpdatePlayerStatisticsResult obj)
	{
		GetAccountInfo(); // Llamamos a esta funcion para obtener la informacion del usuario que logea
	}
	#endregion

	#region GetDataUsers
	public void AddOrUpdateContactEmail()
	{
		var request = new AddOrUpdateContactEmailRequest();
		request.EmailAddress = regEmail.text;

		PlayFabClientAPI.AddOrUpdateContactEmail(request, OnContactEmailResult, OnPlayFabError);
	}

	private void OnContactEmailResult(AddOrUpdateContactEmailResult obj)
	{
		print("<color=#00bc04>" + "Your email addres has been added to the contact email" + "</color>");
	}



	private void GetPlayerData()
	{
		var request = new GetPlayerProfileRequest();
		request.PlayFabId = playFabID;

		var constraints = new PlayerProfileViewConstraints();
		constraints.ShowContactEmailAddresses = true;

		request.ProfileConstraints = constraints;

		PlayFabClientAPI.GetPlayerProfile(request, OnPlayerDataResult, OnPlayFabError);
	}

	private void OnPlayerDataResult(GetPlayerProfileResult obj)
	{
		var myList = obj.PlayerProfile.ContactEmailAddresses;

		if (myList[0].VerificationStatus != EmailVerificationStatus.Confirmed)
		{
			print("<color=#f51c00>" + "Confirma tu cuenta." + "</color>");
			errorText.text = "Please verify your account";
		}
		else
		{
			print("<color=#00bc04>" + "Your account is verified" + "</color>");
			InfoPlayerCorrect();
		}
	}
	#endregion

	#region GetAccountInfo
	public void GetAccountInfo()
	{
		var request = new GetAccountInfoRequest();
		PlayFabClientAPI.GetAccountInfo(request, OnInfoResult, OnPlayFabError);
		
	}

	private void OnInfoResult(GetAccountInfoResult obj)
	{
		displayName = obj.AccountInfo.TitleInfo.DisplayName.ToString();
		contactEmail = obj.AccountInfo.PrivateInfo.Email.ToString();
		print("<color=#00bc04>" + displayName + "</color>");
		GetPlayerData();
	}
	#endregion

	public void InfoPlayerCorrect()
	{
		SceneManager.LoadScene(loadScene);
	}
}
