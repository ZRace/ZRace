using Photon.Pun;
using UnityEngine;

namespace CBGames.UI
{
    public class EnableIfAllPlayersReady : MonoBehaviour
    {
        [SerializeField] protected bool mustBeOnwer = false;
        [SerializeField] protected GameObject[] targets = new GameObject[] { };
        protected UICoreLogic logic;

        protected virtual void Start()
        {
            logic = FindObjectOfType<UICoreLogic>();
        }

        protected virtual void FixedUpdate()
        {
            if (mustBeOnwer == true && PhotonNetwork.IsMasterClient != mustBeOnwer)
            {
                EnableTargets(false);
            }
            else
            {
                EnableTargets(logic.AllPlayersReady());
            }
        }

        protected virtual void EnableTargets(bool isEnabled)
        {
            foreach(GameObject target in targets)
            {
                if (target.activeInHierarchy != isEnabled)
                {
                    target.SetActive(isEnabled);
                }
            }
        }
    }
}