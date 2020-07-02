using Invector.vCamera;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.Events;

namespace CBGames.UI
{
    public class DeathCamera : MonoBehaviour
    {
        [Tooltip("The key to press in order to have the camera switch to a new player " +
            "target if \"allowSwitching\" is true.")]
        [SerializeField] protected string keyToSwitchPrevious = "";
        [Tooltip("The key to press in order to have the camera switch to a new player " +
            "target if \"allowSwitching\" is true.")]
        [SerializeField] protected string keyToSwitchNext = "";
        [Tooltip("(Optional)The UI GameObject to enable when you die.")]
        [SerializeField] protected GameObject deathVisual = null;
        public UnityEvent OnEnableSwitching = new UnityEvent();
        public UnityEvent OnDisableSwitching = new UnityEvent();

        protected int _targetIndex = 0;
        protected bool _canSwitch = false;

        protected virtual void Awake()
        {
            if (deathVisual != null)
            {
                deathVisual.SetActive(false);
            }
        }

        public virtual bool SwitchCameraTarget(Transform target)
        {
            if (target == null) return false;
            FindObjectOfType<vThirdPersonCamera>().SetTarget(target);
            return true;
        }
        public virtual void SelectNextTarget()
        {
            vThirdPersonController[] lookTargets = FindObjectsOfType<vThirdPersonController>();
            _targetIndex += 1;
            if (_targetIndex >= lookTargets.Length)
            {
                _targetIndex = 0;
            }
            if (SwitchCameraTarget(lookTargets[_targetIndex].transform) == false)
            {
                SelectNextTarget();
            }
        }
        public virtual void SelectPreviousTarget()
        {
            vThirdPersonController[] lookTargets = FindObjectsOfType<vThirdPersonController>();
            _targetIndex -= 1;
            if (_targetIndex < 0)
            {
                _targetIndex = lookTargets.Length-1;
            }
            if (SwitchCameraTarget(lookTargets[_targetIndex].transform) == false)
            {
                SelectPreviousTarget();
            }
        }
        public virtual void EnableSwitching(bool isEnabled)
        {
            _canSwitch = isEnabled;
            if (deathVisual != null)
            {
                deathVisual.SetActive(isEnabled);
            }
        }

        protected virtual void Update()
        {
            if (_canSwitch)
            {
                if (Input.GetButtonUp(keyToSwitchPrevious))
                {
                    SelectPreviousTarget();
                }
                else if (Input.GetButtonUp(keyToSwitchNext))
                {
                    SelectNextTarget();
                }
            }
        }
    }
}