using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialPart : MonoBehaviour
{
    public Button previous_button;
    public Button next_button;
    public TextMeshProUGUI page_count;
    private int current_page;
    
    void Start()
    {

        previous_button.gameObject.SetActive(false);

    }

    public void Initialize(int current_page, int page_total){
        this.page_count.text = string.Format("{0}/{1}", current_page, page_total);
        this.current_page = current_page;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
