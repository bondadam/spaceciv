using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length_x, length_y, startpos_x, startpos_y;
    private float velocity_x, velocity_y;
    private float max_velocity, min_velocity;

    private float counter_x;
    private float counter_y;
    public float parallaxEffect;
    // Start is called before the first frame update
    void Start()
    {

        startpos_x = transform.position.x;
        startpos_y = transform.position.y;
        length_x = GetComponent<SpriteRenderer>().bounds.size.x;
        length_y = GetComponent<SpriteRenderer>().bounds.size.y;

        max_velocity = 1.0f;
        min_velocity = -max_velocity;

        //velocity_x = (Random.value*max_velocity) - (max_velocity/2.0f);
        //velocity_y = (Random.value*max_velocity) - (max_velocity/2.0f);
        //velocity_x = 1.0f;
        //velocity_y = 1.0f;


        // sin(radian) = value
        // 0   -> 0
        // 0.5 -> 0.5
        // 1.5 -> 1
        // 3.1 -> 0
        // 4.7 -> -1
        // 6.3 -> 0
        //counter_x = Random.value*6.283f;
        //counter_y = Random.value*6.283f;

        counter_x = 0;
        counter_y = 4.7f;

        velocity_x = Mathf.Sin(counter_x);
        velocity_y = Mathf.Sin(counter_y);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*
        velocity_x += ((Random.value*max_velocity) - (max_velocity/2.0f))/60;
        velocity_y += ((Random.value*max_velocity) - (max_velocity/2.0f))/60;

        if (velocity_x > 0){
            velocity_x = Mathf.Min(velocity_x, max_velocity);
        } else {
            velocity_x = Mathf.Max(velocity_x, min_velocity);
        }

        if (velocity_y > 0){
            velocity_y = Mathf.Min(velocity_y, max_velocity);
        } else {
            velocity_y = Mathf.Max(velocity_y, min_velocity);
        }*/

        counter_x += 0.001f;
        counter_y += 0.001f;
        velocity_x = Mathf.Sin(counter_x);
        velocity_y = Mathf.Sin(counter_y);

        float new_x = transform.position.x + parallaxEffect * Time.deltaTime * velocity_x;
        if (new_x > startpos_x + length_x || new_x < startpos_x - length_x){
            new_x = startpos_x;
        }
        float new_y = transform.position.y + parallaxEffect * Time.deltaTime * velocity_y;
        if (new_y > startpos_y + length_y || new_y < startpos_y - length_y){
            new_y = startpos_y;
        }
        transform.position = new Vector3(new_x, new_y, transform.position.z);
    }
}
