using UnityEngine;
using System.Collections;

public abstract class TriggerableHolder<T> : MonoBehaviour
{
    protected T toTrigger;

    [SerializeField]
    protected bool canBeUsedUp = false;

    [SerializeField]
    protected int numberOfTriggerUses = 1;

    public Triggerable<T> triggerable;

    public TriggerEventHandler<T> Triggering;

    protected virtual void Awake()
    {
        triggerable.UsedUp += OnUsedUp;
        triggerable.Reseting += OnReset;
    }

    public abstract void OnUsedUp();

    public abstract void OnReset();
}
