using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANAR
{
    public abstract class Singleton<T> where T : class
    {
        protected static T instance = null;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = System.Activator.CreateInstance(typeof(T)) as T;
                }
                return instance;
            }
        }
    }
}
public class MonoSingleton <T> : MonoBehaviour where T: MonoBehaviour
{
    protected static T instance = null;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new GameObject("@" + typeof(T).ToString(), typeof(T)).AddComponent<T>();
                DontDestroyOnLoad(instance);
                Debug.Log("@" + typeof(T).ToString() + "Singleton생성");
            }
            return instance;
        }
    }
}

public class ComponentSingleton<T> : MonoBehaviour where T : ComponentSingleton<T>
{
    protected static T instance = null;

    public static T Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = GameObject.FindObjectOfType<T>();
            if (instance != null) return instance;
            throw new NullReferenceException($"Does not exit instance of {typeof(T).Name}");
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = (T)this;
        }
        else if (instance != this)
        {
            Debug.LogError($"Singleton<{typeof(T).Name}> must exist only one instance. Destroy this Component", this.gameObject);
            Destroy(this);
        }
    }

    protected virtual void OnDestroy()
    {
        instance = null;
    }
}
