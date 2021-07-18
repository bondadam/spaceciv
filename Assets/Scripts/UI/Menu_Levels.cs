using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;


public class Menu_Levels : MonoBehaviour
{
    public GameObject prefabButton;
    public Transform grid;

    public ScrollView scrollview;

    // Start is called before the first frame update
    void Start()
    {
        /*
        List<string> save_file_names = Save_File_Manager.getSimpleFileNames();
        foreach(string file_name in save_file_names)
        {
            GameObject userLevelButton = (GameObject)Instantiate(prefabButton);
            userLevelButton.transform.SetParent(ParentPanel, false);
            userLevelButton.transform.localScale = new Vector3(1, 1, 1);
            Button userLevelTempButton = userLevelButton.GetComponent<Button>();
            userLevelButton.GetComponentInChildren<TextMeshProUGUI>().text = file_name;
            userLevelTempButton.onClick.AddListener(() => ButtonClicked(Constants.USER_LEVEL_CODE, file_name));
        }
        this.disable();          
        */
        int i = 0;
        foreach((string, Level_Difficulty) entry in Constants.level_paths)
        {
            var custom = false;
            int k = i;
            GameObject button_object = Instantiate(this.prefabButton) as GameObject;
			button_object.AddComponent<Level_Button>();
			button_object.SetActive(true);
		    Level_Button lb = button_object.GetComponent<Level_Button>();
            int level_completed = Utils.levels_completed[i];
            if(level_completed == 0 && i > 0 && Utils.levels_completed[i-1] == 0)
            {
                level_completed = -1;
            }
			lb.initialize(custom, k, entry.Item2, level_completed, i * 0.05f);
			button_object.transform.SetParent(grid);
            UnityEngine.UI.Button button = button_object.GetComponent<UnityEngine.UI.Button>();
            button.onClick.AddListener(() => level_clicked(k, custom));
            i += 1;
        }
       // ScrollRect scrollRect = GetComponentInChildren<ScrollRect>();
       // scrollRect.verticalScrollbar.value = 0.99f;
          //  float scrollValue = 1 + _element.anchoredPosition.y/scrollRect.content.GetHeight();
           // scrollRect.verticalScrollbar.value = _scrollValue;
     }

    public void level_clicked(int number, bool custom){
        this.start_level(number, "");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void start_level(int level, string level_name){
        Utils.selected_level = level;
        Utils.selected_custom_level = level_name;
        SceneManager.LoadScene("Level");
    }

    public void back(){
        SceneManager.LoadScene("Menu_Main");
    }
}
