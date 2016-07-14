using UnityEngine;
using System.Collections;

public delegate void TriggerEventHandler<T>(Triggerable<T> triggerable);

public abstract class Triggerable<T>
{
    protected T item;

    public System.Action<T> Use;

    public System.Action UsedUp;

    public System.Action Reseting;

    bool isUsedUp = false;

    public bool canBeUsedUp = false;

    public int maxUses = 1;

    private int useCount = 1;

    protected Triggerable(T triggerItem, int uses, bool depleteable) 
    {
        item = triggerItem;
        maxUses = uses;
        useCount = maxUses;
        canBeUsedUp = depleteable;
    }

    public virtual void Trigger()
    {
        if (canBeUsedUp)
        {
            if (!isUsedUp)
            {
                if (Use != null)
                {
                    Use(item);
                }

                useCount--;

                if (useCount == 0)
                {
                    isUsedUp = true;

                    if (UsedUp != null)
                    {
                        UsedUp();
                    }
                }
            }
        }
        else
        {
            Use(item);
        }
    }

    public void Reset()
    {
        isUsedUp = false;
        useCount = maxUses;

        if (Reseting != null)
        {
            Reseting();
        }
    }

    public abstract void Setup();
}
