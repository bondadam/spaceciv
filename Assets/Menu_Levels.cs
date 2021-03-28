using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class Menu_Levels : MonoBehaviour
{
    public GameObject content;
    public GameObject prefabButton;
    public RectTransform ParentPanel;



    // Start is called before the first frame update
    void Start()
    {
        foreach(KeyValuePair<int, string> entry in Constants.level_paths)
            {
                GameObject goButton = (GameObject)Instantiate(prefabButton);
                goButton.transform.SetParent(ParentPanel, false);
                goButton.transform.localScale = new Vector3(1, 1, 1);
                Button tempButton = goButton.GetComponent<Button>();
                goButton.GetComponentInChildren<TextMeshProUGUI>().text = "Level " + entry.Key.ToString();
                tempButton.onClick.AddListener(() => ButtonClicked(entry.Key));
            }
        this.disable();          
     
     }

     void ButtonClicked(int buttonNo)
     {
         Debug.Log ("Button clicked = " + buttonNo);
         start_level(buttonNo);
     }  

    // Update is called once per frame
    void Update()
    {
        
    }

    public void start_level(int level){
        Utils.selected_level = level;
        SceneManager.LoadScene("DemoLevel");
    }

    public void enable(){
        this.gameObject.SetActive(true);
    }

    public void disable(){
        this.gameObject.SetActive(false);
    }
}
