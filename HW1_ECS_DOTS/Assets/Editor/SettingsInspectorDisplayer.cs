using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(Settings))]
public class SettingsInspectorDisplayer : Editor
{
    private SerializedProperty _MaxHealth; //reference to our field
    private bool setHealth;
    private void OnEnable()
    {
        _MaxHealth = serializedObject.FindProperty("characterMaxHealth");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(_MaxHealth);

        GUILayout.Label(_MaxHealth.intValue.ToString());

        setHealth = GUILayout.Button("Reset MaxHealth value");
        if (setHealth)
        {
            _MaxHealth.intValue = 150;
        }
        serializedObject.ApplyModifiedProperties();
    }

    public void OnSceneGUI()
    {
       // Handles.Button();
    }
}
