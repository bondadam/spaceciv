using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Tutorial : MonoBehaviour
{
    public delegate void Show_Tutorial_Finger(Vector2 pos);   
    public delegate void Clear_Tutorial_Finger();   
    public delegate void Show_Tutorial_Text(string txt);   
    public delegate void Clear_Tutorial_Text();   
    public delegate Vector2 Get_Structure_Position(Object_Type type, int pos);   
    public delegate Structure Get_Nth_Structure(Object_Type type, int pos);   
    public delegate List<Spaceship> Get_List_Of_Spaceships();   
    public Show_Tutorial_Finger show_finger;
    public Clear_Tutorial_Finger clear_finger;
    public Show_Tutorial_Text show_text;
    public Clear_Tutorial_Text clear_text;
    public Get_Structure_Position get_structure_position;
    public Get_Nth_Structure get_nth_structure;
    public Get_List_Of_Spaceships get_list_of_spaceships;
    public int state = 0;
    public void Initialize(Show_Tutorial_Finger show_finger, Clear_Tutorial_Finger clear_finger, Show_Tutorial_Text show_text, Clear_Tutorial_Text clear_text, Get_Structure_Position get_structure_position, Get_Nth_Structure get_nth_structure, Get_List_Of_Spaceships get_list_of_spaceships)
    {
        this.show_finger = show_finger;
        this.clear_finger = clear_finger;
        this.show_text = show_text;
        this.clear_text = clear_text;
        this.get_structure_position = get_structure_position;
        this.get_nth_structure = get_nth_structure;
        this.get_list_of_spaceships = get_list_of_spaceships;
        start_tutorial();
    }
    
    public virtual void start_tutorial()
    {
        
    }
    public virtual void Update_Custom()
    {

    }
}
