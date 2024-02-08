using UnityEditor;
using UnityEngine;

public class ReplaceWithPrefab : EditorWindow
{
    // Serialized field to store the selected prefab
    [SerializeField] private GameObject prefab;

    // MenuItem attribute to create the menu item "Tools/Replace With Prefab"
    [MenuItem("Tools/Replace With Prefab")]
    static void CreateReplaceWithPrefab()
    {
        // Open the ReplaceWithPrefab window when the menu item is clicked
        EditorWindow.GetWindow<ReplaceWithPrefab>();
    }

    // GUI function called whenever the editor window is drawn
    private void OnGUI()
    {
        // Create an ObjectField to select the prefab
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);

        // Button to initiate the replacement process
        if (GUILayout.Button("Replace"))
        {
            // Get the currently selected GameObjects in the hierarchy or scene view
            var selection = Selection.gameObjects;

            // Loop through the selected objects in reverse order
            for (var i = selection.Length - 1; i >= 0; --i)
            {
                // Get the current selected GameObject
                var selected = selection[i];

                // Determine the type of the selected prefab
                var prefabAssetType = PrefabUtility.GetPrefabAssetType(prefab);
                var prefabInstanceStatus = PrefabUtility.GetPrefabInstanceStatus(prefab);
                GameObject newObject;

                // Instantiate the prefab based on its type
                if (prefabAssetType == PrefabAssetType.Regular && prefabInstanceStatus == PrefabInstanceStatus.NotAPrefab)
                {
                    // Instantiate a regular prefab and set its name
                    newObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                }
                else
                {
                    newObject = Instantiate(prefab);
                    newObject.name = prefab.name;
                }

                // Check for instantiation errors
                if (newObject == null)
                {
                    Debug.LogError("Error instantiating prefab");
                    break;
                }

                // Register the new object creation for undo functionality
                Undo.RegisterCreatedObjectUndo(newObject, "Replace With Prefabs");

                // Transfer the transform properties from the selected object to the new object
                newObject.transform.parent = selected.transform.parent;
                newObject.transform.localPosition = selected.transform.localPosition;
                newObject.transform.localRotation = selected.transform.localRotation;
                newObject.transform.localScale = selected.transform.localScale;
                newObject.transform.SetSiblingIndex(selected.transform.GetSiblingIndex());

                // Destroy the selected object immediately to complete the replacement
                Undo.DestroyObjectImmediate(selected);
            }
        }

        // Disable the GUI elements (greyed out)
        GUI.enabled = false;

        // Display the count of selected objects
        EditorGUILayout.LabelField("Selection count: " + Selection.objects.Length);
    }
}
