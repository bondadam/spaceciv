using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceLoad : MonoBehaviour
{

    private static SpaceLoad instance = null;
    void Awake()
    {
        /*if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            return;
        }
        Destroy(this.gameObject);
        */
    }

    public static void switchColors(Background_Color color){
        GameObject[] spaceBackgrounds= GameObject.FindGameObjectsWithTag("SpaceBackground");
        foreach (GameObject go in spaceBackgrounds){
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            sr.sprite = Resources.Load<Sprite>("Backgrounds/" + Constants.background_colors[color]);
        }
    }
}
