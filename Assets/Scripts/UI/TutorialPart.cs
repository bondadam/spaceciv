using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialPart : MonoBehaviour
{
    void Start()
    {

    }

    public void close_tutorial(){
        Time.timeScale = 1;
        GameObject.Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
