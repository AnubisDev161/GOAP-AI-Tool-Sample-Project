using UnityEngine;
using UnityEditor;

public class TestWindow : EditorWindow
{
    bool mouseClicked;
    readonly Texture texture;
    [MenuItem("Window/AI/Test")]
    public static void ShowWindow()
    {
        GetWindow<TestWindow>("Test");
    }

    GUIContent testLabel = new GUIContent("Test button");
  
    private void OnGUI()
    {
        GUILayout.Label(testLabel);
        GUILayout.Button(texture);
        GUILayout.VerticalScrollbar(5, 100, 30, 0);


        if (UnityEngine.Input.GetMouseButtonDown(0))
        {
            mouseClicked = true;
        }

        if (mouseClicked)
        {
            GUILayout.BeginArea(new Rect(Input.mousePosition.x, Input.mousePosition.y, 100, 100));
            GUILayout.VerticalScrollbar(10, 30, 30, 0);
        }

        
    }
}
