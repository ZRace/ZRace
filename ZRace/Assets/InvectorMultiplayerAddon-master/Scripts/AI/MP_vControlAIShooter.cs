/*
using Photon.Pun;
using UnityEngine;

namespace Invector.vCharacterController.AI
{
    public class MP_vControlAIShooter : vControlAIShooter, IPunObservable
    {
        protected Transform _leftUpperArm, _rightUpperArm, _leftHand, _rightHand;

        protected override void Start()
        {
            base.Start();
            _leftUpperArm = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
            _rightUpperArm = animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
            _leftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);
            _rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);
        }

        public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            try
            {
                if (PhotonNetwork.InRoom == false) return;
                if (stream.IsWriting)
                {
                    #region IK
                    stream.SendNext(upperArmRotation);
                    stream.SendNext(handRotation);
                    stream.SendNext(armAlignmentWeight);
                    stream.SendNext(handIKWeight);
                    stream.SendNext(handRotationAlignment);
                    stream.SendNext(upperArmRotationAlignment);
                    stream.SendNext(_onlyArmsLayerWeight);
                    stream.SendNext(weaponIKWeight);
                    stream.SendNext(rightRotationWeight);
                    #endregion

                    #region Actions
                    stream.SendNext(isCrouching);
                    stream.SendNext(ragdolled);
                    stream.SendNext(isInCombat);
                    stream.SendNext(isAiming);
                    stream.SendNext(isBlocking);
                    stream.SendNext(isJumping);
                    stream.SendNext(isGrounded);
                    stream.SendNext(isRolling);
                    #endregion

                    #region Weapons/Targets
                    stream.SendNext(aimPosition);
                    stream.SendNext(aimTarget);
                    stream.SendNext(lastTargetPosition);
                    #endregion

                    stream.SendNext(currentHealth);
                }
                else if (stream.IsReading)
                {
                    #region IK
                    upperArmRotation = (Quaternion)stream.ReceiveNext();
                    handRotation = (Quaternion)stream.ReceiveNext();
                    armAlignmentWeight = (float)stream.ReceiveNext();
                    handIKWeight = (float)stream.ReceiveNext();
                    handRotationAlignment = (Quaternion)stream.ReceiveNext();
                    upperArmRotationAlignment = (Quaternion)stream.ReceiveNext();
                    _onlyArmsLayerWeight = (float)stream.ReceiveNext();
                    weaponIKWeight = (float)stream.ReceiveNext();
                    rightRotationWeight = (float)stream.ReceiveNext();
                    #endregion

                    #region Actions
                    isCrouching = (bool)stream.ReceiveNext();
                    ragdolled = (bool)stream.ReceiveNext();
                    isInCombat = (bool)stream.ReceiveNext();
                    isAiming = (bool)stream.ReceiveNext();
                    isBlocking = (bool)stream.ReceiveNext();
                    isJumping = (bool)stream.ReceiveNext();
                    isGrounded = (bool)stream.ReceiveNext();
                    isRolling = (bool)stream.ReceiveNext();
                    #endregion

                    #region Weapons/Targets
                    aimPosition = (Vector3)stream.ReceiveNext();
                    aimTarget = (Vector3)stream.ReceiveNext();
                    lastTargetPosition = (Vector3)stream.ReceiveNext();
                    #endregion

                    currentHealth = (float)stream.ReceiveNext();
                }
            }
            catch { }
        }

        #region IK
        protected override void UpdateAimBehaviour()
        {
            if (GetComponent<PhotonView>().IsMine == true)
            {
                base.UpdateAimBehaviour();
            }
            else
            {
                UpdateHeadTrack();
                HandleShots();
            }
        }
        protected override void RotateAimArm(bool isUsingLeftHand = false)
        {
            if (GetComponent<PhotonView>().IsMine == true)
            {
                base.RotateAimArm(isUsingLeftHand);
            }
            else if (CurrentActiveWeapon && armAlignmentWeight > 0.1f && CurrentActiveWeapon.alignRightUpperArmToAim)
            {
                var upperArm = isUsingLeftHand ? _leftUpperArm : _rightUpperArm;
                if (!float.IsNaN(upperArmRotation.x) && !float.IsNaN(upperArmRotation.y) && !float.IsNaN(upperArmRotation.z))
                    upperArm.localRotation *= Quaternion.Euler(upperArmRotation.eulerAngles.NormalizeAngle() * armAlignmentWeight);
            }
        }
        protected override void RotateAimHand(bool isUsingLeftHand = false)
        {
            if (GetComponent<PhotonView>().IsMine == true)
            {
                base.RotateAimHand(isUsingLeftHand);
            }
            else if (CurrentActiveWeapon && armAlignmentWeight > 0.1f && _canAiming && CurrentActiveWeapon.alignRightHandToAim)
            {
                var hand = isUsingLeftHand ? _leftHand : _rightHand;
                if (!float.IsNaN(handRotation.x) && !float.IsNaN(handRotation.y) && !float.IsNaN(handRotation.z))
                    hand.localRotation *= Quaternion.Euler(handRotation.eulerAngles.NormalizeAngle() * armAlignmentWeight);
            }
        }
        #endregion

        #region Actions
        public override void Attack(bool strongAttack = false, int attackID = -1, bool forceCanAttack = false)
        {
            if (GetComponent<PhotonView>().IsMine == true)
            {
                GetComponent<PhotonView>().RPC("vControlAIShooter_Attack", RpcTarget.Others, strongAttack, attackID, forceCanAttack);
                base.Attack(strongAttack, attackID, forceCanAttack);
            }
        }
        public override void RollTo(Vector3 direction)
        {
            if (GetComponent<PhotonView>().IsMine == true)
            {
                if (!inTurn && !isRolling && !isJumping && isGrounded && !lockMovement && !customAction)
                {
                    GetComponent<PhotonView>().RPC("vControlAIShooter_RollTo", RpcTarget.Others, direction);
                }
                base.RollTo(direction);
            }
        }
        protected override void TriggerDamageRection(vDamage damage)
        {
            if (GetComponent<PhotonView>().IsMine == true)
            {
                if (!isRolling)
                {
                    if (animator != null && animator.enabled && !damage.activeRagdoll && currentHealth > 0)
                    {
                        if (damage.hitReaction)
                        {
                            if (triggerReactionHash.isValid && triggerResetStateHash.isValid)
                            {
                                GetComponent<PhotonView>().RPC("vControlAIShooter_Triggers", RpcTarget.Others, new int[] { triggerReactionHash, triggerResetStateHash });
                            }
                            else if (triggerReactionHash.isValid)
                            {
                                GetComponent<PhotonView>().RPC("vControlAIShooter_Triggers", RpcTarget.Others, new int[] { triggerReactionHash });
                            }
                            else if (triggerResetStateHash.isValid)
                            {
                                GetComponent<PhotonView>().RPC("vControlAIShooter_Triggers", RpcTarget.Others, new int[] { triggerResetStateHash });
                            }
                        }
                        else
                        {
                            if (triggerRecoilHash.isValid && triggerResetStateHash.isValid)
                            {
                                GetComponent<PhotonView>().RPC("vControlAIShooter_Triggers", RpcTarget.Others, new int[] { triggerRecoilHash, triggerResetStateHash });
                            }
                            else if (triggerRecoilHash.isValid)
                            {
                                GetComponent<PhotonView>().RPC("vControlAIShooter_Triggers", RpcTarget.Others, new int[] { triggerRecoilHash });
                            }
                            else if (triggerResetStateHash.isValid)
                            {
                                GetComponent<PhotonView>().RPC("vControlAIShooter_Triggers", RpcTarget.Others, new int[] { triggerResetStateHash });
                            }
                        }
                    }
                }
                base.TriggerDamageRection(damage);
            }
        }
        #endregion

        #region Set States
        public override void ResetAttackTriggers()
        {
            if (GetComponent<PhotonView>().IsMine == true)
            {
                GetComponent<PhotonView>().RPC("vControlAIShooter_ResetTriggers", RpcTarget.Others, new string[] { "StrongAttack", "WeakAttack" });
                base.ResetAttackTriggers();
            }
        }
        protected override void UpdateAI()
        {
            if (GetComponent<PhotonView>().IsMine == true)
            {
                base.UpdateAI();
            }
        }
        public override void SetCurrentTarget(Transform target, bool overrideCanseTarget)
        {
            if (GetComponent<PhotonView>().IsMine == true && target.GetComponent<PhotonView>())
            {
                GetComponent<PhotonView>().RPC(
                    "vControlAIShooter_SetCurrentTarget", 
                    RpcTarget.Others, 
                    target.GetComponent<PhotonView>().ViewID, 
                    overrideCanseTarget
                );
            }
            base.SetCurrentTarget(target, overrideCanseTarget);
        }
        #endregion

        #region RPCs

        #region Triggers
        [PunRPC]
        void vControlAIShooter_Triggers(int[] triggerHashs)
        {
            if (!animator)
            {
                animator = GetComponent<Animator>();
            }
            foreach (int triggerHash in triggerHashs)
            {
                animator.SetTrigger(triggerHash);
            }
        }
        [PunRPC]
        void vControlAIShooter_Triggers(string[] triggerNames)
        {
            if (!animator)
            {
                animator = GetComponent<Animator>();
            }
            foreach (string triggerName in triggerNames)
            {
                animator.SetTrigger(triggerName);
            }
        }
        [PunRPC]
        void vControlAIShooter_ResetTriggers(string[] triggerNames)
        {
            if (!animator)
            {
                animator = GetComponent<Animator>();
            }
            foreach (string triggerName in triggerNames)
            {
                animator.ResetTrigger(triggerName);
            }
        }
        #endregion

        #region Actions
        [PunRPC]
        void vControlAIShooter_RollTo(Vector3 direction)
        {
            RollTo(direction);
        }
        [PunRPC]
        void vControlAIShooter_Attack(bool strongAttack, int attackID, bool forceCanAttack)
        {
            Attack(strongAttack, attackID, forceCanAttack);
        }
        #endregion

        #region Sets
        [PunRPC]
        void vControlAIShooter_SetCurrentTarget(int viewId, bool overrideCanseTarget)
        {
            PhotonView view = PhotonView.Find(viewId);
            if (view)
            {
                SetCurrentTarget(view.transform, overrideCanseTarget);
            }
        }
        #endregion
        #endregion
    }
}
*/
