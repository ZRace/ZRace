/*
using CBGames.Core;
using Invector.vShooter;
using Photon.Pun;
using System.Collections;
using UnityEngine;

public class MP_vShooterManager : vShooterManager, IPunObservable
{
    private bool mp_cancelReload = false;
    private bool mp_isReloading = false;
    private bool mp_isReloadingWeapon = false;
    private bool _isLeftWeapon, _isShooting;
    private bool _initialized = false;
    [HideInInspector] public Vector3 lastAimPos;

    public override void Start()
    {
        base.Start();
        StartCoroutine(WaitToInitialize());
    }
    IEnumerator WaitToInitialize()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        _initialized = true;
    }

    #region Network Sync Animations
    IEnumerator NetworkCancelReloadRoutine()
    {
        if (CurrentWeapon != null)
        {
            GetComponent<PhotonView>().RPC("ResetTriggers", RpcTarget.Others, new string[1] { "Reload" });
            mp_cancelReload = true;
            yield return new WaitForSeconds(CurrentWeapon.reloadTime + 0.1f);
            mp_cancelReload = false;
            if (mp_isReloadingWeapon)
            {
                mp_isReloadingWeapon = false;
            }
        }
    }
    protected override IEnumerator CancelReloadRoutine()
    {
        StartCoroutine(NetworkCancelReloadRoutine());
        StartCoroutine(base.CancelReloadRoutine());
        yield return null;
    }

    IEnumerator NetworkReloadWeapon()
    {
        var weapon = rWeapon ? rWeapon : lWeapon;
        if (!weapon || !weapon.gameObject.activeInHierarchy || mp_isReloading) yield return null;
        bool mp_primaryWeaponAnim = false;
        if (weapon.ammoCount < weapon.clipSize && (weapon.isInfinityAmmo || WeaponHasUnloadedAmmo()) && !weapon.dontUseReload)
        {
            if (GetComponent<Animator>())
            {
                GetComponent<PhotonView>().RPC("SetTriggers", RpcTarget.Others, new int[1] { Reload });
            }
            mp_primaryWeaponAnim = true;
        }
        if (weapon.secundaryWeapon && weapon.secundaryWeapon.ammoCount >= weapon.secundaryWeapon.clipSize && (weapon.secundaryWeapon.isInfinityAmmo || WeaponHasUnloadedAmmo(true)) && !weapon.secundaryWeapon.dontUseReload)
        {
            if (!mp_primaryWeaponAnim)
            {
                if (GetComponent<Animator>())
                {
                    mp_primaryWeaponAnim = true;
                    GetComponent<PhotonView>().RPC("SetTriggers", RpcTarget.Others, new int[1] { Reload });
                }
            }
        }
        yield return null;
    }
    public override void ReloadWeapon()
    {
        StartCoroutine(NetworkReloadWeapon());
        base.ReloadWeapon();
    }

    protected override IEnumerator AddAmmoToWeapon(vShooterWeapon weapon, float delayTime, bool ignoreEffects = false)
    {
        StartCoroutine(NetworkAddAmmoToWeapon(weapon, delayTime, ignoreEffects));
        StartCoroutine(base.AddAmmoToWeapon(weapon, delayTime, ignoreEffects));
        yield return null;
    }
    IEnumerator NetworkAddAmmoToWeapon(vShooterWeapon weapon, float delayTime, bool ignoreEffects = false)
    {
        mp_isReloading = true;
        if (weapon.ammoCount < weapon.clipSize && (weapon.isInfinityAmmo || WeaponHasUnloadedAmmo()) && !weapon.dontUseReload && !mp_cancelReload)
        {
            yield return new WaitForSeconds(delayTime);
            if (!mp_cancelReload)
            {
                if (weapon.reloadOneByOne && weapon.ammoCount < weapon.clipSize && WeaponHasUnloadedAmmo())
                {
                    if (WeaponAmmo(weapon).count == 0)
                    {
                        if (!ignoreEffects)
                        {
                            mp_isReloadingWeapon = false;
                        }
                    }
                    else
                    {
                        if (!ignoreEffects) mp_isReloadingWeapon = true;
                        if (!mp_cancelReload)
                        {
                            if (!ignoreEffects)
                            {
                                GetComponent<PhotonView>().RPC("SetTriggers", RpcTarget.Others, new int[1] { Reload });
                            }
                        }
                    }
                }
                else
                {
                    if (!ignoreEffects)
                    {
                        mp_isReloadingWeapon = false;
                    }
                }
            }
        }
        mp_isReloading = false;
    }

    protected override IEnumerator Recoil(float horizontal, float up)
    {
        StartCoroutine(NetworkRecoil(horizontal, up));
        StartCoroutine(base.Recoil(horizontal, up));
        yield return null;
    }
    IEnumerator NetworkRecoil(float horizontal, float up)
    {
        if (GetComponent<Animator>())
        {
            yield return new WaitForSeconds(0.02f);
            GetComponent<PhotonView>().RPC("SetTriggers", RpcTarget.Others, new int[1] { IsShoot });
        }
        yield return null;
    }
    #endregion

    #region Shot Position
    public override void Shoot(Vector3 aimPosition, bool applyHipfirePrecision = false, bool useSecundaryWeapon = false)
    {
        lastAimPos = applyHipfirePrecision ? aimPosition + HipFirePrecision(aimPosition) : aimPosition;
        base.Shoot(lastAimPos, false, useSecundaryWeapon);
    }
    #endregion

    #region IK
    public override void LoadIKAdjust(string category)
    {
        if (GetComponent<PhotonView>().IsMine == true)
        {
            GetComponent<PhotonView>().RPC("LoadIKAdjustCatagory", RpcTarget.Others, category);
        }
        base.LoadIKAdjust(category);
    }
    public override void SetLeftWeapon(GameObject weapon)
    {
        base.SetLeftWeapon(weapon);
        if (GetComponent<PhotonView>().IsMine == true)
        {
            GetComponent<PhotonView>().RPC(
                "vShooterManager_ReceiveSetLeftWeapon", 
                RpcTarget.Others,
                (weapon) ? StaticMethods.BuildChildTree(transform, weapon.transform, false) : new int[] { }
            );
        }
    }
    public override void SetRightWeapon(GameObject weapon)
    {
        base.SetRightWeapon(weapon);
        if (GetComponent<PhotonView>().IsMine == true)
        {
            GetComponent<PhotonView>().RPC(
                "vShooterManager_ReceiveSetRightWeapon",
                RpcTarget.Others,
                (weapon) ? StaticMethods.BuildChildTree(transform, weapon.transform, false) : new int[] {}
            );
        }
    }
    public override bool IsLeftWeapon
    {
        get
        {
            return (GetComponent<PhotonView>().IsMine == true) ? base.IsLeftWeapon : _isLeftWeapon;
        }
    }
    public override bool isShooting
    {
        get
        {
            return (GetComponent<PhotonView>().IsMine == true) ? base.isShooting : _isShooting;
        }
    }
    #endregion

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) //this function called by Photon View component
    {
        if (_initialized == false) return;
        if (stream.IsWriting)
        {
            stream.SendNext(IsLeftWeapon);
            stream.SendNext(isShooting);
            stream.SendNext(smoothArmIKRotation);
        }
        else if (stream.IsReading)
        {
            try
            {
                _isLeftWeapon = (bool)stream.ReceiveNext();
                _isShooting = (bool)stream.ReceiveNext();
                smoothArmIKRotation = (float)stream.ReceiveNext();
            }
            catch { }
        }
    }

    #region RPCs
    [PunRPC]
    void LoadIKAdjustCatagory(string catagory)
    {
        LoadIKAdjust(catagory);
    }
    [PunRPC]
    void vShooterManager_ReceiveSetLeftWeapon(int[] treeToWeapon)
    {
        if (treeToWeapon.Length > 0)
        {
            Transform weapon = StaticMethods.FindTargetChild(treeToWeapon, transform);
            SetLeftWeapon(weapon.gameObject);
        }
        else
        {
            SetLeftWeapon(null);
        }
    }
    [PunRPC]
    void vShooterManager_ReceiveSetRightWeapon(int[] treeToWeapon)
    {
        if (treeToWeapon.Length > 0)
        {
            Transform weapon = StaticMethods.FindTargetChild(treeToWeapon, transform);
            SetRightWeapon(weapon.gameObject);
        }
        else
        {
            SetRightWeapon(null);
        }
    }
    #endregion
}
*/
