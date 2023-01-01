using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindInactiveObject : MonoBehaviour
{
    public static FindInactiveObject instance;

    void Awake()
    {
        instance = this;
    }

    public GameObject find(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for(int i = 0; i < objs.Length; i++)
        {
            if(objs[i].hideFlags == HideFlags.None)
            {
                if(objs[i].name == name)
                {
                    return objs[i].gameObject;
                }
            }
        }

        return null;
    }
}
