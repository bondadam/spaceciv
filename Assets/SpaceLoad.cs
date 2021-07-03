using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceLoad : MonoBehaviour
{

    private static SpaceLoad instance = null;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            return;
        }
        Destroy(this.gameObject);
    }
}
