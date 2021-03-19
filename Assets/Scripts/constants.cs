using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Team{
        Player,
        CPU1,
        CPU2,
        CPU3,
        CPU4,
        Neutral
    }
public static class Constants
{

    public static Dictionary<Team, Color> team_colors = new Dictionary<Team, Color>()
        {
                {Team.Neutral, Color.black},
                {Team.Player, Color.white},
                {Team.CPU1,  Color.Lerp(Color.red, Color.green, 0.7f)},
                {Team.CPU2,  Color.magenta},
                {Team.CPU3, Color.Lerp(Color.cyan, Color.white, 0.2f)},
                {Team.CPU4, Color.red}
        }

    ;

}
