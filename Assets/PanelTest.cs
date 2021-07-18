using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelTest : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        this.gameObject.transform.position = new Vector3(0,0,0);
        textMeshProUGUI.text = "X: " + this.gameObject.transform.position.x + "\nY: " + this.gameObject.transform.position.y;
    }
}
