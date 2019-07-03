using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.Profiling;
using UnityEngine.Serialization;

// made during Unity Hackweek 2019 by James Stone - @epicnerdrage

public class AccessibilityEditorWindow : EditorWindow
{
    public bool groupEnabled;
    public static GameAccessibilityCreator gameAccessibilityData;
    private Vector2 _scrollPos;

    bool highContrastMode = true;
    
    bool bBasicToggled = false;
    bool bIntermediateToggled = false;
    bool bAdvancedToggled = false;
    
    bool lBasic = true;
    bool lIntermediate = true;
    bool lAdvanced = true;
    
    bool lBasicMotor = false;
    bool lBasicCognative = false;
    bool lBasicVision = false;
    bool lBasicHearing = false;
    bool lBasicSpeech = false;
    bool lBasicGeneral = false;
    
    bool lIntermediateMotor = false;
    bool lIntermediateCognative = false;
    bool lIntermediateVision = false;
    bool lIntermediateHearing = false;
    bool lIntermediateSpeech = false;
    bool lIntermediateGeneral = false;
    
    bool lAdvancedMotor = false;
    bool lAdvancedCognative = false;
    bool lAdvancedVision = false;
    bool lAdvancedHearing = false;
    bool lAdvancedSpeech = false;
    bool lAdvancedGeneral = false;

    [MenuItem("Accessibility/Open Accessibility Window")]
    static void Init()
    {
        AccessibilityEditorWindow window = (AccessibilityEditorWindow)EditorWindow.GetWindow(typeof(AccessibilityEditorWindow));
        window.Show();
    }
    
    void OnGUI()
    {
        Color orange = new Color32(255,165,0,255);
        
        GUIStyle typeStyle = new GUIStyle();
        typeStyle.fontSize = 14;
        typeStyle.fontStyle = FontStyle.Bold;
        typeStyle.margin.left = 5;
        typeStyle.normal.textColor = Color.white;
        
        GUIStyle typeStyleBasic = new GUIStyle();
        typeStyleBasic.fontSize = 12;
        typeStyleBasic.fontStyle = FontStyle.Bold;
        typeStyleBasic.margin.left = 5;
        
        GUIStyle typeStyleIntermediate = new GUIStyle();
        typeStyleIntermediate.fontSize = 12;
        typeStyleIntermediate.fontStyle = FontStyle.Bold;
        typeStyleIntermediate.margin.left = 5;
        
        GUIStyle typeStyleAdvanced = new GUIStyle();
        typeStyleAdvanced.fontSize = 12;
        typeStyleAdvanced.fontStyle = FontStyle.Bold;
        typeStyleAdvanced.margin.left = 5;
        
        GUIStyle labelStyle = new GUIStyle();
        labelStyle.fontSize = 24;
        labelStyle.fontStyle = FontStyle.Bold;
        labelStyle.normal.textColor = orange;
        labelStyle.margin.left = 5;
        
        GUIStyle errorStyle = new GUIStyle(EditorStyles.label);
        errorStyle.fontSize = 12;
        errorStyle.fontStyle = FontStyle.Bold;
        errorStyle.normal.textColor = Color.red;
        errorStyle.margin.left = 5;
        
        GUIStyle labelFoldoutHeader = new GUIStyle(EditorStyles.foldout);
        labelFoldoutHeader.fontStyle = FontStyle.Bold;
        labelFoldoutHeader.fontSize = 16;
        labelFoldoutHeader.alignment = TextAnchor.LowerLeft;
        labelFoldoutHeader.normal.textColor = orange;
        labelFoldoutHeader.onNormal.textColor = orange;
        labelFoldoutHeader.hover.textColor = orange;
        labelFoldoutHeader.onHover.textColor = orange;
        labelFoldoutHeader.focused.textColor = orange;
        labelFoldoutHeader.onFocused.textColor = orange;
        labelFoldoutHeader.active.textColor = orange;
        labelFoldoutHeader.onActive.textColor = orange;

        GUIStyle labelFoldoutSub = new GUIStyle(EditorStyles.foldout);
        labelFoldoutSub.fontStyle = FontStyle.Bold;
        labelFoldoutSub.fontSize = 12;
        labelFoldoutSub.margin = new RectOffset(16,0,0,0);
        labelFoldoutSub.alignment = TextAnchor.LowerLeft;
        labelFoldoutSub.normal.textColor = Color.white;
        labelFoldoutSub.onNormal.textColor = Color.white;
        labelFoldoutSub.hover.textColor = Color.white;
        labelFoldoutSub.onHover.textColor = Color.white;
        labelFoldoutSub.focused.textColor = Color.white;
        labelFoldoutSub.onFocused.textColor = Color.white;
        labelFoldoutSub.active.textColor = Color.white;
        labelFoldoutSub.onActive.textColor = Color.white;

        GUIStyle headerStyle = new GUIStyle();
        headerStyle.fontSize = 24;
        headerStyle.fontStyle = FontStyle.Bold;
        headerStyle.normal.textColor = Color.white;
        headerStyle.margin.left = 5;
        
        GUIStyle bExpand = new GUIStyle(EditorStyles.miniButton);
        bExpand.fixedWidth = 150;

        if (!highContrastMode)
        {
            headerStyle.normal.textColor = Color.white;
            typeStyle.normal.textColor = Color.white;
            typeStyleBasic.normal.textColor = Color.white;
            typeStyleIntermediate.normal.textColor = Color.white;
            typeStyleAdvanced.normal.textColor = Color.white;
            labelStyle.normal.textColor = Color.white;
            errorStyle.normal.textColor = Color.white;
            labelFoldoutHeader.normal.textColor = Color.white;
            labelFoldoutHeader.onNormal.textColor = Color.white;
            labelFoldoutHeader.hover.textColor = Color.white;
            labelFoldoutHeader.onHover.textColor = Color.white;
            labelFoldoutHeader.focused.textColor = Color.white;
            labelFoldoutHeader.onFocused.textColor = Color.white;
            labelFoldoutHeader.active.textColor = Color.white;
            labelFoldoutHeader.onActive.textColor = Color.white;
            labelFoldoutSub.normal.textColor = Color.white;
            headerStyle.normal.textColor = Color.white;
        }

        gameAccessibilityData = (GameAccessibilityCreator)EditorGUILayout.ObjectField(gameAccessibilityData, typeof(GameAccessibilityCreator), true);
        if(File.Exists("Assets/Plugins/Accessibility/AccessibilityData.asset"))
            gameAccessibilityData = (GameAccessibilityCreator)AssetDatabase.LoadAssetAtPath("Assets/Plugins/Accessibility/AccessibilityData.asset", typeof(GameAccessibilityCreator));
        groupEnabled = gameAccessibilityData != null;
        
        GUILayout.Label("Accessibility Assessment Tool",headerStyle);
        
        if (GUILayout.Button("http://gameaccessibilityguidelines.com", GUI.skin.label))
        {
            Application.OpenURL("http://gameaccessibilityguidelines.com");
        }
        
        GUIStyle bHighContrast = new GUIStyle(EditorStyles.toolbarButton);
        bHighContrast.normal.textColor = Color.white;

        if (GUILayout.Button("Toggle high contrast B&W mode", bHighContrast))
        {
            highContrastMode = !highContrastMode;
        }

        if (groupEnabled)
        {
            Vector3 pBasic = CalculateBasicPercentages(gameAccessibilityData.accessibilityData.basicGoals);
            Vector3 pIntermediate = CalculateBasicPercentages(gameAccessibilityData.accessibilityData.intermediateGoals);
            Vector3 pAdvanced = CalculateBasicPercentages(gameAccessibilityData.accessibilityData.advancedGoals);

            if (highContrastMode)
            {
                Color cBasic = Color.Lerp(Color.red, Color.green, pBasic.x /100);
                Color cIntermediate = Color.Lerp(Color.red, Color.green, pIntermediate.x/100);
                Color cAdvanced = Color.Lerp(Color.red, Color.green, pAdvanced.x/100);
                
                typeStyleBasic.normal.textColor = cBasic;
                typeStyleIntermediate.normal.textColor = cIntermediate;
                typeStyleAdvanced.normal.textColor = cAdvanced;
            }
            else
            {
                typeStyleBasic.normal.textColor = Color.white;
                typeStyleIntermediate.normal.textColor = Color.white;
                typeStyleAdvanced.normal.textColor = Color.white;
            }

            EditorGUILayout.BeginVertical();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if(pBasic.x <100)
                GUILayout.Label("Basic Goals - "+pBasic.x+"%" +" ("+pBasic.y+"/"+pBasic.z+")", typeStyleBasic);
            else
                GUILayout.Label("Basic Goals - Done!", typeStyleBasic);
            
            if(pIntermediate.x < 100)
                GUILayout.Label("Intermediate Goals - "+pIntermediate.x+"%"+" ("+pIntermediate.y+"/"+pIntermediate.z+")", typeStyleIntermediate);
            else
                GUILayout.Label("Intermediate Goals - Done!", typeStyleIntermediate);
            
            if(pAdvanced.x<100)
                GUILayout.Label("Advanced Goals - "+pAdvanced.x+"%"+" ("+pAdvanced.y+"/"+pAdvanced.z+")", typeStyleAdvanced);
            else
                GUILayout.Label("Advanced Goals - Done!", typeStyleAdvanced);
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
            EditorGUILayout.BeginHorizontal();
            lBasic = EditorGUILayout.Foldout(lBasic, "Basic Settings ("+CountTotals(gameAccessibilityData.accessibilityData.basicGoals).y+"/"+CountTotals(gameAccessibilityData.accessibilityData.basicGoals).x+")",true, labelFoldoutHeader);

            if (bBasicToggled)
            {
                if (GUILayout.Button("Collapse All",bExpand))
                {
                    lBasicMotor = false;
                    lBasicCognative = false;
                    lBasicVision = false;
                    lBasicHearing = false;
                    lBasicSpeech = false;
                    lBasicGeneral = false;
                    bBasicToggled = false;
                }
            }
                else
                {
                    if (GUILayout.Button("Expand All",bExpand))
                    {
                        lBasic = true;
                        lBasicMotor = true;
                        lBasicCognative = true;
                        lBasicVision = true;
                        lBasicHearing = true;
                        lBasicSpeech = true;
                        lBasicGeneral = true;
                        bBasicToggled = true;
                    }
                }
            
            EditorGUILayout.EndHorizontal();
            if (lBasic)
            {
                lBasicMotor = EditorGUILayout.Foldout(lBasicMotor, "Motor" +" ("+CountTotalsByType(gameAccessibilityData.accessibilityData.basicGoals, AccessibilityGoal.GoalType.Motor).z +"/" +CountTotalsByType(gameAccessibilityData.accessibilityData.basicGoals, AccessibilityGoal.GoalType.Motor).y +")",true, labelFoldoutSub);
                if (lBasicMotor)
                    DisplayGoals(gameAccessibilityData.accessibilityData.basicGoals, AccessibilityGoal.GoalType.Motor);
                lBasicCognative = EditorGUILayout.Foldout(lBasicCognative, "Cognitive"+" ("+CountTotalsByType(gameAccessibilityData.accessibilityData.basicGoals, AccessibilityGoal.GoalType.Cognitive).z +"/" +CountTotalsByType(gameAccessibilityData.accessibilityData.basicGoals, AccessibilityGoal.GoalType.Cognitive).y +")",true, labelFoldoutSub);
                if (lBasicCognative)
                    DisplayGoals(gameAccessibilityData.accessibilityData.basicGoals, AccessibilityGoal.GoalType.Cognitive);
                lBasicVision = EditorGUILayout.Foldout(lBasicVision, "Vision"+" ("+CountTotalsByType(gameAccessibilityData.accessibilityData.basicGoals, AccessibilityGoal.GoalType.Vision).z +"/" +CountTotalsByType(gameAccessibilityData.accessibilityData.basicGoals, AccessibilityGoal.GoalType.Vision).y +")",true, labelFoldoutSub);
                if (lBasicVision)
                    DisplayGoals(gameAccessibilityData.accessibilityData.basicGoals, AccessibilityGoal.GoalType.Vision);
                lBasicHearing = EditorGUILayout.Foldout(lBasicHearing, "Hearing"+" ("+CountTotalsByType(gameAccessibilityData.accessibilityData.basicGoals, AccessibilityGoal.GoalType.Hearing).z +"/" +CountTotalsByType(gameAccessibilityData.accessibilityData.basicGoals, AccessibilityGoal.GoalType.Hearing).y +")",true, labelFoldoutSub);
                if (lBasicHearing)
                    DisplayGoals(gameAccessibilityData.accessibilityData.basicGoals, AccessibilityGoal.GoalType.Hearing);
                lBasicSpeech = EditorGUILayout.Foldout(lBasicSpeech, "Speech"+" ("+CountTotalsByType(gameAccessibilityData.accessibilityData.basicGoals, AccessibilityGoal.GoalType.Speech).z +"/" +CountTotalsByType(gameAccessibilityData.accessibilityData.basicGoals, AccessibilityGoal.GoalType.Speech).y +")",true, labelFoldoutSub);
                if (lBasicSpeech)
                    DisplayGoals(gameAccessibilityData.accessibilityData.basicGoals, AccessibilityGoal.GoalType.Speech);
                lBasicGeneral = EditorGUILayout.Foldout(lBasicGeneral, "General"+" ("+CountTotalsByType(gameAccessibilityData.accessibilityData.basicGoals, AccessibilityGoal.GoalType.General).z +"/" +CountTotalsByType(gameAccessibilityData.accessibilityData.basicGoals, AccessibilityGoal.GoalType.General).y +")",true, labelFoldoutSub);
                if (lBasicGeneral)
                    DisplayGoals(gameAccessibilityData.accessibilityData.basicGoals, AccessibilityGoal.GoalType.General);
            }
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.BeginHorizontal();
            lIntermediate = EditorGUILayout.Foldout(lIntermediate, "Intermediate Settings ("+CountTotals(gameAccessibilityData.accessibilityData.intermediateGoals).y+"/"+CountTotals(gameAccessibilityData.accessibilityData.intermediateGoals).x+")",true, labelFoldoutHeader);
            if (bIntermediateToggled)
            {
                if (GUILayout.Button("Collapse All",bExpand))
                {
                    lIntermediateMotor = false;
                    lIntermediateCognative = false;
                    lIntermediateVision = false;
                    lIntermediateHearing = false;
                    lIntermediateSpeech = false;
                    lIntermediateGeneral = false;
                    bIntermediateToggled = false;
                }
            }
            else
            {
                if (GUILayout.Button("Expand All",bExpand))
                {
                    lIntermediate = true;
                    lIntermediateMotor = true;
                    lIntermediateCognative = true;
                    lIntermediateVision = true;
                    lIntermediateHearing = true;
                    lIntermediateSpeech = true;
                    lIntermediateGeneral = true;
                    bIntermediateToggled = true;
                }
            }
            EditorGUILayout.EndHorizontal();
            if (lIntermediate)
            {
                lIntermediateMotor = EditorGUILayout.Foldout(lIntermediateMotor, "Motor" +" ("+CountTotalsByType(gameAccessibilityData.accessibilityData.intermediateGoals, AccessibilityGoal.GoalType.Motor).z +"/" +CountTotalsByType(gameAccessibilityData.accessibilityData.intermediateGoals, AccessibilityGoal.GoalType.Motor).y +")",true, labelFoldoutSub);
                if (lIntermediateMotor)
                    DisplayGoals(gameAccessibilityData.accessibilityData.intermediateGoals,AccessibilityGoal.GoalType.Motor);
                lIntermediateCognative = EditorGUILayout.Foldout(lIntermediateCognative, "Cognitive" +" ("+CountTotalsByType(gameAccessibilityData.accessibilityData.intermediateGoals, AccessibilityGoal.GoalType.Cognitive).z +"/" +CountTotalsByType(gameAccessibilityData.accessibilityData.intermediateGoals, AccessibilityGoal.GoalType.Cognitive).y +")",true, labelFoldoutSub);
                if (lIntermediateCognative)
                    DisplayGoals(gameAccessibilityData.accessibilityData.intermediateGoals,AccessibilityGoal.GoalType.Cognitive);
                lIntermediateVision = EditorGUILayout.Foldout(lIntermediateVision, "Vision"  +" ("+CountTotalsByType(gameAccessibilityData.accessibilityData.intermediateGoals, AccessibilityGoal.GoalType.Vision).z +"/" +CountTotalsByType(gameAccessibilityData.accessibilityData.intermediateGoals, AccessibilityGoal.GoalType.Vision).y +")",true, labelFoldoutSub);
                if (lIntermediateVision)
                    DisplayGoals(gameAccessibilityData.accessibilityData.intermediateGoals,AccessibilityGoal.GoalType.Vision);
                lIntermediateHearing = EditorGUILayout.Foldout(lIntermediateHearing, "Hearing" +" ("+CountTotalsByType(gameAccessibilityData.accessibilityData.intermediateGoals, AccessibilityGoal.GoalType.Hearing).z +"/" +CountTotalsByType(gameAccessibilityData.accessibilityData.intermediateGoals, AccessibilityGoal.GoalType.Hearing).y +")",true, labelFoldoutSub);
                if (lIntermediateHearing)
                    DisplayGoals(gameAccessibilityData.accessibilityData.intermediateGoals,AccessibilityGoal.GoalType.Hearing);
                lIntermediateSpeech = EditorGUILayout.Foldout(lIntermediateSpeech, "Speech" +" ("+CountTotalsByType(gameAccessibilityData.accessibilityData.intermediateGoals, AccessibilityGoal.GoalType.Speech).z +"/" +CountTotalsByType(gameAccessibilityData.accessibilityData.intermediateGoals, AccessibilityGoal.GoalType.Speech).y +")",true, labelFoldoutSub);
                if (lIntermediateSpeech)
                    DisplayGoals(gameAccessibilityData.accessibilityData.intermediateGoals,AccessibilityGoal.GoalType.Speech);
                lIntermediateGeneral = EditorGUILayout.Foldout(lIntermediateGeneral, "General" +" ("+CountTotalsByType(gameAccessibilityData.accessibilityData.intermediateGoals, AccessibilityGoal.GoalType.General).z +"/" +CountTotalsByType(gameAccessibilityData.accessibilityData.intermediateGoals, AccessibilityGoal.GoalType.General).y +")",true, labelFoldoutSub);
                if (lIntermediateGeneral)
                    DisplayGoals(gameAccessibilityData.accessibilityData.intermediateGoals,AccessibilityGoal.GoalType.General);
            }
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.BeginHorizontal();
            lAdvanced = EditorGUILayout.Foldout(lAdvanced, "Advanced Settings ("+CountTotals(gameAccessibilityData.accessibilityData.advancedGoals).y+"/"+CountTotals(gameAccessibilityData.accessibilityData.advancedGoals).x+")",true, labelFoldoutHeader);
            if (bAdvancedToggled)
            {
                if (GUILayout.Button("Collapse All",bExpand))
                {
                    lAdvancedMotor = false;
                    lAdvancedCognative = false;
                    lAdvancedVision = false;
                    lAdvancedHearing = false;
                    lAdvancedSpeech = false;
                    lAdvancedGeneral = false;
                    bAdvancedToggled = false;
                }
            }
            else
            {
                if (GUILayout.Button("Expand All",bExpand))
                {
                    lAdvanced = true;
                    lAdvancedMotor = true;
                    lAdvancedCognative = true;
                    lAdvancedVision = true;
                    lAdvancedHearing = true;
                    lAdvancedSpeech = true;
                    lAdvancedGeneral = true;
                    bAdvancedToggled = true;
                }
            }
            EditorGUILayout.EndHorizontal();
            if (lAdvanced)
            {
                lAdvancedMotor = EditorGUILayout.Foldout(lAdvancedMotor, "Motor"+" ("+CountTotalsByType(gameAccessibilityData.accessibilityData.advancedGoals, AccessibilityGoal.GoalType.Motor).z +"/" +CountTotalsByType(gameAccessibilityData.accessibilityData.advancedGoals, AccessibilityGoal.GoalType.Motor).y +")",true, labelFoldoutSub);
                if (lAdvancedMotor)
                    DisplayGoals(gameAccessibilityData.accessibilityData.advancedGoals, AccessibilityGoal.GoalType.Motor);
                lAdvancedCognative = EditorGUILayout.Foldout(lAdvancedCognative, "Cognitive"+" ("+CountTotalsByType(gameAccessibilityData.accessibilityData.advancedGoals, AccessibilityGoal.GoalType.Cognitive).z +"/" +CountTotalsByType(gameAccessibilityData.accessibilityData.advancedGoals, AccessibilityGoal.GoalType.Cognitive).y +")",true, labelFoldoutSub);
                if (lAdvancedCognative)
                    DisplayGoals(gameAccessibilityData.accessibilityData.advancedGoals, AccessibilityGoal.GoalType.Cognitive);
                lAdvancedVision = EditorGUILayout.Foldout(lAdvancedVision, "Vision"+" ("+CountTotalsByType(gameAccessibilityData.accessibilityData.advancedGoals, AccessibilityGoal.GoalType.Vision).z +"/" +CountTotalsByType(gameAccessibilityData.accessibilityData.advancedGoals, AccessibilityGoal.GoalType.Vision).y +")",true, labelFoldoutSub);
                if (lAdvancedVision)
                    DisplayGoals(gameAccessibilityData.accessibilityData.advancedGoals, AccessibilityGoal.GoalType.Vision);
                lAdvancedHearing = EditorGUILayout.Foldout(lAdvancedHearing, "Hearing"+" ("+CountTotalsByType(gameAccessibilityData.accessibilityData.advancedGoals, AccessibilityGoal.GoalType.Hearing).z +"/" +CountTotalsByType(gameAccessibilityData.accessibilityData.advancedGoals, AccessibilityGoal.GoalType.Hearing).y +")",true, labelFoldoutSub);
                if (lAdvancedHearing)
                    DisplayGoals(gameAccessibilityData.accessibilityData.advancedGoals, AccessibilityGoal.GoalType.Hearing);
                lAdvancedSpeech = EditorGUILayout.Foldout(lAdvancedSpeech, "Speech"+" ("+CountTotalsByType(gameAccessibilityData.accessibilityData.advancedGoals, AccessibilityGoal.GoalType.Speech).z +"/" +CountTotalsByType(gameAccessibilityData.accessibilityData.advancedGoals, AccessibilityGoal.GoalType.Speech).y +")",true, labelFoldoutSub);
                if (lAdvancedSpeech)
                    DisplayGoals(gameAccessibilityData.accessibilityData.advancedGoals, AccessibilityGoal.GoalType.Speech);
                lAdvancedGeneral = EditorGUILayout.Foldout(lAdvancedGeneral, "General"+" ("+CountTotalsByType(gameAccessibilityData.accessibilityData.advancedGoals, AccessibilityGoal.GoalType.General).z +"/" +CountTotalsByType(gameAccessibilityData.accessibilityData.advancedGoals, AccessibilityGoal.GoalType.General).y +")",true, labelFoldoutSub);
                if (lAdvancedGeneral)
                    DisplayGoals(gameAccessibilityData.accessibilityData.advancedGoals, AccessibilityGoal.GoalType.General);
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
        else
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Data file is missing, please create the data file in the Accessibility Menu", errorStyle);
            EditorGUILayout.EndVertical();
        }
        
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        if (GUILayout.Button("Created by James Stone - Unity Hackweek 2019 - @EpicNerdRage", GUI.skin.label))
        {
            Application.OpenURL("http://www.twitter.com/EpicNerdRage");
        }
    }
    
    void DisplayGoals(List<AccessibilityGoal> data, AccessibilityGoal.GoalType goalType)
    {
        if (data.Count <= 0) return;
        for (int i = 0; i < data.Count; i++)
        {
            if (data[i].goalType == goalType)
            {
                GUIStyle titleName = new GUIStyle(EditorStyles.label);
                GUIStyle newStyle = new GUIStyle(EditorStyles.toolbarButton);
                
                titleName.normal.textColor = Color.white;
                titleName.alignment = TextAnchor.LowerLeft;
                titleName.fontStyle = FontStyle.Bold;
                titleName.wordWrap = true;

                if(highContrastMode)
                    GUI.backgroundColor = Color.green;
                data[i].isComplete = EditorGUILayout.ToggleLeft("Completed", data[i].isComplete);
                GUI.backgroundColor = Color.gray;
                
                GUILayout.Label(data[i].name, titleName);
                
                if (!data[i].notRelevant)
                {
                    GUILayout.Box(data[i].description, EditorStyles.helpBox);
                    newStyle.normal.textColor = Color.white;
                }

                EditorGUILayout.BeginHorizontal();
                if(highContrastMode)
                    GUI.backgroundColor = Color.green;
                
                if (GUILayout.Button("Learn More...", newStyle))
                {
                    Application.OpenURL(data[i].url);
                }
                GUI.backgroundColor = Color.gray;

                if(highContrastMode)
                    GUI.backgroundColor = Color.red;
                data[i].notRelevant = EditorGUILayout.Toggle("Not Relevant for this game ",data[i].notRelevant);
                GUI.backgroundColor = Color.gray;
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }
        }
    }

    Vector3 CalculateBasicPercentages(List<AccessibilityGoal> data)
    {
        float countComplete = 0;
        float totalCount = 0;
        if (data.Count > 0)
        {
            for (int i = 0; i < data.Count; i++)
            {
                totalCount++;
                if (data[i].isComplete || data[i].notRelevant)
                    countComplete++;
            }

            var percentageComplete = Mathf.FloorToInt((Mathf.Abs(countComplete / totalCount) * 100));
            return new Vector3(percentageComplete,countComplete,totalCount);
        }
        else
        {
            return Vector3.zero;
        }
    }

    Vector3 CountTotalsByType(List<AccessibilityGoal> data, AccessibilityGoal.GoalType type)
    {
        Vector3 output = new Vector3();
        for (int i = 0; i < data.Count; i++)
        {
            output.x++;
            if (data[i].goalType == type)
            {
                output.y++;
                if (data[i].isComplete || data[i].notRelevant)
                    output.z++;
            }
        }
        return output;
    }
    
    Vector2 CountTotals(List<AccessibilityGoal> data)
    {
        Vector2 output = new Vector3();
        for (int i = 0; i < data.Count; i++)
        {
            output.x++;
            if (data[i].isComplete || data[i].notRelevant)
                output.y++;
        }
        return output;
    }

    [MenuItem("Accessibility/Reset Accessibility Data")]
    static void RestoreData()
    {
        gameAccessibilityData.accessibilityData = JsonUtility.FromJson<GameAccessibilityData>(File.ReadAllText("Assets/Plugins/Accessibility/data.json"));
    }
    
    [MenuItem("Accessibility/How to Use")]
    static void ShowHelp()
    {
        EditorUtility.DisplayDialog("Help", "How to use the accessibility tracker: \n \n Simply open the Accessibility menu and open the accessibility window \n \n This window allows you to track each accessibility goal. Please click the learn more buttons in order to read relevant articles and data on that particular goal \n \n As you clear off each goal, you can track your progress to completion \n \n Should any goal not be relevant to your game, simply check the not relevant button and you will remove that from your overall target score \n \n You can reset the data at anytime by selecting Restore Data from the menu \n \n If the data file is accidently deleted, you can create a new one from the menu", "Close");
    }
}

