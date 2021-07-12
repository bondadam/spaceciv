using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Standard_Button : MonoBehaviour
{
    // Start is called before the first frame update

    private AudioSource audioSource;

    public void Awake(){
        this.audioSource = this.GetComponent<AudioSource>();
        this.gameObject.GetComponent<Button>().onClick.AddListener(() => Debug.Log("Clicked")); //this.audioSource.Play());
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
