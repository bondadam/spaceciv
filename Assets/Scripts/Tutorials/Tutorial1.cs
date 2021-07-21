using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Tutorial1 : Tutorial
{
    Structure pl;
    Structure pl2;
    List<Spaceship> spaceships;
    public override void start_tutorial()
    {
        Vector2 pos = get_structure_position(Object_Type.Planet, 0);
        pl = get_nth_structure(Object_Type.Planet, 0);
        show_finger(pos);
        show_text("Double tap planet to select your full army");
    }
    public override void Update_Custom()
    {
        if(this.state == 0)
        {
            if(pl.state == Selected_State.Full)
            {
               
                clear_finger();
                clear_text();
                Vector2 pos = get_structure_position(Object_Type.Planet, 1);
                pl2 = get_nth_structure(Object_Type.Planet, 0);
                show_finger(pos);
                show_text("Tap enemy planet to send the attack !");
                this.state += 1;
            }
        }else if(this.state == 1)
        {
            spaceships = get_list_of_spaceships();
            foreach(Spaceship sp in spaceships){
                if(sp.get_source() == pl && sp.get_population() > pl.get_population()){
                    this.state += 1;
                }
            }
            if(this.state == 1 && pl.state != Selected_State.Full){
                this.state = 0;
                Vector2 pos = get_structure_position(Object_Type.Planet, 0);
                pl = get_nth_structure(Object_Type.Planet, 0);
                show_finger(pos);
                show_text("Double tap planet to select your full army");
            }
        }else if(this.state == 2)
        {
            
                clear_finger();
                clear_text();
                show_text("Well done,  Commander uwu!");
        }
    }

    public IEnumerator unfreeze(float seconds_to_wait)
    {
        yield return new WaitForSeconds(seconds_to_wait);
       // this.frozen--;
    }

}
