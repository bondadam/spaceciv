using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Utils
{
    // Start is called before the first frame update
    public static Vector3 getMousePosition(){
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    public static Vector2 getMousePosition2D(){
        Vector3 mousePos = getMousePosition();
        return new Vector2(mousePos.x, mousePos.y);
    }

    public static System.Random _R = new System.Random();

    public static float floatRange(float minNumber, float maxNumber)
    {
        return (float)_R.NextDouble() * (maxNumber - minNumber) + minNumber;
    }
    public static T randomEnumValue<T> ()
    {
        var v = Enum.GetValues (typeof (T));
        return (T) v.GetValue (_R.Next(v.Length));
    }

}
