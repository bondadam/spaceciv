using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Manager : MonoBehaviour
{

    public Level_Manager level_manager;

    public Camera camera;
    private bool dragging = false;

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
                Vector3 mousePos = Input.mousePosition;
                Debug.Log("Move_dragging");
                if (!Input.GetMouseButton(0))
                {
                    Debug.Log("Stop Move_dragging");
                    this.move_dragging = false;
                }
                else
                {
                    Debug.Log("Continue move_dragging");
                    // move camera along dragging axis
                    Vector3 diff_position = new Vector3(mousePos.x - old_mouse_position.x, mousePos.y - old_mouse_position.y, 0);
                    Debug.Log(diff_position);
                    this.camera.transform.position = new Vector3(this.camera.transform.position.x - diff_position.x * 0.0125f, this.camera.transform.position.y - diff_position.y * 0.0125f, this.camera.transform.position.z);

                    //Debug.Log("old_mouse_position : \n" + old_mouse_position);
                    //Debug.Log("mousePos2D : \n" + mousePos2D);
                }
                old_mouse_position = new Vector2(mousePos.x, mousePos.y);
            }
            else
            {
                
                if (this.dragging)
                {
                    RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                    if (hit.collider != null && hit.collider.gameObject.tag == "Planet")
                    {
                        Planet clicked_planet = hit.collider.gameObject.GetComponent<Planet>();
                        if (Input.GetMouseButton(0))
                        {
                            if (clicked_planet.team == Team.Player)
                            {
                                this.level_manager.select(clicked_planet);
                            }
                        }
                        else
                        {
                            this.level_manager.send_spaceship_to_planet(clicked_planet);
                            this.dragging = false;
                        }
                    }
                    else if (!Input.GetMouseButton(0))
                    {
                        // Deselected in empty space
                        // Assume user wanted to cancel action and return pop to each planet
                        //this.level_manager.cancel_selected_planets();
                        this.dragging = false;
                    }
                    else
                    {
                        this.level_manager.set_currently_selected_planet(null);
                    }
                }
                else
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                        if (hit.collider != null)
                        {
                            if (hit.collider.gameObject.tag == "Planet")
                            {
                                Planet clicked_planet = hit.collider.gameObject.GetComponent<Planet>();
                                if (clicked_planet.team == Team.Player)
                                {
                                    this.level_manager.select(clicked_planet);
                                    this.dragging = true;
                                }
                                else
                                {
                                    this.level_manager.send_spaceship_to_planet(clicked_planet);
                                }
                            }
                        }
                        else if (!move_dragging)
                        {
                            this.move_dragging = true;
                            //this.old_mouse_position = Utils.deep_copy_vector2(mousePos2D);
                            Vector3 mousePos = Input.mousePosition;
                            this.old_mouse_position = new Vector2(mousePos.x, mousePos.y);
                        }
                    }
                }

            }
        }
    }
}
