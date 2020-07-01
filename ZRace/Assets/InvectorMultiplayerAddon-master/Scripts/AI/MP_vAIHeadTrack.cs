/*
using Invector.vCharacterController.AI;
using Photon.Pun;
using UnityEngine;

namespace Invector
{
    [RequireComponent(typeof(PhotonView))]
    public class MP_vAIHeadTrack : vAIHeadtrack, IPunObservable
    {
        #region Internal Only Variables
        protected bool _updateIK = false;
        protected Animator _animator = null;
        protected vIControlAI _character = null;
        protected Vector3 _lookAtPoint = Vector3.zero;
        #endregion

        #region Network Serialization
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_currentHeadWeight);
                stream.SendNext(_currentbodyWeight);
                stream.SendNext(_lookAtPoint);
            }
            else if (stream.IsReading)
            {
                _currentHeadWeight = (float)stream.ReceiveNext();
                _currentbodyWeight = (float)stream.ReceiveNext();
                _lookAtPoint = (Vector3)stream.ReceiveNext();
            }
        }
        #endregion

        #region Initializations
        protected override void Start()
        {
            _animator = GetComponent<Animator>();
            _character = GetComponent<vIControlAI>();
            base.Start();
        }
        #endregion

        #region Heartbeat
        protected override void FixedUpdate()
        {
            _updateIK = true;
            base.FixedUpdate();
        }
        protected override void LateUpdate()
        {
            if (GetComponent<PhotonView>().IsMine == true)
            {
                base.LateUpdate();
            }
            else
            {
                if (_animator == null || _character.currentHealth <= 0 || _character.isDead || _character.ragdolled || !_animator.enabled || (!_updateIK && _animator.updateMode == AnimatorUpdateMode.AnimatePhysics)) return;

                _updateIK = false;
                if (onPreUpdateSpineIK != null) onPreUpdateSpineIK.Invoke();
                if (_lookAtPoint != Vector3.zero)
                {
                    LookAtIK(_lookAtPoint, _currentHeadWeight, _currentbodyWeight);
                }
                if (onPosUpdateSpineIK != null && !IgnoreHeadTrackFromAnimator()) onPosUpdateSpineIK.Invoke();
            }
        }
        #endregion

        #region Overrides
        public override void SetMainLookTarget(Transform target)
        {
            if (GetComponent<PhotonView>().IsMine == true)
            {
                if (target.GetComponent<PhotonView>())
                {
                    GetComponent<PhotonView>().RPC(
                        "vAIHeadTrack_SetMainLookTarget",
                        RpcTarget.Others,
                        target.GetComponent<PhotonView>().ViewID
                    );
                }
            }
            base.SetMainLookTarget(target);
        }
        public override void RemoveMainLookTarget()
        {
            if (GetComponent<PhotonView>().IsMine == true)
            {
                GetComponent<PhotonView>().RPC(
                    "vAIHeadTrack_RemoveMainLookTarget",
                    RpcTarget.Others
                );
            }
            base.RemoveMainLookTarget();
        }
        protected override Vector3 GetLookPoint()
        {
            if (GetComponent<PhotonView>().IsMine == true)
            {
                _lookAtPoint = base.GetLookPoint();
            }
            return base.GetLookPoint();
        }
        #endregion

        #region RPCs
        [PunRPC]
        void vAIHeadTrack_SetMainLookTarget(int viewId)
        {
            PhotonView view = PhotonView.Find(viewId);
            if (view)
            {
                Debug.Log(view.transform, view.transform);
                SetMainLookTarget(view.transform);
            }
        }
        [PunRPC]
        void vAIHeadTrack_RemoveMainLookTarget()
        {
            RemoveMainLookTarget();
        }
        #endregion
    }
}
*/
