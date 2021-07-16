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

    public void initialize(bool custom, int number, Level_Difficulty difficulty, int completed_score){
        this.custom = custom;
        this.number = number;
        this.difficulty = difficulty;
        this.completed = completed; // 0: not completed, 1-3: completed with 1-3 stars
        GameObject NewObj = new GameObject();
        Image NewImage = NewObj.AddComponent<Image>();
        switch(completed_score)
        {
            case 1:
                NewImage.sprite = Resources.Load<Sprite>("1star_border"); 
                NewObj.GetComponent<RectTransform>().SetParent(this.transform);
                NewObj.SetActive(true); 
                NewObj.transform.localScale = new Vector3((float)0.7,(float)0.5,(float)1);
                NewObj.transform.position  = new Vector3((float)0, (float)1, (float)0);

                break;
            case 2:
                NewImage.sprite = Resources.Load<Sprite>("2star"); 
                NewObj.GetComponent<RectTransform>().SetParent(this.transform);
                NewObj.SetActive(true); 
                NewObj.transform.localScale = new Vector3((float)0.7,(float)0.5,(float)0.3);
                break;
            case 3:
                NewImage.sprite = Resources.Load<Sprite>("3star"); 
                NewObj.GetComponent<RectTransform>().SetParent(this.transform);
                NewObj.SetActive(true); 
                NewObj.transform.localScale = new Vector3((float)0.7,(float)0.5,(float)0.3);
                break;
            case -1:
                //this.image_display.color = Color.gray;
                this.gameObject.GetComponent<Button>().interactable = false;
                break;


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


