/*
using UnityEngine;
using Photon.Pun;
using Invector.vShooter;
using CBGames.Core;
using CBGames.Player;

namespace CBGames.Objects
{
    [RequireComponent(typeof(vShooterWeapon))]
    public class MP_ShooterWeapon : MonoBehaviour
    {
        #region Parameters
        protected PhotonView view;
        protected vShooterWeapon weapon;
        protected MP_vShooterManager shooterManager;
        protected int[] childTree = null;
        #endregion

        #region Initializations
        protected virtual void Awake()
        {
            ViewCheck();
        }
        protected virtual void Start()
        {
            shooterManager = transform.GetComponentInParent<MP_vShooterManager>();
            WeaponCheck();
            ChildTreeCheck();
        }
        protected virtual void ViewCheck()
        {
            if (!view)
            {
                if (GetComponent<PhotonView>())
                {
                    view = GetComponent<PhotonView>();
                }
                else if (transform.GetComponentInParent<PhotonView>())
                {
                    view = transform.GetComponentInParent<PhotonView>();
                }
            }
        }
        protected virtual void ChildTreeCheck()
        {
            if (childTree == null || childTree.Length == 0)
            {
                childTree = StaticMethods.BuildChildTree(transform.root, transform, false);
            }
        }
        protected virtual void WeaponCheck()
        {
            if (!weapon)
            {
                weapon = GetComponent<vShooterWeapon>();
            }
        }
        #endregion

        #region Bow
        public virtual void SetArrowView(vProjectileControl control)
        {
            control.GetComponentInChildren<ArrowView>().viewId = transform.GetComponentInParent<SyncPlayer>().GetArrowId();
            control.GetComponentInChildren<ArrowView>().owner = transform.GetComponentInParent<SyncPlayer>().transform;
        }
        #endregion

        #region Send Network Events
        public virtual void SendNetworkShot()
        {
            ChildTreeCheck();
            if (transform.GetComponentInParent<PhotonView>().IsMine == false) return;
            if (weapon.ammo > 0)
            {
                if (weapon.transform.GetComponent<vBowControl>())
                {
                    view.RPC("SetTriggers", RpcTarget.Others, new string[1] { "Shot" });
                }

                view.RPC("ShooterWeaponShotWithPosition", RpcTarget.Others, childTree, shooterManager.lastAimPos);
            }
        }
        public virtual void SendNetworkReload()
        {
            ViewCheck();
            ChildTreeCheck();
            if (view.IsMine == false) return;
            view.RPC("ShooterWeaponReload", RpcTarget.Others, childTree);
        }
        public virtual void SendNetworkEmptyClip()
        {
            ViewCheck();
            ChildTreeCheck();
            if (view.IsMine == false) return;
            view.RPC("ShooterWeaponEmptyClip", RpcTarget.Others, childTree);
        }
        public virtual void SendNetworkOnFinishReload()
        {
            ViewCheck();
            ChildTreeCheck();
            if (view.IsMine == false) return;
            view.RPC("ShooterWeaponOnFinishReload", RpcTarget.Others, childTree);
        }
        public virtual void SendNetworkOnFullPower()
        {
            ViewCheck();
            ChildTreeCheck();
            if (view.IsMine == false) return;
            view.RPC("ShooterWeaponOnFullPower", RpcTarget.Others, childTree);
        }
        public virtual void SendNetworkOnFinishAmmo()
        {
            ViewCheck();
            ChildTreeCheck();
            if (view.IsMine == false) return;
            if (weapon.onFinishAmmo.GetPersistentEventCount() > 0)
            {
                view.RPC("ShooterWeaponOnFinishAmmo", RpcTarget.Others, childTree);
            }
        }
        public virtual void SendOnEnableAim()
        {
            ViewCheck();
            ChildTreeCheck();
            if (view.IsMine == false) return;
            view.RPC("ShooterWeaponOnEnableAim", RpcTarget.Others, childTree);
        }
        public virtual void SendOnDisableAim()
        {
            ViewCheck();
            ChildTreeCheck();
            if (view.IsMine == false) return;
            view.RPC("ShooterWeaponOnDisableAim", RpcTarget.Others, childTree);
        }
        public virtual void SendOnChangerPowerCharger(float amount)
        {
            ViewCheck();
            ChildTreeCheck();
            if (view.IsMine == false) return;
            view.RPC("ShooterWeaponOnChangerPowerCharger", RpcTarget.Others, childTree, amount);
        }
        #endregion

        #region Recieve Network Events
        public virtual void RecieveNetworkShot(Vector3 aimPos)
        {
            ViewCheck();
            if (view.IsMine == true) return;
            WeaponCheck();
            weapon.isInfinityAmmo = true;
            weapon.AddAmmo(1);
            weapon.Shoot(aimPos, transform);
        }
        public virtual void RecieveNetworkShot()
        {
            ViewCheck();
            if (view.IsMine == true) return;
            WeaponCheck();
            weapon.isInfinityAmmo = true;
            weapon.AddAmmo(1);
            weapon.Shoot(transform);
        }
        public virtual void RecieveNetworkReload()
        {
            ViewCheck();
            if (view.IsMine == true) return;
            WeaponCheck();
            weapon.ReloadEffect();
        }
        public virtual void RecieveNetworkEmptyClip()
        {
            ViewCheck();
            if (view.IsMine == true) return;
            WeaponCheck();
            if (weapon.source)
            {
                weapon.source.Stop();
                weapon.source.PlayOneShot(weapon.emptyClip);
            }
        }
        public virtual void RecieveNetworkOnFinishReload()
        {
            ViewCheck();
            if (view.IsMine == true) return;
            WeaponCheck();
            weapon.FinishReloadEffect();
            weapon.onFinishReload.Invoke();
        }
        public virtual void RecieveNetworkOnFullPower()
        {
            ViewCheck();
            if (view.IsMine == true) return;
            WeaponCheck();
            weapon.onFullPower.Invoke();
        }
        public virtual void RecieveNetworkOnFinishAmmo()
        {
            ViewCheck();
            if (view.IsMine == true) return;
            WeaponCheck();
            weapon.onFinishAmmo.Invoke();
        }
        public virtual void RecieveNetworkOnEnableAim()
        {
            ViewCheck();
            if (view.IsMine == true) return;
            WeaponCheck();
            weapon.onEnableAim.Invoke();
        }
        public virtual void RecieveNetworkOnDisableAim()
        {
            ViewCheck();
            if (view.IsMine == true) return;
            WeaponCheck();
            weapon.onDisableAim.Invoke();
        }
        public virtual void RecieveNetworkOnChangerPowerCharger(float amount)
        {
            ViewCheck();
            if (view.IsMine == true) return;
            WeaponCheck();
            weapon.onChangerPowerCharger.Invoke(amount);
        }
        #endregion

    }
}
*/
