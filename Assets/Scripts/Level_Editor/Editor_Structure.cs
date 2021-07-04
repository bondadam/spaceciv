using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public abstract class Editor_Structure : MonoBehaviour
{
    public TMP_Dropdown team_dropdown;
    public Slider initPopSlider;
    public TextMeshProUGUI initialPopValue;
    public Slider maxPopSlider;
    public TextMeshProUGUI maxPopValue;
    public TextMeshProUGUI positionValue;
    public Canvas data_Box;
    protected On_Destroy_Callback notify_destroy;
    public delegate void On_Destroy_Callback(Editor_Structure structure);
    public abstract void Initialize(Vector2 coords, On_Destroy_Callback on_destroy);
    public abstract void update_identity();
    public abstract void update_position();
    public abstract void change_team(int new_team);
    public void move(Vector2 new_position)
    {
        this.transform.position = new Vector3(new_position.x, new_position.y, this.transform.position.z);
        this.update_position();
    }
    public abstract void change_initPop();
    public abstract void change_maxPop();

    public void open_databox()
    {
        this.data_Box.gameObject.SetActive(true);
    }

    public void close_databox()
    {
        this.data_Box.gameObject.SetActive(false);
    }

    public void destroy()
    {
        if (notify_destroy != null)
        {
            notify_destroy(this);
        }

        GameObject.Destroy(this.gameObject);
    }

}
