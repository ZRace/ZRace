/*
using Invector;
using Invector.IK;
using Invector.vCharacterController;
using Invector.vEventSystems;
using Invector.vShooter;
using Photon.Pun;
using System.Collections;
using System.IO;
using UnityEngine;

[AddComponentMenu("CB GAMES/Player/MP Components/MP vShooterMeleeInput")]
public class MP_vShooterMeleeInput : vShooterMeleeInput, IPunObservable
{

    #region IK
    bool initialized = false;
    Vector3 _targetArmAligmentDirection = Vector3.zero;
    Vector3 _targetArmAlignmentPosition = Vector3.zero;
    #endregion

    #region Initializations
    protected override void Start()
    {
        if (GetComponent<PhotonView>().IsMine == true)
        {
            base.Start();
        }
        else
        {
            shooterManager = GetComponent<vShooterManager>();
            leftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);
            rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);
            leftLowerArm = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
            rightLowerArm = animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
            leftUpperArm = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
            rightUpperArm = animator.GetBoneTransform(HumanBodyBones.RightUpperArm);

            onlyArmsLayer = animator.GetLayerIndex("OnlyArms");
            aimAngleReference = new GameObject("aimAngleReference");
            aimAngleReference.tag = ("Ignore Ragdoll");
            aimAngleReference.transform.rotation = transform.rotation;
            var chest = animator.GetBoneTransform(HumanBodyBones.Head);
            aimAngleReference.transform.SetParent(chest);
            aimAngleReference.transform.localPosition = Vector3.zero;
            defaultStrafeWalk = cc.strafeSpeed.walkByDefault;
            headTrack = GetComponent<vHeadTrack>();
            lastRotateWithCamera = cc.strafeSpeed.rotateWithCamera;
            if (!controlAimCanvas)
                Debug.LogWarning("Missing the AimCanvas, drag and drop the prefab to this scene in order to Aim", gameObject);
        }
        cc = (cc == null) ? GetComponent<vThirdPersonController>() : cc;
        StartCoroutine(WaitToInitialize());
    }
    IEnumerator WaitToInitialize()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        initialized = true;
    }
    #endregion

    #region Attacks
    /// <summary>
    /// Overrides default functionality of invector to only work if you're the owner player
    /// and will not work if this is called by a networked player.
    /// </summary>
    public override void OnEnableAttack()
    {
        if (GetComponent<PhotonView>().IsMine == false) return;
        base.OnEnableAttack();
    }

    /// <summary>
    /// Overrides default functionality of invector to only work if you're the owner player
    /// and will not work if this is called by a networked player.
    /// </summary>
    public override void OnDisableAttack()
    {
        if (GetComponent<PhotonView>().IsMine == false) return;
        base.OnDisableAttack();
    }

    /// <summary>
    /// Overrides default functionality of invector to only work if you're the owner player
    /// and will not work if this is called by a networked player. Als will reset the 
    /// triggers for the networked players when called via the `ResetTriggers` RPC.
    /// </summary>
    public override void ResetAttackTriggers()
    {
        if (GetComponent<PhotonView>().IsMine == false) return;
        string[] triggers = new string[2] { "WeakAttack", "StrongAttack" };
        GetComponent<PhotonView>().RPC("ResetTriggers", RpcTarget.Others, (object)triggers);

        base.ResetAttackTriggers();
    }

    /// <summary>
    /// Overrides default functionality of invector to only work if you're the owner player
    /// and will not work if this is called by a networked player.
    /// </summary>
    public override void BreakAttack(int breakAtkID)
    {
        if (GetComponent<PhotonView>().IsMine == false) return;
        base.BreakAttack(breakAtkID);
    }

    /// <summary>
    /// Overrides default functionality of invector to only work if you're the owner player
    /// and will not work if this is called by a networked player. Will also set the triggers
    /// for recoil and strong attack and reset the triggers for weak attack and strong attack
    /// for the networked players.
    /// </summary>
    public override void OnRecoil(int recoilID)
    {
        if (GetComponent<PhotonView>().IsMine == false) return;
        string[] triggers = new string[2] { "TriggerRecoil", "StrongAttack" };
        GetComponent<PhotonView>().RPC("SetTriggers", RpcTarget.Others, (object)triggers);

        string[] resettriggers = new string[2] { "WeakAttack", "StrongAttack" };
        GetComponent<PhotonView>().RPC("ResetTriggers", RpcTarget.Others, (object)resettriggers);

        base.OnRecoil(recoilID);
    }

    /// <summary>
    /// Overrides default functionality of invector to only work if you're the owner player
    /// and will not work if this is called by a networked player. Triggers the weak attack
    /// animation for networked players to mimic the owner player via the `SetTriggers` RPC.
    /// </summary>
    public override void TriggerWeakAttack()
    {
        if (GetComponent<PhotonView>().IsMine == false) return;
        string[] triggers = new string[1] { "WeakAttack" };
        GetComponent<PhotonView>().RPC("SetTriggers", RpcTarget.Others, (object)triggers);

        base.TriggerWeakAttack();
    }

    /// <summary>
    /// Overrides default functionality of invector to only work if you're the owner player
    /// and will not work if this is called by a networked player. Triggers the strong attack
    /// animation for networked players to mimic the owner player via the `SetTriggers` RPC.
    /// </summary>
    public override void TriggerStrongAttack()
    {
        if (GetComponent<PhotonView>().IsMine == false) return;
        string[] triggers = new string[1] { "StrongAttack" };
        GetComponent<PhotonView>().RPC("SetTriggers", RpcTarget.Others, (object)triggers);

        base.TriggerStrongAttack();
    }
    public override void OnReceiveAttack(vDamage damage, vIMeleeFighter attacker)
    {
        if (cc == null)
        {
            cc = GetComponent<vThirdPersonController>();
        }
        base.OnReceiveAttack(damage, attacker);
    }
    #endregion

    #region Animator Weights
    /// <summary>
    /// Overrides default functionality of invector to only work if you're the owner player
    /// and will not work if this is called by a networked player. Sets the animator layer
    /// weights for the networked players to mimic the owner player via the `SetAnimatorLayerWeights`
    /// RPC.
    /// </summary>
    public override void ResetShooterAnimations()
    {
        if (GetComponent<PhotonView>().IsMine == false) return;
        base.ResetShooterAnimations();
        GetComponent<PhotonView>().RPC("SetAnimatorLayerWeights", RpcTarget.Others, onlyArmsLayer, onlyArmsLayerWeight);
    }

    /// <summary>
    /// Overrides default functionality of invector to only work if you're the owner player
    /// and will not work if this is called by a networked player. Sets the animator layer
    /// weights for the networked players to mimic the owner player via the `SetAnimatorLayerWeights`
    /// RPC.
    /// </summary>
    protected override void UpdateShooterAnimations()
    {
        if (GetComponent<PhotonView>().IsMine == false) return;
        base.UpdateShooterAnimations();
        onlyArmsLayerWeight = Mathf.Lerp(onlyArmsLayerWeight, (CurrentActiveWeapon) ? 1f : 0f, 6f * Time.deltaTime);
        GetComponent<PhotonView>().RPC("SetAnimatorLayerWeights", RpcTarget.Others, onlyArmsLayer, onlyArmsLayerWeight);
    }

    /// <summary>
    /// Overrides default functionality of invector to only work if you're the owner player
    /// and will not work if this is called by a networked player. Otherwise the functionality is 
    /// the same but it also sets the animator layer weights for the networked players to mimic 
    /// the owner player via the `SetAnimatorLayerWeights`
    /// RPC.
    /// </summary>
    protected override void UpdateMeleeAnimations()
    {
        if (GetComponent<PhotonView>().IsMine == false) return;
        if (!animator) return;

        if (cc.customAction)
        {
            ResetMeleeAnimations();
            ResetShooterAnimations();
            UpdateCameraStates();
            CancelAiming();
            return;
        }
        if ((shooterManager == null || !CurrentActiveWeapon) && meleeManager)
        {
            base.UpdateMeleeAnimations();
            onlyArmsLayerWeight = Mathf.Lerp(onlyArmsLayerWeight, 0, 6f * Time.deltaTime);
            animator.SetLayerWeight(onlyArmsLayer, onlyArmsLayerWeight);
            GetComponent<PhotonView>().RPC("SetAnimatorLayerWeights", RpcTarget.Others, onlyArmsLayer, onlyArmsLayerWeight);
            animator.SetBool(vAnimatorParameters.IsAiming, false);
            isReloading = false;
        }
        else if (shooterManager && CurrentActiveWeapon)
            UpdateShooterAnimations();
        else
        {
            ResetMeleeAnimations();
            ResetShooterAnimations();
        }
    }
    #endregion

    #region Heartbeat
    /// <summary>
    /// Overrides default functionality of invector to only work if you're the owner player
    /// and will not work if this is called by a networked player.
    /// </summary>
    public override void OnAnimatorMove()
    {
        if (cc == null || GetComponent<PhotonView>().IsMine == false) return;
        base.OnAnimatorMove();
    }

    /// <summary>
    /// Overrides default functionality of invector to only work if you're the owner player
    /// and will not work if this is called by a networked player.
    /// </summary>
    protected override void Update()
    {
        if (GetComponent<PhotonView>().IsMine == false) return;
        base.Update();
    }

    /// <summary>
    /// Overrides default functionality of invector to only work if you're the owner player
    /// and will not work if this is called by a networked player.
    /// </summary>
    protected override void FixedUpdate()
    {
        if (GetComponent<PhotonView>().IsMine == true)
        {
            base.FixedUpdate();
        }
        else
        {
            updateIK = true;
        }
    }

    /// <summary>
    /// Overrides default functionality of invector to perform the same if you're the 
    /// owner player. Otherwise will only update its aiming location based on what
    /// is received over the network.
    /// </summary>
    protected override void LateUpdate()
    {
        if (GetComponent<PhotonView>().IsMine == true)
        {
            base.LateUpdate();
        }
        else if (initialized == true)
        {
            if ((!updateIK && animator.updateMode == AnimatorUpdateMode.AnimatePhysics)) return;
            UpdateAimBehaviour();
            updateIK = false;
        }
    }
    #endregion

    #region IK
    protected override Vector3 targetArmAlignmentPosition
    {
        get
        {
            if (GetComponent<PhotonView>().IsMine == true)
            {
                if (cameraMain == null)
                {
                    return Vector3.zero;
                }
                else
                {
                    return base.targetArmAlignmentPosition;
                }
            }
            else
            {
                return _targetArmAlignmentPosition;
            }
        }
    }
    protected override Vector3 targetArmAligmentDirection
    {
        get
        {
            if (GetComponent<PhotonView>().IsMine == true)
            {
                if (cameraMain == null)
                {
                    return Vector3.zero;
                }
                else
                {
                    return base.targetArmAligmentDirection;
                }
            }
            else
            {
                return _targetArmAligmentDirection;
            }
        }
    }
    protected override void UpdateAimBehaviour()
    {
        if (GetComponent<PhotonView>().IsMine == true)
        {
            base.UpdateAimBehaviour();
        }
        else
        {
            UpdateHeadTrack();
            if (shooterManager && CurrentActiveWeapon)
            {
                UpdateIKAdjust(shooterManager.IsLeftWeapon);
                RotateAimArm(shooterManager.IsLeftWeapon);
                RotateAimHand(shooterManager.IsLeftWeapon);
                UpdateArmsIK(shooterManager.IsLeftWeapon);
            }
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) //this function called by Photon View component
    {
        try
        {
            if (initialized == false) return;
            if (stream.IsWriting)
            {
                stream.SendNext(targetArmAligmentDirection);
                stream.SendNext(targetArmAlignmentPosition);
                stream.SendNext(_isAiming);
                stream.SendNext(isReloading);
                stream.SendNext(isEquipping);
                stream.SendNext(cc.isStrafing);
                stream.SendNext(aimPosition);
                stream.SendNext(_aimTimming);
                stream.SendNext(aimConditions);
                stream.SendNext(isUsingScopeView);
                stream.SendNext(isCameraRightSwitched);
                stream.SendNext(onlyArmsLayerWeight);
                stream.SendNext(supportIKWeight);
                stream.SendNext(weaponIKWeight);
                stream.SendNext(armAlignmentWeight);
                stream.SendNext(aimWeight);
                stream.SendNext(lastAimDistance);
                stream.SendNext(lastRotateWithCamera);
                stream.SendNext(aimAngleReference.transform.position);
                stream.SendNext(aimAngleReference.transform.rotation);
                stream.SendNext(aimAngleReference.transform.localPosition);
            }
            else if (stream.IsReading)
            {
                _targetArmAligmentDirection = (Vector3)stream.ReceiveNext();
                _targetArmAlignmentPosition = (Vector3)stream.ReceiveNext();
                _isAiming = (bool)stream.ReceiveNext();
                isReloading = (bool)stream.ReceiveNext();
                isEquipping = (bool)stream.ReceiveNext();
                cc.isStrafing = (bool)stream.ReceiveNext();
                aimPosition = (Vector3)stream.ReceiveNext();
                _aimTimming = (float)stream.ReceiveNext();
                aimConditions = (bool)stream.ReceiveNext();
                isUsingScopeView = (bool)stream.ReceiveNext();
                isCameraRightSwitched = (bool)stream.ReceiveNext();
                onlyArmsLayerWeight = (float)stream.ReceiveNext();
                supportIKWeight = (float)stream.ReceiveNext();
                weaponIKWeight = (float)stream.ReceiveNext();
                armAlignmentWeight = (float)stream.ReceiveNext();
                aimWeight = (float)stream.ReceiveNext();
                lastAimDistance = (float)stream.ReceiveNext();
                lastRotateWithCamera = (bool)stream.ReceiveNext();
                aimAngleReference.transform.position = (Vector3)stream.ReceiveNext();
                aimAngleReference.transform.rotation = (Quaternion)stream.ReceiveNext();
                aimAngleReference.transform.localPosition = (Vector3)stream.ReceiveNext();
            }
        }
        catch { }
    }
    #endregion
}
*/
