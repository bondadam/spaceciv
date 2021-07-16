using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sun : SpaceEntity
{
    
    


    void Start()
    {

    }


    public void Initialize(SerializedSpaceEntity serializedSpaceEntity, string name)
    {

        this.name = name;
        this.transform.position = new Vector3(serializedSpaceEntity.position_x, serializedSpaceEntity.position_y, 0);
        this.tag = "Sun";   

        this.m_SpriteRenderer = this.GetComponentInChildren<SpriteRenderer>();


        this.transform.localScale = new Vector3(serializedSpaceEntity.size, serializedSpaceEntity.size, 1);

        
    }



    public void Update()
    {

    }


}
