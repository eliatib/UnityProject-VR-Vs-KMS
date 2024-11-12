using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Utility
{
    public class Singleton<T> : MonoBehaviour where T:MonoBehaviour
    {
        public static T Instance { get; private set; }
        public virtual void Awake()
        {
            if(Instance != null)
            {
                DestroyImmediate(this);
                return;
            }

            Instance = this as T;
            DontDestroyOnLoad(this);
        }
    }
}

//public class Singleton
//{
//    private static readonly Lazy<Singleton> _lazy = new Lazy<Singleton>(() => new Singleton());

//    public static Singleton Instance { get => _lazy.Value; }
//}