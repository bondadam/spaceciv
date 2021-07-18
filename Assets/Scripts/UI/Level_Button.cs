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

    public Image[] star1;

    public Image[] star2;

    public Image[] star3;
    private Animator animator;
    public void Start()
    {
        this.animator = this.GetComponentInChildren<Animator>();
        this.transform.localScale = Vector3.one;

    }
    public void show()
    {
        this.gameObject.SetActive(true);
        this.transform.localScale = Vector3.one;
    }

    public void hide()
    {
        this.gameObject.SetActive(false);
    }

    public void show_star(Image[] star, bool show){
        star[0].gameObject.SetActive(!show);
        star[1].gameObject.SetActive(show);
    }

    public void initialize(bool custom, int number, Level_Difficulty difficulty, int completed_score, float time_to_appear)
    {
        this.custom = custom;
        this.number = number;
        this.difficulty = difficulty;
        this.completed = completed; // 0-3 = completed with 0-3 stars, -1: not completed
        switch (completed_score)
        {
            case 0:
                show_star(this.star1, false);
                show_star(this.star2, false);
                show_star(this.star3, false);
                break;
            case 1:
                show_star(this.star1, false);
                show_star(this.star2, false);
                show_star(this.star3, false);
                break;
            case 2:
                show_star(this.star1, true);
                show_star(this.star2, false);
                show_star(this.star3, false);
                break;
            case 3:
                show_star(this.star1, true);
                show_star(this.star2, true);
                show_star(this.star3, false);
                break;
            case 4:
                show_star(this.star1, true);
                show_star(this.star2, true);
                show_star(this.star3, true);
                break;
            case -1:
                //this.image_display.color = Color.gray;
                this.gameObject.GetComponent<Button>().interactable = false;
                break;
        }
        this.transform.localScale = new Vector3(0,0,0);
        StartCoroutine(AppearAfterSeconds(time_to_appear));
        this.update_display();
    }

    IEnumerator AppearAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        this.animator.Play("Appear");
    }

    public void update_display()
    {
        this.text_display.text = number.ToString();
        //this.image_display.sprite = Resources.Load<Sprite>(Constants.level_difficulty_icons[this.difficulty]);
        Color final_color = Color.clear;
        switch(this.difficulty){
            case Level_Difficulty.Easy:
                final_color = new Color(0.309f, 0.623f, 0.137f);
                break;
            case Level_Difficulty.Medium:
                final_color = new Color(0.137f, 0.341f, 0.623f);
                break;
            case Level_Difficulty.Hard:
                final_color = new Color(1, 0.541f, 0);
                break;
            case Level_Difficulty.Impossible:
                final_color = new Color(0.623f, 0.137f, 0.2f);
                break;
        }
        this.image_display.color = final_color;
    }
    public void Button_Click()
    {
        this.menu_levels.level_clicked(this.number, this.custom);

    }
}


