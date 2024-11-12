using UnityEngine;

public class TextReader
{
    public static string LoadResourceTextfile(string path)
    {
        // Ajouter
        path = Application.dataPath + "/StreamingAssets/" + path.TrimStart('/');
        string filePath = path.Replace(".json", "");

        // Lire dans le dossier Resources
        TextAsset targetFile = Resources.Load<TextAsset>(filePath);
        return System.IO.File.ReadAllText(path);
    }

    public static string LoadResourceTextfileFromStreamingAsset(string path)
    {
        path = System.IO.Path.Combine(Application.streamingAssetsPath, path);
        return LoadResourceTextfile(path);
    }
}



