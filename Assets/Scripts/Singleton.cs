using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T: MonoBehaviour
{
    static T instance;
    public static T Instance
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

    public static bool isExists { get { return instance != null; } }

}
