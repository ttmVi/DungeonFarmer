using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Items))]
public class ItemsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw default fields that always appear
        DrawDefaultInspector();

        // Get a reference to the script
        Items item = (Items)target;

        // Conditional display based on ItemType
        if (item.itemType == Items.ItemType.Tree)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Tree Information", EditorStyles.boldLabel);

            // Add fields specific to the tree type
            item.maxGrowthIndex = EditorGUILayout.FloatField("Max Growth Index", item.maxGrowthIndex);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("phasesGrowthIndex"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("growingPhasesSprites"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("deceasingSprites"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("possibleDrops"), true);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
