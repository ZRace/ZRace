using NWH.VehiclePhysics2;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PhotonLiveRemote : MonoBehaviourPun, IPunObservable
{
    public float liveValue;
    public Slider liveSlider;

    private VehicleController _vc;


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(liveValue);
        }
        else if (stream.IsReading)
        {
            liveValue = (float)stream.ReceiveNext();
        }
    }


    public void SetLive()
    {
        if(photonView.IsMine)
        {
            photonView.RPC("Live", RpcTarget.All);
        }

    }

    private void FixedUpdate()
    {
        liveSlider.value = -liveValue;
        if(photonView.IsMine)
        {
            liveSlider.gameObject.SetActive(false);
        }
    }

    [PunRPC]
    void Live()
    {
        _vc = GetComponent<VehicleController>();
        liveValue = _vc.damageHandler.Damage;
    }


}
