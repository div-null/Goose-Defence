using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T: MonoBehaviour
{
    T instance;
    public T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<T>();
            }
            return instance;
        }
    }

    bool isExists { get { return instance != null; } }

}
