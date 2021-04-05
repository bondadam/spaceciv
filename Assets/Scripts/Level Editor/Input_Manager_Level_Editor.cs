using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Manager_Level_Editor : MonoBehaviour
{
    private bool move_dragging;
    public Level_Editor_Manager Level_Editor_Manager;
    private Vector2 old_mouse_position;
    private bool holding;
    public Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        move_dragging = false;
        holding = false;
        old_mouse_position = new Vector2();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos2D = Utils.getMousePosition2D();

        if (this.move_dragging)
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
                Planet clicked_planet = hit.collider.gameObject.GetComponent<Planet>();
                if (clicked_planet.team == Team.Player)
                { }
            }
        }
    }
}
