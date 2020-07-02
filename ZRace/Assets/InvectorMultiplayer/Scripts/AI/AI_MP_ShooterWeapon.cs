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

        /// <summary>
        /// Find the parent MP_vAIShooterManager component. Also makes sure
        /// this weapon has a valid vShooterWeapon component. Finally, makes 
        /// sure the root transform has a photon view.
        /// </summary>
        protected override void Start()
        {
            ai_shooterManager = transform.GetComponentInParent<MP_vAIShooterManager>();
            ChildTreeCheck();
            WeaponCheck();
            ViewCheck();
        }

        /// <summary>
        /// Make sure this has a index tree built for this current object. 
        /// If not then it builds one.
        /// </summary>
        protected override void ChildTreeCheck()
        {
            if (childTree == null || childTree.Length == 0)
            {
                childTree = StaticMethods.BuildChildTree(ai_shooterManager.transform, transform, false);
            }
        }

        #region Sends
        /// <summary>
        /// Send the 'Shoot' trigger over the network and tells the other network
        /// versions of this object to play their weapon fire function.
        /// </summary>
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

        /// <summary>
        /// Sends a command to the other networked versions of this weapon to trigger
        /// their reload function
        /// </summary>
        public override void SendNetworkReload()
        {
            if (view.IsMine == false) return;
            view.RPC("vAIShooterManager_ShooterWeaponReload", RpcTarget.Others, (object)childTree);
        }

        /// <summary>
        /// Sends a command to the other networked versions of this weapon to trigger
        /// their empty clip function
        /// </summary>
        public override void SendNetworkEmptyClip()
        {
            if (view.IsMine == false) return;
            view.RPC("vAIShooterManager_ShooterWeaponEmptyClip", RpcTarget.Others, (object)childTree);
        }

        /// <summary>
        /// Sends a command to the other networked versions of this weapon to trigger
        /// their on finish reload function
        /// </summary>
        public override void SendNetworkOnFinishReload()
        {
            if (view.IsMine == false) return;
            view.RPC("vAIShooterManager_ShooterWeaponOnFinishReload", RpcTarget.Others, (object)childTree);
        }

        /// <summary>
        /// Sends a command to the other networked versions of this weapon to trigger
        /// their on full power function
        /// </summary>
        public override void SendNetworkOnFullPower()
        {
            if (view.IsMine == false) return;
            view.RPC("vAIShooterManager_ShooterWeaponOnFullPower", RpcTarget.Others, (object)childTree);
        }

        /// <summary>
        /// Sends a command to the other networked versions of this weapon to trigger
        /// their on finish ammo function
        /// </summary>
        public override void SendNetworkOnFinishAmmo()
        {
            if (view.IsMine == false) return;
            if (weapon.onFinishAmmo.GetPersistentEventCount() > 0)
            {
                view.RPC("vAIShooterManager_ShooterWeaponOnFinishAmmo", RpcTarget.Others, (object)childTree);
            }
        }

        /// <summary>
        /// Sends a command to the other networked versions of this weapon to trigger
        /// their on enable aim function
        /// </summary>
        public override void SendOnEnableAim()
        {
            if (view.IsMine == false) return;
            view.RPC("vAIShooterManager_ShooterWeaponOnEnableAim", RpcTarget.Others, (object)childTree);
        }

        /// <summary>
        /// Sends a command to the other networked versions of this weapon to trigger
        /// their on disable aim function
        /// </summary>
        public override void SendOnDisableAim()
        {
            if (view.IsMine == false) return;
            view.RPC("vAIShooterManager_ShooterWeaponOnDisableAim", RpcTarget.Others, (object)childTree);
        }

        /// <summary>
        /// Sends a command to the other networked versions of this weapon to trigger
        /// their on chane power charger function
        /// </summary>
        public override void SendOnChangerPowerCharger(float amount)
        {
            if (view.IsMine == false) return;
            view.RPC("vAIShooterManager_ShooterWeaponOnChangerPowerCharger", RpcTarget.Others, (object)childTree, amount);
        }
        #endregion
    }
}
*/