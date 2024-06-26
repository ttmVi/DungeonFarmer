using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonPersistent : MonoBehaviour
{
    public static SingletonPersistent instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
