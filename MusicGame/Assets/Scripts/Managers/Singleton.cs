using UnityEngine;
using System.Collections;

public abstract class Singleton<T> : MonoBehaviour
    where T : Singleton<T>
{
    public static T Instance { get; set; }

	protected virtual void Awake ()
    {
        if (Instance == null)
        {
            Instance = this as T;
            Debug.Log("Created: " + Instance.GetType());
        }
	}
}
