using UnityEditor;
using UnityEngine;

public class CustomLevelEditor : EditorWindow
{
    // The prefab to instantiate
    public GameObject prefab;

    // The size of the grid cells on the X axis
    public float gridSizeX = 0.2f;

    // The size of the grid cells on the Z axis
    public float gridSizeZ = 0.2f;

    // The number of cells on the X axis of the grid
    public int gridCellsX = 20;

    // The number of cells on the Z axis of the grid
    public int gridCellsZ = 20;
    
    // Public field to store the offset value
    public Vector3 offset = new Vector3(-2f,0f,-2f);

    // The instance of the prefab that is being placed
    private GameObject instance;
    
    // Flag to track whether the editor window is focused
    private bool isFocused = false;

    // Add a menu item to create the window
    [MenuItem("Window/Custom Level Editor")]
    public static void ShowWindow()
    {
        GetWindow<CustomLevelEditor>("Custom Level Editor");
    }

    // Override the OnGUI method to customize the window GUI
    private void OnGUI()
    {
        // Add a button to instantiate the prefab
        if (GUILayout.Button("Instantiate Prefab"))
        {
            // Load the prefab from the Assets folder
            GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Cube.prefab", typeof(GameObject));

            // Instantiate the prefab
            instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        }

        // Add fields to adjust the grid size and number of cells
        gridSizeX = EditorGUILayout.FloatField("Grid Size X", gridSizeX);
        gridSizeZ = EditorGUILayout.FloatField("Grid Size Z", gridSizeZ);
        gridCellsX = EditorGUILayout.IntField("Grid Cells X", gridCellsX);
        gridCellsZ = EditorGUILayout.IntField("Grid Cells Z", gridCellsZ);
        offset = EditorGUILayout.Vector3Field("Grid Start Pos", offset);
    }

    // Register the OnSceneGUI method with the SceneView.duringSceneGui event when the editor window is opened
    private void OnFocus()
    {
        isFocused = true;
        SceneView.duringSceneGui += OnSceneGUI;

        gridSizeX = EditorPrefs.GetFloat("CustomLevelEditor_gridSizeX", 0.2f);
        gridSizeZ = EditorPrefs.GetFloat("CustomLevelEditor_gridSizeZ", 0.2f);
        gridCellsX = EditorPrefs.GetInt("CustomLevelEditor_gridCellsX", 20);
        gridCellsZ = EditorPrefs.GetInt("CustomLevelEditor_gridCellsZ", 20);
        offset = StringToVector3(EditorPrefs.GetString("CustomLevelEditor_offset", Vector3ToString(new Vector3(-2f,0f,-2f))));
    }

    // Convert a Vector3 value to a string
    private static string Vector3ToString(Vector3 v)
    {
        return string.Format("{0},{1},{2}", v.x, v.y, v.z);
    }

// Convert a string to a Vector3 value
    private static Vector3 StringToVector3(string s)
    {
        string[] parts = s.Split(',');
        float x = float.Parse(parts[0]);
        float y = float.Parse(parts[1]);
        float z = float.Parse(parts[2]);
        return new Vector3(x, y, z);
    }

    // Unregister the OnSceneGUI method from the SceneView.duringSceneGui event when the editor window is closed
    private void OnLostFocus()
    {
        isFocused = false;
        SceneView.duringSceneGui -= OnSceneGUI;
        // Redraw the scene view
        SceneView.RepaintAll();
        
        // Store the field values in the editor preferences
        EditorPrefs.SetFloat("CustomLevelEditor_gridSizeX", gridSizeX);
        EditorPrefs.SetFloat("CustomLevelEditor_gridSizeZ", gridSizeZ);
        EditorPrefs.SetInt("CustomLevelEditor_gridCellsX", gridCellsX);
        EditorPrefs.SetInt("CustomLevelEditor_gridCellsZ", gridCellsZ);
        EditorPrefs.SetString("CustomLevelEditor_offset", Vector3ToString(offset));
    }

    // Override the OnSceneGUI method to customize the scene GUI
    public void OnSceneGUI(SceneView sceneView)
    {
        if (isFocused)
        {
            // Draw the grid
            for (int x = 0; x < gridCellsX; x++)
            {
                for (int z = 0; z < gridCellsZ; z++)
                {
                    Handles.DrawWireCube(new Vector3(x * gridSizeX + offset.x, offset.y, z * gridSizeZ + offset.z), new Vector3(gridSizeX, 1, gridSizeZ));
                }
            }

            // Follow the mouse position and snap to the grid when the prefab is being placed
            if (instance != null)
            {
                // Get a ray from the mouse position in screen space to the scene view
                Ray mouseRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

                // Get the position of the mouse in world space
                Vector3 mousePosition = mouseRay.origin + mouseRay.direction * 10;

                // Snap the mouse position to the grid
                Vector3 snappedPosition = Handles.SnapValue(mousePosition, new Vector3(gridSizeX*gridCellsX, 0, gridSizeZ*gridCellsZ));

                // Set the position of the prefab instance to the snapped position
                instance.transform.position = snappedPosition;

                // Place the prefab on the grid when left clicking
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                {
                    // Set the prefab as a child of the grid
                    instance.transform.parent = GameObject.Find("Grid").transform;

                    // Clear the instance reference
                    instance = null;
                }
            }
        }
    }
    
    // Override the OnDestroy method to redraw the scene view when the prefab instance is destroyed
    private void OnDestroy()
    {
        if (instance != null)
        {
            instance = null;

            // Redraw the scene view
            SceneView.RepaintAll();
        }
    }
}