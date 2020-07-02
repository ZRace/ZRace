/*
using Invector;
using Photon.Pun;
using UnityEngine;

namespace CBGames.Objects
{
    [RequireComponent(typeof(PhotonView))]
    [RequireComponent(typeof(vSimpleTrigger))]
    public class MP_vSimpleTrigger : MonoBehaviour
    {
        private vSimpleTrigger trigger;
        private bool networkEnabled = false;

        private void Awake()
        {
            trigger = GetComponent<vSimpleTrigger>();
            networkEnabled = trigger.onTriggerEnter.GetPersistentEventCount() > 0 || trigger.onTriggerExit.GetPersistentEventCount() > 0;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (networkEnabled == false) return;
            bool inLayer = trigger.layersToDetect == (trigger.layersToDetect | (1 << other.gameObject.layer));
            if (trigger.tagsToDetect.Contains(other.tag) || inLayer)
            {
                if (other.GetComponent<PhotonView>())
                {
                    GetComponent<PhotonView>().RPC("SyncTriggerEnter", RpcTarget.Others, other.GetComponent<PhotonView>().ViewID);
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (networkEnabled == false) return;
            bool inLayer = trigger.layersToDetect == (trigger.layersToDetect | (1 << other.gameObject.layer));
            if (trigger.tagsToDetect.Contains(other.tag) || inLayer)
            {
                if (other.GetComponent<PhotonView>())
                {
                    GetComponent<PhotonView>().RPC("SyncTriggerExit", RpcTarget.Others, other.GetComponent<PhotonView>().ViewID);
                }
            }
        }

        [PunRPC]
        public void SyncTriggerEnter(int viewId)
        {
            PhotonView view = PhotonView.Find(viewId);
            if (view)
            {
                Collider col = view.transform.GetComponent<Collider>();
                if (col)
                {
                    trigger.onTriggerEnter.Invoke(col);
                }
            }
        }
        [PunRPC]
        public void SyncTriggerExit(int viewId)
        {
            PhotonView view = PhotonView.Find(viewId);
            if (view)
            {
                Collider col = view.transform.GetComponent<Collider>();
                if (col)
                {
                    trigger.onTriggerExit.Invoke(col);
                }
            }
        }
    }
}
*/
