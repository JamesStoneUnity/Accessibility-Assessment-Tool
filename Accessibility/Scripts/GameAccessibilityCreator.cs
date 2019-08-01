using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

// made during Unity Hackweek 2019 by James Stone - @epicnerdrage

public class GameAccessibilityCreator : ScriptableObject
{
    public GameAccessibilityData accessibilityData;
}

#if UNITY_EDITOR

public class CreateData : MonoBehaviour
{
    [MenuItem("Accessibility/Create Accessibility Data")]
    static void CreateNewData()
    {
        var scriptableObject = ScriptableObject.CreateInstance<GameAccessibilityCreator>();
        if(!Directory.Exists("Assets/Plugins/Accessibility"))
            Directory.CreateDirectory("Assets/Plugins/Accessibility");
        if(!File.Exists("Assets/Plugins/Accessibility/AccessibilityData.asset"))
            AssetDatabase.CreateAsset(scriptableObject, "Assets/Plugins/Accessibility/AccessibilityData.asset");
        else
            Debug.LogError("Accessibility Data already exists");

        scriptableObject.accessibilityData = JsonUtility.FromJson<GameAccessibilityData>(File.ReadAllText("Assets/Plugins/Accessibility/data.json"));
    }
}

#endif
