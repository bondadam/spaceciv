using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Tutorial1 : Tutorial
{
    Structure pl;
    public override void  start_tutorial()
    {
        Vector2 pos = get_structure_position(Object_Type.Planet, 0);
        pl = get_nth_structure(Object_Type.Planet, 0);
        show_finger(pos);
        show_text("Double tap planet to select your full army");
    }
    public override void Update_Custom()
    {
        Debug.Log("Update");
        if(this.state == 0)
        {
            if(pl.state == Selected_State.Full)
            {
                
                Vector2 pos = get_structure_position(Object_Type.Planet, 1);
                pl = get_nth_structure(Object_Type.Planet, 0);
                show_finger(pos);
                show_text("Tap enemy planet to send the attack !");
                this.state += 1;
            }
        }
    }
}
