using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click_Manager : MonoBehaviour
{

    public Level_Manager level_manager;
    private bool dragging = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (this.dragging){
            Vector2 mousePos2D = Utils.getMousePosition2D();
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject.tag == "Planet") {
                Planet clicked_planet = hit.collider.gameObject.GetComponent<Planet>();
                if (Input.GetMouseButton(0)){
                    this.level_manager.select(clicked_planet);
                } else {
                    this.level_manager.send_spaceship_to_planet(clicked_planet);
                    this.dragging = false;
                }
            } else if (!Input.GetMouseButton(0)){
                // Deselected in empty space
                // Assume user wanted to cancel action and return pop to each planet
                this.level_manager.cancel_selected_planets();
                this.dragging = false;
            } else {
                this.level_manager.set_currently_selected_planet(null);
            }
        } else {
            if (Input.GetMouseButtonDown(0)) {
                Vector2 mousePos2D = Utils.getMousePosition2D();
                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                if (hit.collider != null && hit.collider.gameObject.tag == "Planet") {
                    Planet clicked_planet = hit.collider.gameObject.GetComponent<Planet>();
                    this.level_manager.select(clicked_planet);
                    this.dragging = true;
                }
            }
        }

    }
}
