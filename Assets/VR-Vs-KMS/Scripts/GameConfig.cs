using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class GameConfig
{

    private static GameConfig instance = null;


    public static GameConfig Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameConfig();
            }
            return instance;
        }
    }

    private GameConfig() { }



    public int lifeNumber;
    public int delayShoot;
    public int delayTeleport;
    public string colorShotVirus;
    public string colorShotKMS;
    public int nbContaminedPlayerToVictory;
    public int radiusExplosion;
    public int timeToAreaContamination;
    
    private string pathJson = "/gameConfig.json";


    public void OverWriteJson()
    {
        string json = JsonUtility.ToJson(this);
        File.WriteAllText(Application.streamingAssetsPath+pathJson, json);
    }

    public void LoadJson ()
    {
        JsonUtility.FromJsonOverwrite(TextReader.LoadResourceTextfileFromStreamingAsset(pathJson), this);
    }
}