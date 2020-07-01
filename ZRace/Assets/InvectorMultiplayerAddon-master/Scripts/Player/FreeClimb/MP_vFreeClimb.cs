/*
using Invector.vCharacterController.vActions;
using Photon.Pun;
using System.Collections;
using UnityEngine;

namespace CBGames.Player
{
    [RequireComponent(typeof(SyncPlayer))]

    public class MP_vFreeClimb : vFreeClimb
    {
        protected override void Start()
        {
            if (GetComponent<PhotonView>().IsMine == false)
            {
                this.enabled = false;
            }
            else
            {
                base.Start();
            }
        }
        protected override void ClimbJump()
        {
            GetComponent<PhotonView>().RPC("CrossFadeInFixedTime", RpcTarget.OthersBuffered, "ClimbJump", 0.2f);
            base.ClimbJump();
        }
        protected override void EnterClimb()
        {
            var dragPosition = new Vector3(dragInfo.position.x, transform.position.y, dragInfo.position.z) + transform.forward * -TP_Input.cc._capsuleCollider.radius;
            var castObstacleUp = Physics.Raycast(dragPosition + transform.up * TP_Input.cc._capsuleCollider.height, transform.up, TP_Input.cc._capsuleCollider.height * 0.5f, obstacle);
            var castDragableWallForward = Physics.Raycast(dragPosition + transform.up * (TP_Input.cc._capsuleCollider.height * climbUpHeight), transform.forward, out hit, 1f, draggableWall) && draggableTags.Contains(hit.collider.gameObject.tag);
            var climbUpConditions = TP_Input.cc.isGrounded && !castObstacleUp && castDragableWallForward;
            GetComponent<PhotonView>().RPC("CrossFadeInFixedTime", RpcTarget.OthersBuffered, climbUpConditions ? "EnterClimbGrounded" : "EnterClimbAir", 0.2f);
            base.EnterClimb();
        }
        protected override void ExitClimb()
        {
            if (!inClimbUp)
            {
                bool nextGround = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.5f, TP_Input.cc.groundLayer);
                GetComponent<PhotonView>().RPC("CrossFadeInFixedTime", RpcTarget.OthersBuffered, nextGround ? "ExitGrounded" : "ExitAir", 0.2f);
            }
            base.ExitClimb();
        }
        protected override void ClimbUp()
        {
            StartCoroutine(WaitForCrossFade());
            base.ClimbUp();
        }
        IEnumerator WaitForCrossFade()
        {
            var transition = 0f;
            var dir = transform.forward;
            dir.y = 0;
            var angle = Vector3.Angle(Vector3.up, transform.forward);

            var targetRotation = Quaternion.LookRotation(-dragInfo.normal);
            var targetPosition = ((dragInfo.position + dir * -TP_Input.cc._capsuleCollider.radius + Vector3.up * 0.1f) - transform.rotation * handTarget.localPosition);
            while (transition < 1 && Vector3.Distance(targetRotation.eulerAngles, transform.rotation.eulerAngles) > 0.2f && angle < 60)
            {
                yield return null;
            }
            GetComponent<PhotonView>().RPC("CrossFadeInFixedTime", RpcTarget.OthersBuffered, "ClimbUpWall", 0.1f);
        }
    }
}
*/
