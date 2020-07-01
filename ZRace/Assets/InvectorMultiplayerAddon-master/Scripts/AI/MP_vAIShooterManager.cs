/*
using CBGames.Core;
using CBGames.Objects;
using Photon.Pun;
using UnityEngine;

namespace Invector.vCharacterController.AI
{
    public class MP_vAIShooterManager : vAIShooterManager
    {
        [HideInInspector] public Vector3 lastAimPos; //Variable is referenced by weapons to know where to fire its projectile
        protected Animator _animator;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        #region Set Weapon
        public new void SetLeftWeapon(GameObject weapon)
        {
            if (GetComponent<PhotonView>().IsMine == true)
            {
                int[] childTree = StaticMethods.BuildChildTree(transform, weapon.transform);
                GetComponent<PhotonView>().RPC("vAIShooterManager_SetLeftWeapon", RpcTarget.Others, (object)childTree);
            }
            base.SetLeftWeapon(weapon);
        }
        public new void SetRightWeapon(GameObject weapon)
        {
            if (GetComponent<PhotonView>().IsMine == true)
            {
                int[] childTree = StaticMethods.BuildChildTree(transform, weapon.transform);
                GetComponent<PhotonView>().RPC("vAIShooterManager_SetRightWeapon", RpcTarget.Others, (object)childTree);
            }
            base.SetRightWeapon(weapon);
        }
        #endregion

        #region IK Placements
        public override void LoadIKAdjust(string category)
        {
            if (GetComponent<PhotonView>().IsMine == true)
            {
                GetComponent<PhotonView>().RPC("vAIShooterManager_LoadIKAdjust", RpcTarget.Others, category);
            }
            base.LoadIKAdjust(category);
        }
        #endregion

        #region Actions
        public override void Shoot(Vector3 aimPosition, bool useSecundaryWeapon = false)
        {
            if (GetComponent<PhotonView>().IsMine == true)
            {
                lastAimPos = aimPosition;
            }
            base.Shoot(aimPosition, useSecundaryWeapon);
        }
        public new void ReloadWeapon()
        {
            var weapon = currentWeapon;

            if (!weapon || !weapon.gameObject.activeSelf) return;

            bool primaryWeaponAnim = false;
            if (!((weapon.ammoCount >= weapon.clipSize)) && !weapon.dontUseReload)
            {
                onReloadWeapon.Invoke(weapon);
                var needAmmo = weapon.clipSize - weapon.ammoCount;

                weapon.AddAmmo(needAmmo);

                if (_animator)
                {
                    _animator.SetInteger("ReloadID", GetReloadID());
                    _animator.SetTrigger("Reload");
                    GetComponent<PhotonView>().RPC("AI_SetTrigger", RpcTarget.Others, "Reload");
                }
                weapon.ReloadEffect();
                primaryWeaponAnim = true;
            }
            if (weapon.secundaryWeapon && !((weapon.secundaryWeapon.ammoCount >= weapon.secundaryWeapon.clipSize)) && !weapon.secundaryWeapon.dontUseReload)
            {
                var needAmmo = weapon.secundaryWeapon.clipSize - weapon.secundaryWeapon.ammoCount;

                weapon.secundaryWeapon.AddAmmo(needAmmo);

                if (!primaryWeaponAnim)
                {
                    if (_animator)
                    {
                        primaryWeaponAnim = true;
                        _animator.SetInteger("ReloadID", weapon.secundaryWeapon.reloadID);
                        _animator.SetTrigger("Reload");
                        GetComponent<PhotonView>().RPC("AI_SetTrigger", RpcTarget.Others, "Reload");
                    }
                    weapon.secundaryWeapon.ReloadEffect();
                }
            }
        }
        #endregion

        #region RPCs

        #region Set Weapon
        [PunRPC]
        void vAIShooterManager_SetLeftWeapon(int[] treeToWeapon)
        {
            Transform weapon = StaticMethods.FindTargetChild(treeToWeapon, transform);
            if (weapon)
            {
                SetLeftWeapon(weapon.gameObject);
            }
        }
        [PunRPC]
        void vAIShooterManager_SetRightWeapon(int[] treeToWeapon)
        {
            Transform weapon = StaticMethods.FindTargetChild(treeToWeapon, transform);
            if (weapon)
            {
                SetRightWeapon(weapon.gameObject);
            }
        }
        #endregion

        #region IK
        [PunRPC]
        void vAIShooterManager_LoadIKAdjust(string catagory)
        {
            LoadIKAdjust(catagory);
        }
        #endregion

        #region Weapon Actions
        [PunRPC]
        void vAIShooterManager_Shoot(int[] treeToWeapon, Vector3 aimPos)
        {
            Transform weaponTransform = StaticMethods.FindTargetChild(treeToWeapon, transform);
            weaponTransform.gameObject.GetComponent<MP_ShooterWeapon>().RecieveNetworkShot(aimPos);
        }
        [PunRPC]
        void vAIShooterManager_ShooterWeaponEmptyClip(int[] treeToWeapon)
        {
            Transform weaponTransform = StaticMethods.FindTargetChild(treeToWeapon, transform);
            weaponTransform.gameObject.GetComponent<MP_ShooterWeapon>().RecieveNetworkEmptyClip();
        }
        [PunRPC]
        void vAIShooterManager_ShooterWeaponOnFinishReload(int[] treeToWeapon)
        {
            Transform weaponTransform = StaticMethods.FindTargetChild(treeToWeapon, transform);
            weaponTransform.gameObject.GetComponent<MP_ShooterWeapon>().RecieveNetworkOnFinishReload();
        }
        [PunRPC]
        void vAIShooterManager_ShooterWeaponOnFullPower(int[] treeToWeapon)
        {
            Transform weaponTransform = StaticMethods.FindTargetChild(treeToWeapon, transform);
            weaponTransform.gameObject.GetComponent<MP_ShooterWeapon>().RecieveNetworkOnFullPower();
        }
        [PunRPC]
        void vAIShooterManager_ShooterWeaponOnFinishAmmo(int[] treeToWeapon)
        {
            Transform weaponTransform = StaticMethods.FindTargetChild(treeToWeapon, transform);
            weaponTransform.gameObject.GetComponent<MP_ShooterWeapon>().RecieveNetworkOnFinishAmmo();
        }
        [PunRPC]
        void vAIShooterManager_ShooterWeaponReload(int[] treeToWeapon)
        {
            Transform weaponTransform = StaticMethods.FindTargetChild(treeToWeapon, transform);
            weaponTransform.gameObject.GetComponent<MP_ShooterWeapon>().RecieveNetworkReload();
        }
        [PunRPC]
        void vAIShooterManager_ShooterWeaponOnEnableAim(int[] treeToWeapon)
        {
            Transform weaponTransform = StaticMethods.FindTargetChild(treeToWeapon, transform);
            weaponTransform.gameObject.GetComponent<MP_ShooterWeapon>().RecieveNetworkOnEnableAim();
        }
        [PunRPC]
        void vAIShooterManager_ShooterWeaponOnDisableAim(int[] treeToWeapon)
        {
            Transform weaponTransform = StaticMethods.FindTargetChild(treeToWeapon, transform);
            weaponTransform.gameObject.GetComponent<MP_ShooterWeapon>().RecieveNetworkOnDisableAim();
        }
        [PunRPC]
        void vAIShooterManager_ShooterWeaponOnChangerPowerCharger(int[] treeToWeapon, float amount)
        {
            Transform weaponTransform = StaticMethods.FindTargetChild(treeToWeapon, transform);
            weaponTransform.gameObject.GetComponent<MP_ShooterWeapon>().RecieveNetworkOnChangerPowerCharger(amount);
        }
        #endregion
        #endregion
    }
}
*/
