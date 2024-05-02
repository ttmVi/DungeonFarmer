using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Items))]
public class ItemsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        serializedObject.Update();

        Items script = (Items)target;

        // Dropdown for selecting the configuration type
        script.itemType = (Items.ItemType)EditorGUILayout.EnumPopup("Item Type", script.itemType);

        // Conditional display based on the selected configuration type
        switch (script.itemType)
        {
            case Items.ItemType.General:
                break;
            case Items.ItemType.Tree:
                script.maxGrowthIndex = EditorGUILayout.FloatField("Max Growth Index", script.maxGrowthIndex);
                //script.phasesGrowthIndex = EditorGUILayout.("Option", script.phasesGrowthIndex);
                //script.advancedToggle = EditorGUILayout.Toggle("Enable Feature", script.advancedToggle);
                break;
        }

        //serializedObject.ApplyModifiedProperties();
    }
}
