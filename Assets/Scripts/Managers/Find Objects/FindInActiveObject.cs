using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FindInactiveObject : MonoBehaviour
{
    public static FindInactiveObject instance;

    void Awake()
    {
        instance = this; 

        if(instance == null)
        {
            Debug.Log("Null");
        }

        else
        {
            Debug.Log("Not Null");
        }
    }

    public GameObject find(string name)
    {
        Debug.Log("Triggered");
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for(int i = 0; i < objs.Length; i++)
        {
            if(objs[i].hideFlags == HideFlags.None)
            {
                if(objs[i].name == name)
                {
                    Debug.Log("Returned");
                    return objs[i].gameObject;
                }
            }
        }

         Debug.Log("Null");
        return null;
    }
}
