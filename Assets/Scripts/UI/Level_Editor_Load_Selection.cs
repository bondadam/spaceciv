using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Level_Editor_Load_Selection : MonoBehaviour
{

    public GameObject prefabButton;
    public GameObject content;
	private List<string> level_list;


	void Start () {
        this.level_list = Save_File_Manager.getAllSimpleFileNames();
		foreach(string level_path in this.level_list)
		{
			GameObject button_object = Instantiate(this.prefabButton) as GameObject;
            button_object.SetActive(true);
            button_object.transform.SetParent(content.transform);
            button_object.transform.localScale = new Vector3(1,1,1);
            Button button = button_object.GetComponent<Button>();
            TMP_Text button_label = button.GetComponentInChildren<TMP_Text>();
            button_label.text = level_path;
            string current_level_path = string.Copy(level_path); // deep copy so the listener doesn't update level_path value
            button.onClick.AddListener(() => level_clicked(current_level_path));
        }
	}

    public void level_clicked(string level_path){
        PlayerPrefs.SetString(Constants.EDITOR_CURRENT_LEVEL_NAME_PLAYER_PREF, level_path);
        SceneManager.LoadScene("Level_Editor");
    }
	
    public void back(){
        SceneManager.LoadScene("Level_Editor_Menu");
    }
}
