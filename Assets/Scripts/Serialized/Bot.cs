using System;
using UnityEngine;

[Serializable]
public class SerializedBot
{
    public SerializedBot(){

    }

    public SerializedBot(Team team, Bot_Type type, float decision_interval){
        this.team = team;
        this.type = type;
        this.decision_interval = decision_interval;
    }
    public Team team;
    public Bot_Type type;
    public float decision_interval;
}