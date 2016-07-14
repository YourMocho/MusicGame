using UnityEngine;
using System.Collections;

public class TriggerHandler<TThis, TTriggerType> : Singleton<TThis>
    where TThis : TriggerHandler<TThis, TTriggerType>
{
    public void HandleTriggerable(Triggerable<TTriggerType> triggerable)
    {
        triggerable.Trigger();
    }
}
