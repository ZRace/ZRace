using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RememberAccount : MonoBehaviour
{
	public string Username;
	public string Password;

	public TMP_InputField inputName;
	public TMP_InputField inputPassword;

	public GameObject checkImg;
	public Button buttonToRemember;

	public bool canRemember;



	private void Start()
	{
		canRemember = PlayerPrefs2.GetBool("Remember");
		if(canRemember)
		{
			inputName.text = PlayerPrefs.GetString("NickName");
			inputPassword.text = PlayerPrefs.GetString("PasswordAccount");
		}
	}
	public void Update()
	{
		if (canRemember)
		{

			Username = PlayerPrefs.GetString("NickName");
			Password = PlayerPrefs.GetString("PasswordAccount");

			PlayerPrefs.SetString("NickName", inputName.text);
			PlayerPrefs.SetString("PasswordAccount", inputPassword.text);
			PlayerPrefs2.SetBool("Remember", canRemember);

			checkImg.SetActive(true);

			buttonToRemember.onClick.AddListener(DontSaveParameters);
		}
		else
		{
			PlayerPrefs2.SetBool("Remember", canRemember);
			checkImg.SetActive(false);
			buttonToRemember.onClick.AddListener(SaveParameters);
		}
	}


	public void SaveParameters()
	{

		canRemember = true;

	}

	public void DontSaveParameters()
	{
		canRemember = false;
	}
}
