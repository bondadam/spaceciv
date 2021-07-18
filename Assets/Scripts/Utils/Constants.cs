using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum Team
{
    Player,
    CPU1,
    CPU2,
    CPU3,
    CPU4,
    Neutral
}

public enum Bot_Type
{
    BlitzBot,    // 0
    DefensiveBot,    // 1
    ExpandingBot,    // 2
    JuggernautBot,    // 3
    ExpandingJuggernautBot,    // 4
    EmptyBot,    // 5
    ProximityBot,    // 6
    JuggernautProximityBot    // 7
}

public enum Selected_State
{
    Unselected,
    Half,
    Full
}

public enum Object_Type
{
    None,
    Move,
    Planet,
    Turret,
    Spacegun,
    FrozenVoid,
    Sun
}

public enum Background_Color{
    default_color,
    blue,
    aqua,
    red
}

public enum Level_Difficulty
{
    Easy,
    Medium,
    Hard,
    Impossible
}
public static class Constants
{
    public static Color bluegreen = new Color(0.5f, 1.0f, 1.0f);
    public static Color bluey = new Color(0.25f, 0.25f, 1.0f);
    public static Color greeny = new Color(0.25f, 1.0f, 0.25f);

    public static float Long_Click_Duration = 0.45f;

    public const float PLANET_BASE_GROWTH = 10;

    public const int PLANET_DEFAULT_INITIAL_POPULATION = 25;
    public const int PLANET_DEFAULT_MAX_POPULATION = 99;

    public const float FROZEN_SPACESHIP_RELATIVE_SPEED = 0.5f;

    public const int PLANET_ABSOLUTE_MAX_POPULATION = 199;

    public const float DEFAULT_RECORD_TIME = 90;
    public const float PLANET_MAX_SIZE = 5;
    public const float PLANET_MIN_SIZE = 0.5f;
    public const float PLANET_DEFAULT_SIZE = 1f;

    public const float BOT_DEFAULT_MAX_SPEED = 10f;
    public const float BOT_DEFAULT_SPEED = 5f;
    public const float BOT_DEFAULT_MIN_SPEED = 0.5f;

    public const float FROZENVOID_MAX_SIZE = 10;
    public const float FROZENVOID_MIN_SIZE = 1;
    public const float FROZENVOID_DEFAULT_SIZE = 1;

    public const float SUN_MAX_SIZE = 10;
    public const float SUN_MIN_SIZE = 1;
    public const float SUN_DEFAULT_SIZE = 1;

    public const Bot_Type BOT_DEFAULT_TYPE = Bot_Type.ExpandingBot;
    public static string USER_LEVEL_DIRECTORY_PATH = Application.persistentDataPath;
    public const string USER_LEVEL_DEFAULT_FILE_PATH = "/levels/saved_level.json";
    public const string EDITOR_CURRENT_LEVEL_NAME_PLAYER_PREF = "editor_current_level_name";
    public const int USER_LEVEL_CODE = 999;

    public static string USER_LEVEL_DEFAULT_COMPLETE_PATH = String.Concat(USER_LEVEL_DIRECTORY_PATH, USER_LEVEL_DEFAULT_FILE_PATH);

    public static Dictionary<Team, Color> team_colors = new Dictionary<Team, Color>()
        {
                {Team.Neutral, new Color(0.607f, 1f, 0.662f)}, //Green
                {Team.Player, new Color(1, 0.603f, 0f)}, // Orange
                {Team.CPU1, new Color(1f,1f,1f)}, // Black
                {Team.CPU2, new Color(0.698f, 0.313f, 0.807f)}, // Pink
                {Team.CPU3, new Color(0.039f, 0.168f, 0.454f)}, // Blue
                {Team.CPU4, new Color(0.788f, 0.094f, 0)} // Red
        };
    
    public static Dictionary<Background_Color, string> background_colors = new Dictionary<Background_Color, string>(){
                {Background_Color.aqua, "aqua"},
                {Background_Color.blue, "blue"},
                {Background_Color.red, "red"},
                {Background_Color.default_color, "blue"}
    };

    public static Dictionary<Level_Difficulty, String> level_difficulty_icons = new Dictionary<Level_Difficulty, String>()
        {
                {Level_Difficulty.Easy, "Level_Icons/easy"},
                {Level_Difficulty.Medium, "Level_Icons/medium"},
                {Level_Difficulty.Hard, "Level_Icons/hard"},
                {Level_Difficulty.Impossible, "Level_Icons/impossible"}
        };

    public static Dictionary<Team, string> team_names = new Dictionary<Team, string>()
        {
                {Team.Neutral, "Neutral"},
                {Team.Player, "Player"},
                {Team.CPU1, "CPU 1"},
                {Team.CPU2, "CPU 2"},
                {Team.CPU3, "CPU 3"},
                {Team.CPU4, "CPU 4"}
        };

        public static Dictionary<Bot_Type, string> bot_names = new Dictionary<Bot_Type, string>(){
                {Bot_Type.BlitzBot, "Blitz"},
                {Bot_Type.DefensiveBot, "Defensive" },
                {Bot_Type.ExpandingBot, "Expanding" },
                {Bot_Type.JuggernautBot, "Juggernaut"},
                {Bot_Type.ExpandingJuggernautBot, "ExpJuggernaut"},
                {Bot_Type.EmptyBot, "Empty"},
                {Bot_Type.ProximityBot, "Proximity" },
                {Bot_Type.JuggernautProximityBot, "ProxJuggernaut" }
        };

    public static List<(string, Level_Difficulty)> level_paths = new List<(string, Level_Difficulty)>{
        ("Levels/First_level", Level_Difficulty.Easy),
        ("Levels/level1", Level_Difficulty.Easy),
        ("Levels/Easy_level_3", Level_Difficulty.Easy),
        ("Levels/Circle", Level_Difficulty.Easy),
        ("Levels/Interference", Level_Difficulty.Easy),
        ("Levels/Easy_level_1", Level_Difficulty.Easy),
        ("Levels/Easy_level_5", Level_Difficulty.Easy),
        ("Levels/Easy_level_6", Level_Difficulty.Easy),
        ("Levels/level4", Level_Difficulty.Medium),
        ("Levels/level5", Level_Difficulty.Medium),
        ("Levels/Circle_2", Level_Difficulty.Medium),
        ("Levels/level6", Level_Difficulty.Medium),
        ("Levels/Central_garden", Level_Difficulty.Medium),
        ("Levels/Defense_belt", Level_Difficulty.Medium),
        ("Levels/Satellites", Level_Difficulty.Medium),
        ("Levels/Green_giant", Level_Difficulty.Medium),
        ("Levels/Turret_tutorial_2", Level_Difficulty.Medium),
        ("Levels/level16", Level_Difficulty.Medium),
        ("Levels/Sun_and_moon", Level_Difficulty.Medium),
        ("Levels/level2", Level_Difficulty.Hard),
        ("Levels/Bonanza", Level_Difficulty.Hard),
        ("Levels/level3", Level_Difficulty.Hard),
        ("Levels/level11", Level_Difficulty.Hard),
        ("Levels/Brute_force", Level_Difficulty.Hard),
        ("Levels/Satellite", Level_Difficulty.Hard),
        ("Levels/Breakthrough", Level_Difficulty.Hard),
        ("Levels/level14", Level_Difficulty.Hard),
        ("Levels/Last_stand", Level_Difficulty.Impossible),
        ("Levels/Run_around", Level_Difficulty.Impossible),
        ("Levels/Prison_planet", Level_Difficulty.Impossible),
        ("Levels/Mine_field", Level_Difficulty.Impossible),
        ("Levels/Ancient_defenses", Level_Difficulty.Impossible),   
        ("Levels/Protected", Level_Difficulty.Impossible),
        ("Levels/Hanging_by_a_thread", Level_Difficulty.Impossible),
        ("Levels/Common_enemy", Level_Difficulty.Impossible)
    };

    public static Dictionary<int, Level_Difficulty> level_difficulties = new Dictionary<int, Level_Difficulty>(){
        {1, Level_Difficulty.Easy},
        {2, Level_Difficulty.Easy},
        {3, Level_Difficulty.Easy},
        {4, Level_Difficulty.Easy},
        {5, Level_Difficulty.Medium},
        {6, Level_Difficulty.Medium},
        {7, Level_Difficulty.Medium},
        {8, Level_Difficulty.Medium},
        {9, Level_Difficulty.Medium},
        {10, Level_Difficulty.Medium},
        {11, Level_Difficulty.Hard},
        {12, Level_Difficulty.Hard},
        {13, Level_Difficulty.Hard},
        {14, Level_Difficulty.Impossible},
        {15, Level_Difficulty.Impossible},
        {16, Level_Difficulty.Impossible},
        {17, Level_Difficulty.Impossible}
        };

    public static int states_num = Enum.GetNames(typeof(Selected_State)).Length;

    public static List<String> space_colors = new List<String>(){
        "aqua",
        "blue",
        "red"
    };

    public static Dictionary<Selected_State, float> selected_value = new Dictionary<Selected_State, float>(){
        {Selected_State.Unselected, 0},
        {Selected_State.Half, 0.5f},
        {Selected_State.Full, 1}
        };

    public static Dictionary<Selected_State, Color> selected_color = new Dictionary<Selected_State, Color>(){
        {Selected_State.Unselected, team_colors[Team.Player]},
        {Selected_State.Half, Color.gray},
        {Selected_State.Full, Color.black}
        };

}
