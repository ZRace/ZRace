using CBGames.Core;
using Photon.Pun;
using UnityEngine;

public class EnableIfTeam : MonoBehaviour
{
    public enum ChangeType { ForEveryone, IfOwner, IfNotOwner }

    [SerializeField] protected ChangeType checkType = ChangeType.ForEveryone;
    [Tooltip("Enable the selected items if you are on the team name.")]
    [SerializeField] protected string teamName = "";
    [SerializeField] protected GameObject[] items = new GameObject[] { };

    protected virtual void Update()
    {
        switch(checkType)
        {
            case ChangeType.ForEveryone:
                EnableItems(NetworkManager.networkManager.teamName == teamName);
                break;
            case ChangeType.IfNotOwner:
                if (PhotonNetwork.IsMasterClient == false)
                {
                    EnableItems(NetworkManager.networkManager.teamName == teamName);
                }
                break;
            case ChangeType.IfOwner:
                if (PhotonNetwork.IsMasterClient == true)
                {
                    EnableItems(NetworkManager.networkManager.teamName == teamName);
                }
                break;
        }
    }
    protected virtual void EnableItems(bool isEnabled)
    {
        foreach(GameObject item in items)
        {
            item.SetActive(isEnabled);
        }
    }
}
