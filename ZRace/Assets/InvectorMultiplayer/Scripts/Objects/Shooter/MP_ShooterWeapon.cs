/*
using UnityEngine;
using Photon.Pun;
using Invector.vShooter;

namespace CBGames.Objects
{
    [AddComponentMenu("CB GAMES/Weapons/MP Shooter Weapon")]
    [RequireComponent(typeof(vShooterWeapon))]
    public class MP_ShooterWeapon : MP_BaseShooterWeapon
    {
        #region Parameters
        protected PhotonView view;
        protected vShooterWeapon weapon;
        protected MP_vShooterManager shooterManager;
        protected int[] childTree = null;
        #endregion

        #region Initializations
        /// <summary>
        /// Assign the MP_vShooterManager component that should be in the root of this object
        /// </summary>
        protected override void Start()
        {
            shooterManager = transform.GetComponentInParent<MP_vShooterManager>();
            base.Start();
        }
        #endregion

        #region Send Network Events
        /// <summary>
        /// This is called via the OnShot UnityEvent and will cause the networked versions to 
        /// fire a shot with this weapon.
        /// </summary>
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
        #endregion
    }
}
*/
