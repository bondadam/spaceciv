using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Manager : MonoBehaviour
{

    public Level_Manager level_manager;

    public Camera camera;

    private bool holding = false;

    private float holding_time = 0f;

    private bool move_dragging = false;

    private Vector2 old_mouse_position;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (!Pause_Menu.get_paused())
        {

            Vector2 mousePos2D = Utils.getMousePosition2D();

            if (this.move_dragging)
            {
                //Debug.Log(mousePos2D);
                Vector3 mousePos = Input.mousePosition;
                if (!Input.GetMouseButton(0))
                {
                    this.move_dragging = false;
                }
                else
                {
                    // move camera along dragging axis
                    Vector3 diff_position = new Vector3(mousePos.x - old_mouse_position.x, mousePos.y - old_mouse_position.y, 0);
                    Vector3 new_camera_position = new Vector3(this.camera.transform.position.x - diff_position.x * 0.0125f, this.camera.transform.position.y - diff_position.y * 0.0125f, this.camera.transform.position.z);
                    if (new_camera_position.x > -15 && new_camera_position.x < 15 && new_camera_position.y < 10 && new_camera_position.y > -10){
                        this.camera.transform.position = new_camera_position;
                    }
                }
                old_mouse_position = new Vector2(mousePos.x, mousePos.y);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                // If we clicked this frame
                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                if (hit.collider == null)
                {
                    // Just clicked in empty space, start move dragging
                    this.move_dragging = true;
                    Vector3 mousePos = Input.mousePosition;
                    this.old_mouse_position = new Vector2(mousePos.x, mousePos.y);
                }
                else
                {
                    // Clicked on a planet
                    Structure clicked_planet = hit.collider.gameObject.GetComponent<Structure>();
                    if (clicked_planet.team == Team.Player)
                    {
                        this.holding = true;
                        clicked_planet.select();
                    }
                    else
                    {
                        this.level_manager.send_spaceship_to_planet(clicked_planet);
                    }
                }
            }

            else if (holding && Input.GetMouseButton(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                if (hit.collider != null)
                {
                    Structure clicked_planet = hit.collider.gameObject.GetComponent<Structure>();
                    if (clicked_planet.team == Team.Player)
                    {
                        holding_time += Time.deltaTime;
                        if (holding_time >= Constants.Long_Click_Duration){
                            holding_time = 0;
                            holding = false;
                            this.level_manager.send_spaceship_to_planet(clicked_planet);
                            clicked_planet.unselect();
                        }
                    }
                    else
                    {
                        holding = false;
                    }
                }
                else
                {
                    holding = false;
                }
            }
            else
            {
                holding = false;
            }
        }
    }
}
