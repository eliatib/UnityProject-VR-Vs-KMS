using Project.Utility;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DataManager : Singleton<DataManager>
{
    public InputField filePath;
    public MapData data;

    // Update is called once per frame
    public void Export()
    {
        Save();
    }

    public void Save() 
    {
        try
        {
            string json = "";
            json = JsonUtility.ToJson(data);
            WriteToFile(filePath.text, json);

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        
    }

    public void Load() 
    {
        string json = ReadFromFile(filePath.text);
        JsonUtility.FromJsonOverwrite(json, data);
    }

    private void WriteToFile(string fileName,string json) 
    {
        string path = GetFilePath(fileName);
        File.WriteAllText(path, json);
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
