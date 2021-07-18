using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Editor_FrozenVoid : Editor_Object
{
    private SerializedSpaceEntity data;
    public TextMeshProUGUI positionValue;
    public Slider sizeSlider;
    public TextMeshProUGUI sizeValue;
    public void Initialize(Vector2 coords, On_Destroy_Callback on_destroy){
        this.data = new SerializedSpaceEntity();
        this.data.position_x = coords.x;
        this.data.position_y = coords.y;
        this.data.size = 1;

        this.sizeSlider.maxValue = Constants.FROZENVOID_MAX_SIZE;
        this.sizeSlider.value = (int)Constants.FROZENVOID_DEFAULT_SIZE;
        this.sizeSlider.minValue = Constants.FROZENVOID_MIN_SIZE;
        this.sizeValue.text = Constants.FROZENVOID_DEFAULT_SIZE.ToString();

        this.notify_destroy = on_destroy;

        this.open_databox();
        this.update_identity();
    }

     public void Initialize_Load(SerializedSpaceEntity data){
        this.data = data;

        this.sizeSlider.maxValue = Constants.FROZENVOID_MAX_SIZE;
        this.sizeSlider.value = (int)this.data.size;
        this.sizeSlider.minValue = Constants.FROZENVOID_MIN_SIZE;
        this.sizeValue.text = this.sizeSlider.value.ToString();

        this.close_databox();
        this.update_position();
        this.update_identity();
    }

    public void changeSize(){
        this.data.size = this.sizeSlider.value;
        this.sizeValue.text =  this.data.size.ToString();
        this.update_identity();
    }
    
    override public void update_identity(){
        this.transform.localScale = new Vector3(this.data.size, this.data.size, this.data.size);
    }
    override public void update_position(){
        this.data.position_x = this.transform.position.x;
        this.data.position_y = this.transform.position.y;
        this.positionValue.text = "X : " + this.data.position_x + "\nY : " + this.data.position_y;
    }

    public SerializedSpaceEntity get_data()
    {
        return this.data;
    }

}
