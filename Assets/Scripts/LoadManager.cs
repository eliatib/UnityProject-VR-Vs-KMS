using Project.Utility;
using System;
using System.IO;
using UnityEngine;

public class LoadManager : Singleton<LoadManager>
{
    public MapData data;
    public String filePath;

    public void Load()
    {
        string json = ReadFromFile(filePath + ".json");
        JsonUtility.FromJsonOverwrite(json, data);
    }

    private string ReadFromFile(string fileName)
    {
        string path = GetFilePath(fileName);
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }
        else
            Debug.LogWarning("File not Found");

        return "";
    }

    private string GetFilePath(string fileName)
    {
        return Application.streamingAssetsPath + "/" + fileName;
    }
}
