using UnityEngine;
using Photon.Pun;
using Invector.vItemManager;
using System.Collections;
using UnityEngine.Events;
using Invector.vCharacterController.vActions;
using CBGames.Core;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace CBGames.Objects
{
    [RequireComponent(typeof(PhotonView))]
    [DisallowMultipleComponent]
    public class SyncItemCollection : MonoBehaviour
    {
        #region Parameters
        [Tooltip("Whether or not you want this to be a globally updated item. (EX: If a player " +
            "in scene 1 collects this then when other players enter this scene they will see " +
            "this as already collected.)")]
        [SerializeField] protected bool syncCrossScenes = true;
        [Tooltip("This is only important if \"syncCrossScenes\" is true. \n\nThe object you want to track " +
            "the position of. When trying to sync this item across the scene it uses it's name and this " +
            "holder position to try and figure out what object to update.")]
        [SerializeField] protected Transform holder = null;
        [Tooltip("Only matters if \"Sync Cross Scenes\" is true.\n\n" +
            "Enable if this object is dynamically added to the scene. Will instantiate/destroy this object across " +
            "Unity Scenes and for all networked players.")]
        [SerializeField] protected bool syncCreateDestroy = false;
        [Tooltip("Only matters if \"Sync Cross Scenes\" is true.\n\n" +
            "The prefab name in the resources folder that corresponds to this object. Must be exact in " +
            "order to spawn from the resources folder.")]
        [SerializeField] protected string resourcesPrefab = null;
        [Tooltip("If false, will check to make sure this has been instantiated with item data via the network call. " +
            "If not it will destroy itself in favor of another ItemCollection. Enable this ONLY if you DON'T dynamically " +
            "add items to the ItemCollection component.")]
        [SerializeField] protected bool skipStartCheck = false;
        [Tooltip("This should be copied exactly from vItemCollection.")]
        public float onPressActionDelay;

        public UnityEvent OnPressActionInput;
        public OnDoActionWithTarget onPressActionInputWithTarget;
        public UnityEvent OnSceneEnterUpdate;

        protected vItemCollection ic = null;
        protected bool collected = false;
        [SerializeField] List<ItemReference> references = new List<ItemReference>();
        #endregion

        #region Initializations
        protected virtual void Awake()
        {
            ic = (ic == null) ? GetComponent<vItemCollection>() : ic;
        }
        protected virtual void Start()
        {
            if (skipStartCheck == false)
            {
                object[] data = GetComponent<PhotonView>().InstantiationData;
                if (PhotonNetwork.InRoom == true && data == null && syncCreateDestroy == true)
                {
                    Destroy(gameObject);
                }
                else if (PhotonNetwork.InRoom == true && syncCreateDestroy == true)
                {
                    ItemWrapper wrapper = JsonUtility.FromJson<ItemWrapper>(data[0] as string);
                    GetComponent<vItemCollection>().items = wrapper.items;
                }
            }
        }
        #endregion

        #region SYNC Create/Destroys
        protected virtual string[] SerializeItems(List<ItemReference> items)
        {
            string[] data = new string[1];
            ItemWrapper wrapper = new ItemWrapper(items);
            data[0] = JsonUtility.ToJson(wrapper);
            return data;
        }
        public virtual List<ItemReference> ConvertBackToItemRefs(string[] serializedItems)
        {
            ItemWrapper wrapper = JsonUtility.FromJson<ItemWrapper>(serializedItems[0]);
            return wrapper.items;
        }
        public virtual void UpdateDatabase(List<ItemReference> items)
        {
            StartCoroutine(UpdateScenesDatabase(items));
        }
        protected virtual IEnumerator UpdateScenesDatabase(List<ItemReference> items)
        {
            yield return new WaitUntil(() => NetworkManager.networkManager.GetChabox() && NetworkManager.networkManager.GetChabox().IsConnectedToDataChannel() == true);
            ObjectAction objectInfo = new ObjectAction(
                holder.name.Replace("(Clone)", ""),
                SceneManager.GetActiveScene().name,
                resourcesPrefab,
                holder.position.Round(),
                ObjectActionEnum.Create,
                null,
                SerializeItems(items)
            );
            NetworkManager.networkManager.GetChabox().BroadcastData(
                NetworkManager.networkManager.GetChatDataChannel(),
                objectInfo
            );
        }
        #endregion

        #region SYNC Updates
        public virtual void SceneCollected()
        {
            GetComponent<PhotonView>().RPC("NetworkSceneCollected", RpcTarget.AllBuffered);
        }
        public virtual void EnableVItemCollection(bool isEnabled)
        {
            GetComponent<vItemCollection>().enabled = isEnabled;
        }
        #endregion

        #region SYNC Update/Create/Delete
        public virtual void Collect()
        {
            if (collected == false)
            {
                collected = true;
                GetComponent<PhotonView>().RPC("NetworkCollect", RpcTarget.OthersBuffered);
                ChatBoxBroadCastCollect();
                StartCoroutine(E_OnPressAction(null, true));
            }
        }
        public virtual void Collect(GameObject target = null)
        {
            if (collected == false)
            {
                collected = true;
                if (string.IsNullOrEmpty(ic.playAnimation) == false)
                {
                    GetComponent<PhotonView>().RPC("NetworkPlayerPlayAnimation", RpcTarget.Others, target.GetComponent<PhotonView>().ViewID);
                }
                if (GetComponent<PhotonView>().ViewID != 0)
                {
                    GetComponent<PhotonView>().RPC("NetworkCollect", RpcTarget.OthersBuffered, target.GetComponent<PhotonView>().ViewID);
                }
                ChatBoxBroadCastCollect();
                StartCoroutine(E_OnPressAction(target, true));
            }
        }       
        public virtual void ChatBoxBroadCastCollect()
        {
            if (syncCrossScenes == true && NetworkManager.networkManager.GetChabox() && 
                NetworkManager.networkManager.GetChabox().IsConnectedToDataChannel())
            {
                if (syncCreateDestroy == true)
                {
                    ObjectAction objectInfo = new ObjectAction(
                        holder.name.Replace("(Clone)", ""),
                        SceneManager.GetActiveScene().name,
                        resourcesPrefab,
                        holder.position.Round(),
                        ObjectActionEnum.Delete,
                        null, null
                    );
                    NetworkManager.networkManager.GetChabox().BroadcastData(
                        NetworkManager.networkManager.GetChatDataChannel(),
                        objectInfo
                    );
                }
                else
                {
                    ObjectAction objectInfo = new ObjectAction(
                        holder.name,
                        SceneManager.GetActiveScene().name,
                        resourcesPrefab,
                        holder.position,
                        ObjectActionEnum.Update,
                        "SceneCollected",
                        null
                    );
                    NetworkManager.networkManager.GetChabox().BroadcastData(
                        NetworkManager.networkManager.GetChatDataChannel(),
                        objectInfo
                    );
                }
            }
        }
        protected virtual IEnumerator E_OnPressAction(GameObject target = null, bool sendNetwork = true)
        {
            if (onPressActionDelay > 0) yield return new WaitForSeconds(onPressActionDelay);
            OnPressActionInput.Invoke();
            if (target != null)
            {
                onPressActionInputWithTarget.Invoke(target);
            }
            if (sendNetwork == true)
            {
                if (syncCreateDestroy == true)
                {
                    if (PhotonNetwork.IsMasterClient == true)
                    {
                        PhotonNetwork.Destroy(this.gameObject);
                    }
                    else
                    {
                        GetComponent<PhotonView>().RPC("NetworkDestroySelf", RpcTarget.MasterClient);
                    }
                }
            }
            else if (GetComponent<vItemCollection>().destroyAfter == true)
            {
                yield return new WaitForSeconds(GetComponent<vItemCollection>().destroyDelay);
                if (gameObject)
                {
                    Destroy(gameObject);
                }
            }
        }
        #endregion

/*        #region Shooter Template
        public virtual void NetworkArrowViewDestroy()
        {
            Transform you = NetworkManager.networkManager.GetYourPlayer().transform;
            you.GetComponent<PhotonView>().RPC(
                "DestroyArrow",
                RpcTarget.Others,
                GetComponent<ArrowView>().viewId
            );
        }
        #endregion*/

        #region NetworkEvents
        public virtual Transform GetHolder()
        {
            return holder;
        }

        [PunRPC]
        protected virtual void NetworkPlayerPlayAnimation(int viewId)
        {
            PhotonView pv = PhotonView.Find(viewId);
            if (pv && pv.gameObject)
            {
                pv.gameObject.GetComponent<Animator>().Play(ic.playAnimation, ic.animatorActionState);
            }
        }
        [PunRPC]
        protected virtual void NetworkCollect(int viewId)
        {
            if (collected == true) return;
            collected = true;
            PhotonView pv = PhotonView.Find(viewId);
            if (pv && pv.gameObject)
            {
                StartCoroutine(E_OnPressAction(pv.gameObject, false));
            }
            else
            {
                StartCoroutine(E_OnPressAction(null, false));
            }
        }
        [PunRPC]
        protected virtual void NetworkCollect()
        {
            if (collected == true) return;
            collected = true;
            StartCoroutine(E_OnPressAction(null, false));
        }
        [PunRPC]
        protected virtual void NetworkDestroySelf()
        {
            if (PhotonNetwork.IsMasterClient == true)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
        [PunRPC]
        protected virtual void NetworkSceneCollected()
        {
            if (collected == false)
            {
                collected = true;
                OnSceneEnterUpdate.Invoke();
            }
        }
        #endregion
    }
}
