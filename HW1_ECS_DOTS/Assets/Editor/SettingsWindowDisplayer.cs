using UnityEditor;
using UnityEngine;

public class SettingsWindowDisplayer : EditorWindow
{
    private string[] settingsList;
    private bool buttonPressed = false;
    // [MenuItem("Examples/GameSettingWindow")]
    [MenuItem("Window/Game Settings Window")]
    public static void ShowWindow()
    {
       EditorWindow.GetWindow(typeof(SettingsWindowDisplayer));
    }

    //similar to Update
    private void OnGUI()
    {
        settingsList = AssetDatabase.FindAssets("t:settings");
        GUILayout.Label("Game Settings", EditorStyles.boldLabel);
        GUILayout.Space(10);
        //GUILayout.Label(settingsList?.Length.ToString(), EditorStyles.label);
        foreach(var file in settingsList)
        {
            GUILayout.Label(AssetDatabase.GUIDToAssetPath(file), EditorStyles.label);
        }

        buttonPressed = GUILayout.Button("Increase Max Health Amount");
        if (buttonPressed)
        {
            foreach (var file in settingsList)
            {
                var settingsFile = AssetDatabase.LoadAssetAtPath<Settings>(AssetDatabase.GUIDToAssetPath(file));
                if (settingsFile != null)
                {
                    settingsFile.characterMaxHealth += 35;
                }
            }
            AssetDatabase.SaveAssets();
        }

    }
}
