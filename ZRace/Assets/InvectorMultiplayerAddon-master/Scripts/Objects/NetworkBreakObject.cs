using UnityEngine;
using Photon.Pun;
using Invector;
using CBGames.Core;
using UnityEngine.SceneManagement;

namespace CBGames.Objects
{
    [RequireComponent(typeof(PhotonView))]
    [DisallowMultipleComponent]
    public class NetworkBreakObject : MonoBehaviour
    {
        [Tooltip("Whether or not you want to sync the break object action across unity scenes.")]
        [SerializeField] protected bool syncCrossScenes = true;
        [Tooltip("The object to track the position of.")]
        [SerializeField] protected Transform holder = null;
        [Tooltip("Whether or not you want to drop a network prefab from the 'Resources' folder when this is broken.")]
        public bool dropPrefab = false;
        [Tooltip("The name of the prefab that lives in the resources folder that you want to drop.")]
        [SerializeField] protected string prefabName = "";
        [Tooltip("Used by 'DropObject' function. Will drop the networked object at this position and rotation")]
        [SerializeField] protected Transform dropPoint = null;
        protected bool isBroken = false;

        public virtual void BreakObject()
        {
            if (isBroken == true) return;
            if (syncCrossScenes == true)
            {
                ObjectAction objectInfo = new ObjectAction(
                    holder.name.Replace("(Clone)", ""),
                    SceneManager.GetActiveScene().name,
                    null,
                    holder.position.Round(),
                    ObjectActionEnum.Update,
                    "SceneBreak", null
                );
                NetworkManager.networkManager.GetChabox().BroadcastData(
                    NetworkManager.networkManager.GetChatDataChannel(),
                    objectInfo
                );
            }
            SceneBreak();
        }

        public virtual void SceneBreak()
        {
            if (isBroken == true) return;
            GetComponent<PhotonView>().RPC("SendNetworkBreakObject", RpcTarget.AllBuffered);
        }

        protected virtual void DropObject()
        {
            if (PhotonNetwork.IsMasterClient == true)
            {
                object[] data = new object[1];
                data[0] = 1;
                NetworkManager.networkManager.NetworkInstantiatePersistantPrefab(
                    prefabName,
                    dropPoint.position,
                    dropPoint.rotation,
                    0,
                    data
                );
            }
        }
        [PunRPC]
        protected virtual void SendNetworkBreakObject()
        {
            if (isBroken == true) return;
            isBroken = true;
            
            if (GetComponent<PhotonRigidbodyView>()) Destroy(GetComponent<PhotonRigidbodyView>());
            if (GetComponent<Rigidbody>()) Destroy(GetComponent<Rigidbody>());
            vDamage breakableDamage = new vDamage();
            breakableDamage.damageValue = 100000;

            if (dropPrefab == true)
            {
                DropObject();
            }
            GetComponent<vBreakableObject>().TakeDamage(breakableDamage);
        }
    }
}
