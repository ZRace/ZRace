using UnityEngine;


namespace Photon.Pun.Demo.PunBasics
{
    public class zombiesIsMine : MonoBehaviourPunCallbacks
    {
        [Tooltip("El objeto del jugador local. Usar para saber si el jugador local esta en la escena.")]
        public static GameObject LocalPlayerInstance;

        public void Awake()
        {

            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instanciation when levels are synchronized
            if (photonView.IsMine)
            {
                LocalPlayerInstance = gameObject;
            }

            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(gameObject);
        }
    }
}
