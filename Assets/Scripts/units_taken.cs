using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class units_taken : MonoBehaviour
{

    private TextMeshPro display;
    private int number_taken_total;

    public Dictionary<Planet, int> number_from_planet;
    // Start is called before the first frame update
    void Awake()
    {
        this.number_from_planet = new Dictionary<Planet, int>();
        this.display = this.GetComponent<TextMeshPro>();
        this.hide();
    }

    public void show(){
        this.display.enabled = true;
    }

    public void hide(){
        this.display.enabled = false;
    }

    public void selection_done(){
        this.number_from_planet = new Dictionary<Planet, int>();
        this.number_taken_total = 0;
        this.hide();
    }

    private void update_count(){
        int total = 0;
        foreach (Planet p in  this.number_from_planet.Keys){
            total += this.number_from_planet[p];
        }
        this.number_taken_total = total;
    }

    public void set_position_to_mouse(){
        this.transform.position = Utils.getMousePosition();
    }

    public void display_units(){
        this.display.text = this.number_taken_total.ToString();
    }

    public void add(Planet planet, int number_taken){
        if (number_from_planet.ContainsKey(planet)){
            this.number_from_planet[planet] += number_taken;
        } else {
            this.number_from_planet.Add(planet, number_taken);
        }
        this.update_count();
        this.display_units();
    }

    public int get(Planet planet = null){
        if (planet == null){
            return this.number_taken_total;
        } else if (!this.number_from_planet.ContainsKey(planet)){
            return 0;
        } else {
            return this.number_from_planet[planet];
        }
    }

}
