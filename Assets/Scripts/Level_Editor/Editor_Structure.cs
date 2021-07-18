using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public abstract class Editor_Structure : Editor_Object
{
    public TMP_Dropdown team_dropdown;
    public Slider initPopSlider;
    public TextMeshProUGUI initialPopValue;
    public Slider maxPopSlider;
    public TextMeshProUGUI maxPopValue;
    public TextMeshProUGUI positionValue;
    public abstract void Initialize(Vector2 coords, On_Destroy_Callback on_destroy);
    public abstract void change_team(int new_team);
    public abstract void change_initPop();
    public abstract void change_maxPop();

}
