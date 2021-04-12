using System.Collections;
using System.Collections.Generic;
using System;


public static class Save_File_Manager
{
    public static List<string> getSimpleFileNames()
    {
        List<string> simple_file_names = new List<string>();
        string[] NameList = System.IO.Directory.GetFiles(Constants.USER_LEVEL_DIRECTORY_PATH + "/levels/");
		foreach(string str in NameList)
		{
			var cuts = str.Split('/');
			string myStr = cuts[cuts.Length-1];
			if(myStr.Substring(myStr.Length-5, 5).Equals(".json"))
			{
				myStr = myStr.Substring(0,myStr.Length-5);
                simple_file_names.Add(myStr);
			}
        }
        return simple_file_names;
    }
    
    public static void saveFile(string file_name)
    {
        System.IO.FileInfo file;
        if(file_name == null || file_name == "")
        {
            List<string> paths_list = new List<string>(System.IO.Directory.GetFiles(Constants.USER_LEVEL_DIRECTORY_PATH + "/levels/"));
            int postfix  = 1;
            string file_path = Constants.USER_LEVEL_DIRECTORY_PATH + "/levels/saved_level"+postfix.ToString()+".json";
            while(paths_list.Contains(file_path))
            {
                postfix += 1;
                file_path = Constants.USER_LEVEL_DIRECTORY_PATH + "/levels/saved_level"+postfix.ToString()+".json";
            }

            file = new System.IO.FileInfo(file_path);
        }
        else
        {
            file = new System.IO.FileInfo(Constants.USER_LEVEL_DIRECTORY_PATH + "/levels" + file_name + ".json");
        }
    }

    public static string getFullPath(string file_name)
    {
        string level_path;
        if(file_name == "")
        {
            level_path = Constants.USER_LEVEL_DEFAULT_COMPLETE_PATH; 
        }
        else
        {
            level_path = Constants.USER_LEVEL_DIRECTORY_PATH + "/levels/" + file_name + ".json"; 
        }
        return level_path;
    }
}