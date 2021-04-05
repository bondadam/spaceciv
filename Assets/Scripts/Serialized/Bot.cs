using System;
using UnityEngine;

[Serializable]
public class SerializedBot
{
    public SerializedBot(){

    }

    public SerializedBot(Team team, String type, float decision_interval){
        this.team = team;
        this.type = type;
        this.decision_interval = decision_interval;
    }
    public Team team;
    public String type;
    public float decision_interval;
}