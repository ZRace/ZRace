/*
using CBGames.Core;
using CBGames.Objects;
using Invector.vCharacterController.AI;
using Invector.vShooter;
using Photon.Pun;
using UnityEngine;

namespace CBGames.AI
{
    public class AI_MP_ShooterWeapon : MP_ShooterWeapon
    {
        protected MP_vAIShooterManager ai_shooterManager = null;
       
        protected override void Start()
        {
            ai_shooterManager = transform.GetComponentInParent<MP_vAIShooterManager>();
            ChildTreeCheck();
            WeaponCheck();
            ViewCheck();
        }
        protected override void ChildTreeCheck()
        {
            if (childTree == null || childTree.Length == 0)
            {
                childTree = StaticMethods.BuildChildTree(ai_shooterManager.transform, transform, false);
            }
        }

        #region Sends
        public override void SendNetworkShot()
        {
            if (transform.GetComponentInParent<PhotonView>().IsMine == false) return;
            if (weapon.ammo > 0)
            {
                if (weapon.transform.GetComponent<vBowControl>())
                {
                    view.RPC("SetTriggers", RpcTarget.Others, new string[1] { "Shoot" });
                }

                view.RPC("vAIShooterManager_Shoot", RpcTarget.Others, (object)childTree, ai_shooterManager.lastAimPos);
            }
        }
        public override void SendNetworkReload()
        {
            if (view.IsMine == false) return;
            view.RPC("vAIShooterManager_ShooterWeaponReload", RpcTarget.Others, (object)childTree);
        }
        public override void SendNetworkEmptyClip()
        {
            if (view.IsMine == false) return;
            view.RPC("vAIShooterManager_ShooterWeaponEmptyClip", RpcTarget.Others, (object)childTree);
        }
        public override void SendNetworkOnFinishReload()
        {
            if (view.IsMine == false) return;
            view.RPC("vAIShooterManager_ShooterWeaponOnFinishReload", RpcTarget.Others, (object)childTree);
        }
        public override void SendNetworkOnFullPower()
        {
            if (view.IsMine == false) return;
            view.RPC("vAIShooterManager_ShooterWeaponOnFullPower", RpcTarget.Others, (object)childTree);
        }
        public override void SendNetworkOnFinishAmmo()
        {
            if (view.IsMine == false) return;
            if (weapon.onFinishAmmo.GetPersistentEventCount() > 0)
            {
                view.RPC("vAIShooterManager_ShooterWeaponOnFinishAmmo", RpcTarget.Others, (object)childTree);
            }
        }
        public override void SendOnEnableAim()
        {
            if (view.IsMine == false) return;
            view.RPC("vAIShooterManager_ShooterWeaponOnEnableAim", RpcTarget.Others, (object)childTree);
        }
        public override void SendOnDisableAim()
        {
            if (view.IsMine == false) return;
            view.RPC("vAIShooterManager_ShooterWeaponOnDisableAim", RpcTarget.Others, (object)childTree);
        }
        public override void SendOnChangerPowerCharger(float amount)
        {
            if (view.IsMine == false) return;
            view.RPC("vAIShooterManager_ShooterWeaponOnChangerPowerCharger", RpcTarget.Others, (object)childTree, amount);
        }
        #endregion
    }
}
*/
