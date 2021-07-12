using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.animator.GetCurrentAnimatorStateInfo(0).IsName("SpaceshipExplosion") && 
           this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f){
               GameObject.Destroy(this.gameObject);
           }
    }
}
