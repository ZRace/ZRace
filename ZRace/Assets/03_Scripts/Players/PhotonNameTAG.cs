using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class PhotonNameTAG : MonoBehaviourPun
{

    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private GameObject panelName;
    

    private void Start()
    {
        if(photonView.IsMine) 
        {
            gameObject.transform.name = photonView.Owner.NickName;
            panelName.SetActive(false);
            return;
        }
        SetName();
    }

    private void SetName()
    {
        nameText.text = photonView.Owner.NickName;
        gameObject.transform.name = photonView.Owner.NickName;
    }
}
