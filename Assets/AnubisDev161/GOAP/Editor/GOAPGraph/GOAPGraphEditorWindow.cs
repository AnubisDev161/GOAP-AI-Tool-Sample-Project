using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace GOAP.GOAPGraph.Editor
{
    public class GOAPGraphEditorWindow : EditorWindow
    {
        [field: SerializeField]
        public GOAPGraphAsset currentGraph {  get; private set; }

        private DragAndDropVisualMode dragAndDropMode = DragAndDropVisualMode.Generic;
        public DragAndDropVisualMode dragAndDropVisualMode => dragAndDropMode;

        [SerializeField]
        private SerializedObject serializedObject;

        [SerializeField]
        private GOAPGraphView currentView;

        private void OnEnable()
        {
            if (currentGraph != null)
            {
                DrawGraph();
            }
        }

        public static void Open(GOAPGraphAsset graphAssetToOpen)
        {
            GOAPGraphEditorWindow[] windows = Resources.FindObjectsOfTypeAll<GOAPGraphEditorWindow>();
            foreach (var win in windows)
            {
                if (win.currentGraph == graphAssetToOpen)
                {
                    win.Focus();
                    return;
                }
            }

            GOAPGraphEditorWindow window = CreateWindow<GOAPGraphEditorWindow>(typeof(GOAPGraphEditorWindow), typeof(SceneView));
            window.titleContent = new GUIContent($"{graphAssetToOpen.name}", EditorGUIUtility.ObjectContent(null, typeof(GOAPGraphAsset)).image);
            window.Load(graphAssetToOpen);
        }

        private void Load(GOAPGraphAsset graphAssetToOpen)
        {
            currentGraph = graphAssetToOpen;
            DrawGraph();
            
        }

        private void DrawGraph()
        {
            serializedObject = new SerializedObject(currentGraph);
            currentView = new GOAPGraphView(serializedObject, this);
            currentView.graphViewChanged += OnChange;
            rootVisualElement.Add(currentView);
        }

        private GraphViewChange OnChange(GraphViewChange graphViewChange)
        {
            EditorUtility.SetDirty(currentGraph);
            return graphViewChange;
        }

        //[MenuItem("Window/AI/GOAP")]
        private void OnGUI()
        {
            GUILayout.Label("GOAP GRAPH", EditorStyles.boldLabel);
           
            if (currentGraph == null) return;
            if (EditorUtility.IsDirty(currentGraph))
            {
                this.hasUnsavedChanges = true;
            }
            else
            {
                this.hasUnsavedChanges = false;
            }
        }
    }
}