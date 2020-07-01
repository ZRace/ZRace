using UnityEngine;
using UnityEngine.Events;

public class GenericEventCaller : MonoBehaviour
{
    [Tooltip("Call these events when this objects calls onAwake (before on start)")]
    [SerializeField] protected bool onAwake = false;
    [Tooltip("Call these events when starting this object")]
    [SerializeField] protected bool onStart = false;
    [Tooltip("Call these events when this gameobject is enabled.")]
    [SerializeField] protected bool onEnable = false;
    [Tooltip("Call these events when this gameobject is disabled")]
    [SerializeField] protected bool onDisable = false;
    [SerializeField] protected UnityEvent EventsToCall = new UnityEvent();

    protected virtual void OnEnable()
    {
        if (onEnable == true)
        {
            EventsToCall.Invoke();
        }
    }

    protected virtual void OnDisable()
    {
        if (onDisable == true)
        {
            EventsToCall.Invoke();
        }
    }

    protected virtual void Awake()
    {
        if (onAwake == true)
        {
            EventsToCall.Invoke();
        }
    }

    protected virtual void Start()
    {
        if (onStart == true)
        {
            EventsToCall.Invoke();
        }
    }
}
