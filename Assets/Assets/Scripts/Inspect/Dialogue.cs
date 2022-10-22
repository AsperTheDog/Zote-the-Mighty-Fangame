using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

[System.Serializable]
[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/Create Scriptable Object")]
public class Dialogue : ScriptableObject
{
    public new string name;
    public string subtitle;

    public List<State> states;

    public Dialogue()
    {
        states = new List<State>();
    }

    private int GetStateIdx(string stateName)
    {
        int idx = -1;
        for (int i = 0; i < states.Count; i++)
        {
            if (states[i].name.Equals(stateName))
            {
                idx = i;
                break;
            }
        }

        return idx;
    }


    public string[] GetSentences(string stateName)
    {
        var idx = GetStateIdx(stateName);
        if (idx < 0 || idx >= states.Count)
            return null;

        return states[idx].sentences.ToArray();
    }

#if UNITY_EDITOR
    public void SaveJson()
    {
        string json = JsonUtility.ToJson(this, true);
        var assetPath = AssetDatabase.GetAssetPath(this).Replace(".asset", "");
        string path = EditorUtility.SaveFilePanel("Save dialogue as JSON", assetPath, this.name, "json");
        File.WriteAllText(path, json);
    }


    [MenuItem("Assets/Create/Dialogue/From Json")]
    public static void LoadFromJson()
    {
        
        string pathRead = EditorUtility.OpenFilePanel("Load dialogue from JSON", "", "json");        
        string json = File.ReadAllText(pathRead);
        try
        {
            Dialogue dialogue = ScriptableObject.CreateInstance<Dialogue>();
            JsonUtility.FromJsonOverwrite(json, dialogue);
            string pathWrite = GetSelectedPathOrFallback();
            pathWrite = EditorUtility.SaveFilePanelInProject("Save dialogue object", "Dialogue", "asset", "Message aquiasodi haoisd", pathWrite);

            AssetDatabase.CreateAsset(dialogue, pathWrite);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = dialogue;
        }
        catch (System.ArgumentException e)
        {
            Debug.LogWarning(e);
        }
       
    }

    //https://answers.unity.com/questions/472808/how-to-get-the-current-selected-folder-of-project.html#:~:text=a%2007%3A%2032-,Check%20this%20out,-%3A%20https%3A//gist.github
    private static string GetSelectedPathOrFallback()
    {
        string path = "Assets";

        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
                break;
            }
        }
        return path;
    }
#endif
}