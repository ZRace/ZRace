using UnityEngine;

public class vBuilderCollisionSensor : MonoBehaviour
{
    public TriggerEvent onTriggerStay;
    public TriggerEvent onTriggerExit;
    public CapsuleCollider _capsuleCollider;
    private void Awake()
    {
        _capsuleCollider = GetComponent<CapsuleCollider>();
        if(_capsuleCollider)
        {
            _capsuleCollider.gameObject.tag = "Action";
            _capsuleCollider.isTrigger = true;
        }
           
    }
    public void OnTriggerStay(Collider other)
    {
        onTriggerStay.Invoke(other);
    }

    public void OnTriggerExit(Collider other)
    {
        onTriggerExit.Invoke(other);
    }

    [System.Serializable]
    public class TriggerEvent : UnityEngine.Events.UnityEvent<Collider>
    {
        
    }
}