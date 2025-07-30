using System.Collections;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class ConfigLoader : MonoBehaviour
{
    [SerializeField] private bool _loadOnEditor = false;

    public ScriptableObject[] scriptableObjects;

    private void Awake()
    {
#if UNITY_EDITOR
        if(_loadOnEditor) StartCoroutine(CoroutineLoadAllJsons());
#else
        StartCoroutine(CoroutineLoadAllJsons());

#endif
    }

    IEnumerator CoroutineLoadAllJsons()
    {
        foreach (var scriptableObject in scriptableObjects)
        {
            yield return StartCoroutine(LoadJsonFile(scriptableObject));
        }

        Debug.Log("All Json Loaded");
    }

    IEnumerator LoadJsonFile(ScriptableObject outSO)
    {
        string jsonData = "";
        string jsonName = outSO.name + ".json";
        string filePath = Path.Combine(Application.streamingAssetsPath, jsonName);

        if (!File.Exists(filePath))
        {
            Debug.LogError("File not found: " + filePath);
            yield break;
        }

        if (filePath.StartsWith("jar") || filePath.StartsWith("http"))
        {
            // Special case to access StreamingAsset content on Android and Web
            UnityWebRequest request = UnityWebRequest.Get(filePath);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                jsonData = request.downloadHandler.text;
            }
        }
        else
        {
            // This is a regular file path on most platforms and in playmode of the editor
            jsonData = System.IO.File.ReadAllText(filePath);
        }

        JsonUtility.FromJsonOverwrite(jsonData, outSO);
        Debug.Log("Json Overwrited: " + jsonName);
    }

    [ContextMenu("Create Streaming Assets Json Files")]
    void CreateStreamingAssetsJsonFiles()
    {
        foreach (var scriptableObject in scriptableObjects)
        {
            SaveScriptableObjectAsJson(scriptableObject);
        }
        Debug.Log(" Streaming Assets Json Files Created");
    }

    private void SaveScriptableObjectAsJson(ScriptableObject so)
    {
        string json = JsonUtility.ToJson(so);
        string jsonName = so.name + ".json";
        string path = Path.Combine(Application.streamingAssetsPath, jsonName);
        File.WriteAllText(path, json);
        Debug.Log("ScriptableObject saved as JSON to StreamingAssets: " + path);
    }
}
