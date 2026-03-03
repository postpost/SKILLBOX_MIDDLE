using System.Linq;
using UnityEditor;


[CanEditMultipleObjects]
[CustomEditor(typeof(SettingsInstaller))]
public class SettingsInspectorDisplayer : Editor
{
    public override void OnInspectorGUI()
    {
        //показывает поля из SettingsInstaller, кроме settings
        serializedObject.Update();
        DrawPropertiesExcluding(serializedObject, "settings");
        
        SettingsInstaller installer = (SettingsInstaller)target;
        SerializedProperty useScriptableObj = serializedObject.FindProperty("useScriptableObject");
        if(useScriptableObj.boolValue)
        {
            var assets = AssetDatabase.FindAssetGUIDs("t:Settings");
            var settings = assets
                        .Select(id =>   AssetDatabase.LoadAssetAtPath<Settings>(AssetDatabase.GUIDToAssetPath(id)))
                        .Where(asset => asset != null)
                        .ToArray();
            if (settings.Length > 0)
            {
                string[] names = settings.Select(s => s.name).ToArray();

                EditorGUI.BeginChangeCheck();

                int currentIndex = System.Array.IndexOf(settings, installer.settings);
                if(currentIndex < 0 ) currentIndex = 0;
                int newIndex = EditorGUILayout.Popup("Select congfig file", currentIndex, names);

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObjects(targets, "Change Installer Settings");
                    foreach(var t in targets)
                    {
                        ((SettingsInstaller)t).settings = settings[newIndex];
                        EditorUtility.SetDirty(t);
                    }
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Cannot find any Settings", MessageType.Info);
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}
