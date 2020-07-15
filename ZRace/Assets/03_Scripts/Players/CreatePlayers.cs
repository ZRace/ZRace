using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

using Photon.Realtime;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Photon.Pun.UtilityScripts
{


    public class CreatePlayers : MonoBehaviour
    {
        public GameObject prefab;
        public Transform spawn;

        private void Start()
        {
            SpawnPlayers();
        }

        void SpawnPlayers()
        {
            PhotonNetwork.Instantiate(prefab.name, spawn.position, Quaternion.identity, 0);
        }

    }
}
