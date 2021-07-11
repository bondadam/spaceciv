using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Level_Button : MonoBehaviour
{
    private bool custom;
    private int number;
    private Level_Difficulty difficulty;
	public Text text_display;
    public Image image_display;
	public Menu_Levels menu_levels;
    private bool completed;

    public void Start(){
        this.transform.localScale = Vector3.one;
        
    }
    public void show(){
        this.gameObject.SetActive(true);
        this.transform.localScale = Vector3.one;
    }

    public void hide(){
        this.gameObject.SetActive(false);
    }

    public void initialize(bool custom, int number, Level_Difficulty difficulty, bool completed){
        this.custom = custom;
        this.number = number;
        this.difficulty = difficulty;
        this.completed = completed;
        if(completed)
        {
            GameObject NewObj = new GameObject();
            Image NewImage = NewObj.AddComponent<Image>();
            NewImage.sprite = Resources.Load<Sprite>("star"); 
            NewObj.GetComponent<RectTransform>().SetParent(this.transform);
            NewObj.SetActive(true); 
            NewObj.transform.localScale = new Vector3((float)0.3,(float)0.3,(float)0.3);
        }
        this.update_display();
    }

    public void update_display(){
        this.text_display.text = number.ToString();
        this.image_display.sprite = Resources.Load<Sprite>(Constants.level_difficulty_icons[this.difficulty]);
    }
	public void Button_Click()
	{
		this.menu_levels.level_clicked(this.number, this.custom);

	}
}


