using Photon.Pun;

namespace CBGames.Player
{
    public class MP_vSimpleInput : vSimpleInput
    {

        void Update()
        {
            if (GetComponent<PhotonView>().IsMine)
            {
                if (input.GetButtonDown() && gameObject.activeSelf)
                {
                    if (disableThisObjectAfterInput)
                    {
                        this.gameObject.SetActive(false);
                    }

                    GetComponent<PhotonView>().RPC("NetworkOnPressInput", RpcTarget.All);
                }
            }
        }
        [PunRPC]
        void NetworkOnPressInput()
        {
            OnPressInput.Invoke();
        }
    }
}