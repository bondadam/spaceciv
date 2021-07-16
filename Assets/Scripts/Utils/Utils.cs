using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public static class Utils
{

    public static int selected_level = 2;
    public static string selected_custom_level = "";
    public static List<int> levels_completed = new List<int>();
    public static System.Random _R = new System.Random();

    public static Vector3 getMousePosition(){
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
    
    public static float randomlyAlterPitchPlanet(float currentPitch, float planetSize){
        return ((currentPitch + 0.1f - (0.1f * planetSize)) * UnityEngine.Random.Range(0.95f,1.05f));
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
    public static bool collision_circle_line(Vector2 circle_center, float radius, Vector2 pt1, Vector2 pt2)
    {
        float dist1 = Mathf.Sqrt(Mathf.Pow(pt1[0]-circle_center[0],2) + Mathf.Pow(pt1[1]-circle_center[1],2));
        float dist2 = Mathf.Sqrt(Mathf.Pow(pt2[0]-circle_center[0],2) + Mathf.Pow(pt2[1]-circle_center[1],2));
        if (dist1 < radius || dist2 < radius) return true;
        float distX = pt1[0] - pt2[0];
        float distY = pt1[1] - pt2[1];
        float line_dist = Mathf.Sqrt( (distX*distX) + (distY*distY) );
        float dot = ( ((circle_center[0]-pt1[0])*(pt2[0]-pt1[0])) + ((circle_center[1]-pt1[1])*(pt2[1]-pt1[1])) ) / Mathf.Pow(line_dist,2);

        float closestX = pt1[0] + (dot * (pt2[0]-pt1[0]));
        float closestY = pt1[1] + (dot * (pt2[1]-pt1[1]));

        float dist_from_pt1 = Mathf.Sqrt(Mathf.Pow(pt1[0]-closestX,2) + Mathf.Pow(pt1[1]-closestY,2));
        float dist_from_pt2 = Mathf.Sqrt(Mathf.Pow(pt2[0]-closestX,2) + Mathf.Pow(pt2[1]-closestY,2));
        if (dist_from_pt1 > line_dist || dist_from_pt2 > line_dist) return false;

        distX = closestX - circle_center[0];
        distY = closestY - circle_center[1];
        float distance = Mathf.Sqrt( (distX*distX) + (distY*distY) );

        if (distance <= radius) {
            return true;
        }
        return false;
    }
}
