using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Finger : MonoBehaviour
{
    public delegate void Fade_Out_Complete();
    private Fade_Out_Complete fade_out_complete_callback;
    public void Initialize(Fade_Out_Complete fade_out_complete_callback)
    {
        this.fade_out_complete_callback = fade_out_complete_callback;
    }
}
