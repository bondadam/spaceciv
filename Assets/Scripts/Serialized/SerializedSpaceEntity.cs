using System;
using UnityEngine;

[Serializable]
public class SerializedSpaceEntity {

    public SerializedSpaceEntity()
    {
        this.size = 1;
    }
    public float position_x;
    public float position_y;
    public float size;

}