using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Team{
        Player,
        CPU,
        Neutral
    }
public static class Constants
{

    public static Dictionary<Team, Color> team_colors = new Dictionary<Team, Color>()
        {
                {Team.Neutral, Color.gray},
                {Team.Player, Color.white},
                {Team.CPU, Color.blue}
        }

    ;

}
