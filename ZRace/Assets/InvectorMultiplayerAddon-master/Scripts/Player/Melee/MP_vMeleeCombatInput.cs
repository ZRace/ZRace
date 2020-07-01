using CBGames.Core;
using Invector;
using Invector.vCharacterController;
using Invector.vEventSystems;
using Invector.vMelee;
using Photon.Pun;
using UnityEngine;

namespace CBGames.Player
{
    public class MP_vMeleeCombatInput : vMeleeCombatInput
    {
        #region Initializations
        private void Awake()
        {
            if (GetComponent<PhotonView>().IsMine == false && PhotonNetwork.IsConnected == true)
            {
                enabled = false;
            }
            cc = (cc == null) ? GetComponent<vThirdPersonController>() : cc;
        }
        protected override void Start()
        {
            base.Start();
            meleeManager = GetComponent<MP_vMeleeManager>();
        }

        public override void OnAnimatorMove()
        {
            if (cc == null)
            {
                cc = GetComponent<vThirdPersonController>();
            }
            if (cc != null)
            {
                base.OnAnimatorMove();
            }
        }
        public override bool lockInventory
        {
            get
            {
                if (cc == null)
                {
                    vThirdPersonController yourPlayer = NetworkManager.networkManager.GetYourPlayer();
                    if (yourPlayer)
                    {
                        cc = yourPlayer;
                    }
                    if (cc == null) return true;
                }
                return isAttacking || cc.isDead;
            }
        }
        #endregion

        #region Attacks
        public override void OnEnableAttack()
        {
            if (GetComponent<PhotonView>().IsMine == false) return;
            base.OnEnableAttack();
        }
        public override void OnDisableAttack()
        {
            if (GetComponent<PhotonView>().IsMine == false) return;
            base.OnDisableAttack();
        }
        public override void ResetAttackTriggers()
        {
            if (GetComponent<PhotonView>().IsMine == false) return;
            string[] triggers = new string[2] { "WeakAttack", "StrongAttack" };
            GetComponent<PhotonView>().RPC("ResetTriggers", RpcTarget.Others, (object)triggers);

            base.ResetAttackTriggers();
        }
        public override void BreakAttack(int breakAtkID)
        {
            if (GetComponent<PhotonView>().IsMine == false) return;
            base.BreakAttack(breakAtkID);
        }
        public override void OnRecoil(int recoilID)
        {
            if (GetComponent<PhotonView>().IsMine == false) return;
            string[] triggers = new string[2] { "TriggerRecoil", "StrongAttack" };
            GetComponent<PhotonView>().RPC("SetTriggers", RpcTarget.Others, (object)triggers);

            string[] resettriggers = new string[2] { "WeakAttack", "StrongAttack" };
            GetComponent<PhotonView>().RPC("ResetTriggers", RpcTarget.Others, (object)resettriggers);

            base.OnRecoil(recoilID);
        }
        public override void OnReceiveAttack(vDamage damage, vIMeleeFighter attacker)
        {
            if (GetComponent<PhotonView>().IsMine == false) return;
            base.OnReceiveAttack(damage, attacker);
        }
        public override void TriggerWeakAttack()
        {
            if (GetComponent<PhotonView>().IsMine == false) return;
            string[] triggers = new string[1] { "WeakAttack" };
            GetComponent<PhotonView>().RPC("SetTriggers", RpcTarget.Others, (object)triggers);

            base.TriggerWeakAttack();
        }
        public override void TriggerStrongAttack()
        {
            if (GetComponent<PhotonView>().IsMine == false) return;
            string[] triggers = new string[1] { "StrongAttack" };
            GetComponent<PhotonView>().RPC("SetTriggers", RpcTarget.Others, (object)triggers);
            
            base.TriggerStrongAttack();
        }
        #endregion
    }
}
