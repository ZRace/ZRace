/*
using CBGames.Core;
using CBGames.Player;
using Invector;
using Invector.vShooter;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBGames.Objects
{
    public class ArrowView : MonoBehaviour
    {
        public int viewId = 0;
        public Transform owner;

        public virtual void NetworkSetPosRot(RaycastHit hit)
        {
            if (!hit.transform.GetComponentInParent<SyncPlayer>()) return;
            if (hit.transform.GetComponentInParent<PhotonView>().IsMine == false) return;

            int[] tree = StaticMethods.BuildChildTree(hit.transform.root, hit.transform, false);
            
            hit.transform.GetComponentInParent<PhotonView>().RPC(
                "SetArrowPositionRotation",
                RpcTarget.Others,
                viewId,
                tree,
                JsonUtility.ToJson(transform.position),
                JsonUtility.ToJson(transform.rotation)
            );
        }
    }
}
*/
