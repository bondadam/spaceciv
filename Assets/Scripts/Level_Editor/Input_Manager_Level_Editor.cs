using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Input_Manager_Level_Editor : MonoBehaviour
{
    private bool move_dragging;
    public Level_Editor_Manager level_Editor_Manager;
    private Vector2 old_mouse_position;
    private bool holding;
    private bool moving_planet;
    public Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        move_dragging = false;
        holding = false;
        moving_planet = false;
        old_mouse_position = new Vector2();
    }

    public void hold()
    {
        this.holding = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos2D = Utils.getMousePosition2D();
        bool noUIControlsInUse = EventSystem.current.currentSelectedGameObject == null;

        if (noUIControlsInUse)
        {
            if (this.holding && Input.GetMouseButton(0)){

            }
            if (this.move_dragging && !holding)
            {
                Vector3 mousePos = Input.mousePosition;
                if (!Input.GetMouseButton(0))
                {
                    this.move_dragging = false;
                }
                else
                {
                    // move camera along dragging axis
                    Vector3 diff_position = new Vector3(mousePos.x - old_mouse_position.x, mousePos.y - old_mouse_position.y, 0);
                    camera.transform.position = new Vector3(this.camera.transform.position.x - diff_position.x * 0.0125f, this.camera.transform.position.y - diff_position.y * 0.0125f, this.camera.transform.position.z);
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                Debug.Log(holding);
                // If we clicked this frame
                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                if (hit.collider == null)
                {
                    if (holding)
                    {
                        Debug.Log("Placing planet");
                        level_Editor_Manager.place_selected(mousePos2D);
                        this.holding = false;
                    }
                    else
                    {
                        // Just clicked in empty space, start move dragging
                        this.move_dragging = true;
                    }
                }
                else
                {
                    // Clicked on a planet
                    Editor_Planet clicked_planet = hit.collider.gameObject.GetComponent<Editor_Planet>();
                    // Close all other databoxes before opening this one
                    level_Editor_Manager.close_all_databoxes();
                    clicked_planet.open_databox();
                    this.holding = true;
                    level_Editor_Manager.choose_planet(clicked_planet);
                }
            }
            else if (holding && Input.GetMouseButton(0)){
                if(old_mouse_position[0] != Input.mousePosition.x || old_mouse_position[1] != Input.mousePosition.y)
                {
                    level_Editor_Manager.move_chosen_planet(mousePos2D);
                }
            }
            else if (!Input.GetMouseButton(0))
            {
                move_dragging = false;
            } else if (holding && Input.GetMouseButtonUp(0)){
                holding = false;
            }
        }
        old_mouse_position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }
}