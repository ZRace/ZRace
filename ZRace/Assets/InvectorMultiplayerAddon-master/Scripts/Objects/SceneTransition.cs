using CBGames.Core;
using Invector.vCharacterController;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace CBGames.Objects
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(PhotonView))]
    public class SceneTransition : MonoBehaviour
    {
        #region Modifiables 
        [Tooltip("When entring this trigger whether or not to automatically move to the new scene or not.")]
        [SerializeField] protected bool autoTravel = false;
        [Tooltip("The button that must be pressed when inside this trigger to travel the new scene.")]
        [SerializeField] protected string buttonToTravel = "E";
        [Tooltip("The gameobjects to active/deactive when entering/exiting this trigger.")]
        [SerializeField] protected GameObject[] activeOnEnter = null;
        [Tooltip("The name of the scene to load. This must be an exact spelling according to what is in the build settings.")]
        [SerializeField] protected string LoadSceneName = "";
        [Tooltip("The name of the LoadPoint object to spawn at in the desired scene. This naming must be exact.")]
        [SerializeField] protected string SpawnAtPoint = "";
        [Tooltip("Whether to send everyone when traveling or to just send the person entering.")]
        [SerializeField] public bool sendAllTogether = true;
        [Tooltip("The scene database that holds a list of all scenes and LoadPoint objects in those scenes.")]
        [SerializeField] private SceneDatabase database;
        [SerializeField] private UnityEvent OnOwnerEnterTrigger = null;
        [SerializeField] private UnityEvent OnOwnerExitTrigger = null;
        [SerializeField] private UnityEvent BeforeTravel = null;
        [SerializeField] private UnityEvent OnAnyPlayerEnterTrigger = null;
        [SerializeField] private UnityEvent OnAnyPlayerExitTrigger = null;
        #endregion

        #region Internal Variables
        protected bool acceptingInput = false;
        protected Component comp;
        protected Color gizmoColor = new Color(1f, 0.92f, 0.016f, 0.3f);
        protected Color gizmoError = Color.red;
        protected GameObject targetPlayer;
        #endregion
        
        //Used For Automation
        public virtual void SetDatabase(SceneDatabase input)
        {
            database = input;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<vThirdPersonController>())
            {
                OnAnyPlayerEnterTrigger.Invoke();
                if (other.GetComponent<PhotonView>() && other.GetComponent<PhotonView>().IsMine)
                {
                    OnOwnerEnterTrigger.Invoke();
                    if (autoTravel && other.GetComponent<PhotonView>())
                    {
                        targetPlayer = other.gameObject;
                        Travel();
                        return;
                    }
                    else
                    {
                        acceptingInput = true;
                        foreach (GameObject go in activeOnEnter)
                        {
                            go.SetActive(true);
                        }
                    }
                }
            }
        }
        protected virtual void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<vThirdPersonController>())
            {
                OnAnyPlayerExitTrigger.Invoke();
                if (other.GetComponent<PhotonView>() && other.GetComponent<PhotonView>().IsMine)
                {
                    OnOwnerExitTrigger.Invoke();
                    if (autoTravel) return;
                    targetPlayer = other.gameObject;
                    acceptingInput = false;
                    foreach (GameObject go in activeOnEnter)
                    {
                        go.SetActive(false);
                    }
                }
            }
        }
        protected virtual void Update()
        {
            if (acceptingInput && Input.GetButtonDown(buttonToTravel.ToString()))
            {
                Travel();
            }
        }

        public virtual void Travel()
        {
            acceptingInput = false;
            if (sendAllTogether)
            {
                GetComponent<PhotonView>().RPC("SendToScene", RpcTarget.All, SpawnAtPoint);
            }
            else
            {
                BeforeTravel.Invoke();
                NetworkManager.networkManager.NetworkLoadLevel(database.storedScenesData.Find(x => x.sceneName == LoadSceneName).index, SpawnAtPoint, sendAllTogether);
            }
        }

        [PunRPC]
        protected virtual void SendToScene(string entrancePoint)
        {
            sendAllTogether = false;
            Travel();
        }

        //Draw trigger box
        protected virtual void OnDrawGizmos()
        {
            if (GetComponent<BoxCollider>())
            {
                if (gameObject.GetComponent<BoxCollider>().isTrigger && gameObject.layer == 2 && database)
                {
                    Gizmos.color = gizmoColor;
                }
                else
                {
                    Gizmos.color = gizmoError;
                }
                gameObject.GetComponent<BoxCollider>().center = Vector3.zero;
                gameObject.GetComponent<BoxCollider>().size = Vector3.one;

                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawCube(Vector3.zero, Vector3.one);
            }
        }
    }
}