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
        if (item.GetItemType() == Items.ItemType.Seed)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Tree Information", EditorStyles.boldLabel);

            item.maxGrowthIndex = EditorGUILayout.FloatField("Max Growth Index", item.maxGrowthIndex);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("phasesGrowthIndex"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("growingPhasesSprites"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("deceasingSprites"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("possibleDrops"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("treeAnimator"), true);
        }
        else if (item.GetItemType() == Items.ItemType.Crop)
        {

        }
        else if (item.GetItemType() == Items.ItemType.Fertilizer)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Fertilizer Information", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("fertilizableSeed"), true);
        }
        else if (item.GetItemType() == Items.ItemType.MonsterPart)
        {

        }
        else if (item.GetItemType() == Items.ItemType.Potion)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Potion Information", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("potionEffect"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("potionEffectDuration"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("potionEffectValue"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("potionEffectEvent"), true);
        }

        if (item.CanBeCrafted())
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Recipe", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("craftingRecipe"), true);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
