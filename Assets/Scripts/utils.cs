using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public static class Utils
{

    public static int selected_level = 2;
    public static string selected_custom_level = "";
    public static System.Random _R = new System.Random();

    public static Vector3 getMousePosition(){
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
    

    public static Vector2 getMousePosition2D(){
        Vector3 mousePos = getMousePosition();
        return new Vector2(mousePos.x, mousePos.y);
    }

    public static Vector2 deep_copy_vector2(Vector2 to_copy){
        Vector2 result = new Vector2();
        result.x = to_copy.x;
        result.y = to_copy.y;
        return result;
    }


    public static float floatRange(float minNumber, float maxNumber) {
        return (float)_R.NextDouble() * (maxNumber - minNumber) + minNumber;
    }
    public static T randomEnumValue<T> (){

        var v = Enum.GetValues (typeof (T));
        return (T) v.GetValue (_R.Next(v.Length));
    }

    public static void Shuffle<T>(this IList<T> list)  {  
    int n = list.Count;  
    while (n > 1) {  
        n--;  
        int k = _R.Next(n + 1);  
        T value = list[k];  
        list[k] = list[n];  
        list[n] = value;  
    }  
}

public static bool check_alive(Team team, Game_State game_state){
        bool still_alive = false;
        for (int i = 0; i < game_state.planets.Count; i++){
            if (game_state.planets[i].team == team){
                still_alive = true;
                break;
            }
        }
        if (!still_alive){
            for (int i = 0; i < game_state.spaceships.Count; i++){
                if (game_state.spaceships[i].get_team() == team){
                    still_alive = true;
                    break;
                }
            }
        }
        return still_alive;
    }
    public static Vector2 find_center_of_weighted_points(List<Vector2> pts, List<float> weights)
    {
        float x = 0;
        float y = 0;
        float total_weights = 0;
        for (int i = 0; i < pts.Count; i++)
        {
            x += pts[i][0]*weights[i];
            y += pts[i][1]*weights[i];
            total_weights += weights[i];
        }
        Vector2 average_point = new Vector2(x/total_weights, y/total_weights);
        return average_point;
    }
}
