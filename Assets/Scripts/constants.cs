using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum Team{
        Player,
        CPU1,
        CPU2,
        CPU3,
        CPU4,
        Neutral
    }

public enum Selected_State{
    Unselected,
    Half,
    Full
}

public enum Object_Type{
    None,
    Planet
}
public static class Constants
{
    public static Color bluegreen = new Color(0.5f, 1.0f, 1.0f);
    public static Color bluey = new Color(0.25f, 0.25f, 1.0f);
    public static Color greeny = new Color(0.25f, 1.0f, 0.25f);

    public static float Long_Click_Duration = 1.0f;

    public const float PLANET_BASE_GROWTH = 15;

    public const int PLANET_DEFAULT_INITIAL_POPULATION = 0;
    public const int PLANET_DEFAULT_MAX_POPULATION = 99;

    public const int PLANET_ABSOLUTE_MAX_POPULATION = 199;

    public const float PLANET_MAX_SIZE = 5;

    public const float PLANET_MIN_SIZE = 0.5f;
    public const float PLANET_DEFAULT_SIZE = 1f;

    public static string USER_LEVEL_DIRECTORY_PATH = Application.persistentDataPath;
    public const string USER_LEVEL_DEFAULT_FILE_PATH = "/levels/saved_level.json";

    public const int USER_LEVEL_CODE = 99;

    public static string USER_LEVEL_DEFAULT_COMPLETE_PATH = String.Concat(USER_LEVEL_DIRECTORY_PATH, USER_LEVEL_DEFAULT_FILE_PATH);

    public static Dictionary<Team, Color> team_colors = new Dictionary<Team, Color>()
        {
                {Team.Neutral, bluegreen},
                {Team.Player, Color.white},
                {Team.CPU1,  bluey},
                {Team.CPU2,  Color.magenta},
                {Team.CPU3, greeny},
                {Team.CPU4, Color.red}
        };
    
    public static Dictionary<int, String> level_paths = new Dictionary<int, string>(){
        {1, "Levels/level1"},
        {2, "Levels/Level2"},
        {3, "Levels/level3"}
        };

        public static int states_num =  Enum.GetNames(typeof(Selected_State)).Length;

        public static Dictionary<Selected_State, float> selected_value = new Dictionary<Selected_State, float>(){
        {Selected_State.Unselected, 0},
        {Selected_State.Half, 0.5f},
        {Selected_State.Full, 1}
        };

        public static Dictionary<Selected_State, Color> selected_color = new Dictionary<Selected_State, Color>(){
        {Selected_State.Unselected, Color.white},
        {Selected_State.Half, Color.gray},
        {Selected_State.Full, Color.black}
        };

}
