using UnityEngine;
using System.Collections;

public abstract class BaseSingletonObject<T> : BaseScriptableObject
    where T : BaseSingletonObject<T>
{
    public static T Instance { get; set; }

    protected BaseSingletonObject()
    {
        if (Instance == null)
        {
            Instance = this as T;
        }
    }
}
