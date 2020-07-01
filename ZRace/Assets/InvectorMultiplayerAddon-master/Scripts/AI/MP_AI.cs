/*
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBGames.AI
{
    [RequireComponent(typeof(PhotonView))]
    public class MP_AI : MonoBehaviourPun
    {
        [Tooltip("The components to disable if this is not owned by you.")]
        [SerializeField] public List<Behaviour> components = new List<Behaviour> { };
        protected Animator _anim = null;

        protected virtual void Start()
        {
            _anim = GetComponent<Animator>();
            StartCoroutine(WaitToInitialize());
        }
        void SetComponentStatus()
        {
            if (photonView.IsMine == true) return;
            foreach (Behaviour component in components)
            {
                component.enabled = false;
            }
        }
        IEnumerator WaitToInitialize()
        {
            yield return new WaitUntil(() => PhotonNetwork.InRoom == true);
            SetComponentStatus();
        }

        [PunRPC]
        void AI_SetTrigger(string triggerName)
        {
            _anim.SetTrigger(triggerName);
        }
    }
}
*/