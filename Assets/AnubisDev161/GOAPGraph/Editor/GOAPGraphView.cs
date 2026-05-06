using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GOAPGraph.Editor
{
    public class GOAPGraphView : GraphView
    {
        private GOAPGraphAsset goapGraph;
        private SerializedObject serializedObject;
        private Label logo = new Label("GOAP GRAPH");

        public List<GOAPGraphEditorNode> graphNodes;
        public Dictionary<string, GOAPGraphEditorNode> nodeDictionary;
        public Dictionary<Edge, GOAPGraphConnection> connectionDictionary;
        public GOAPGraphEditorWindow window { get; private set; }
        
        private GOAPGraphWindowSearchProvider searchProvider;
        public GOAPGraphView(SerializedObject serializedObject, GOAPGraphEditorWindow window)
        {
            this.serializedObject = serializedObject;
            goapGraph = (GOAPGraphAsset)serializedObject.targetObject;
            this.window = window;

            graphNodes = new List<GOAPGraphEditorNode>();
            nodeDictionary = new Dictionary<string, GOAPGraphEditorNode>();
            connectionDictionary = new Dictionary<Edge, GOAPGraphConnection>();

            searchProvider = ScriptableObject.CreateInstance<GOAPGraphWindowSearchProvider>();
            searchProvider.graph = this;
            this.nodeCreationRequest = ShowSearchWindow;

            StyleSheet style = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/AnubisDev161/GOAPGraph/Editor/USS/GOAPGraphEditor.uss");
            styleSheets.Add(style);

            GridBackground background = new GridBackground();
            background.name = "Grid";
            Add(background);
            background.SendToBack();

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ClickSelector());
            this.AddManipulator(new ContentZoomer());

            DrawNodes();
            DrawConnections();
          
            graphViewChanged += OnGraphViewChanged;

            logo.style.fontSize = 32;
            Add(logo);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> allPorts = new List<Port>();
            List<Port> ports = new List<Port>();

            foreach (var node in graphNodes)
            {
                allPorts.AddRange(node.ports);
            }
            
            foreach(Port port in allPorts)
            {
                if (port == startPort) continue;
                if (port.node == startPort.node) continue;
                if (port.direction == startPort.direction) continue;    
                if (port.portType == startPort.portType)
                {
                    ports.Add(port);
                }
            }

            return ports;
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.movedElements != null)
            {
                Undo.RecordObject(serializedObject.targetObject, "Moved Nodes");
                foreach (GOAPGraphEditorNode editorNode in graphViewChange.movedElements.OfType<GOAPGraphEditorNode>().ToList())
                {
                    editorNode.SavePosition();
                }  
            }

            if (graphViewChange.elementsToRemove != null)
            {
                Undo.RecordObject(serializedObject.targetObject, "Removed Node");

                List<GOAPGraphEditorNode> nodesToRemove = graphViewChange.elementsToRemove.OfType<GOAPGraphEditorNode>().ToList();
                Debug.Log("Nodes removed: " +  nodesToRemove.Count);

                if (nodesToRemove.Count > 0)
                {
                    for (int i = nodesToRemove.Count -1; i > -1; i--)
                    {
                        RemoveNode(nodesToRemove[i]);
                    }
                }

                foreach (Edge edge in graphViewChange.elementsToRemove.OfType<Edge>())
                {
                    RemoveConnection(edge);

                    if (edge.input.portName == GOAPGraphEditorNode.PARAM_PORT_NAME)
                    {
                        GOAPGraphEditorNode node = (GOAPGraphEditorNode)edge.input.node;
                      //  node.RemoveInputParams();
                    }
                }
            }

            if (graphViewChange.edgesToCreate != null)
            {
                Undo.RecordObject(serializedObject.targetObject, "Added connections");
                
               foreach (var edge in graphViewChange.edgesToCreate)
               {
                    CreateEdge(edge);

                    if (edge.input.portName == GOAPGraphEditorNode.PARAM_PORT_NAME)
                    {
                        GOAPGraphEditorNode node = (GOAPGraphEditorNode)edge.input.node;
                      //  node.ExpandInputParams();
                    }
               }
            }

           
            
            return graphViewChange;
        }

        private void CreateEdge(Edge edge)
        {
            GOAPGraphEditorNode inputNode = (GOAPGraphEditorNode)edge.input.node;
            int inputIndex = inputNode.ports.IndexOf(edge.input);

            GOAPGraphEditorNode outputNode = (GOAPGraphEditorNode)(edge.output.node);
            int outputIndex = outputIndex = outputNode.ports.IndexOf(edge.output);

            GOAPGraphConnection connection = new GOAPGraphConnection(inputNode.graphNode.id, inputIndex, outputNode.graphNode.id, outputIndex);
            goapGraph.Connections.Add(connection);
            connectionDictionary.Add(edge, connection);
        }

        private void RemoveConnection(Edge edge)
        {
            if (connectionDictionary.TryGetValue(edge, out GOAPGraphConnection connection))
            {
                goapGraph.Connections.Remove(connection);
                connectionDictionary.Remove(edge);
            }
        }

        private void RemoveNode(GOAPGraphEditorNode editorNode)
        {
            goapGraph.Nodes.Remove(editorNode.graphNode);
            nodeDictionary.Remove(editorNode.graphNode.id);
            graphNodes.Remove(editorNode);
            serializedObject.Update();
        }

        private void DrawNodes()
        {
            foreach (var node in goapGraph.Nodes)
            {
                AddNodeToGraph(node);
                Bind();
            }
        }

        private void DrawConnections()
        {
            if (goapGraph.Connections == null) return;
            
            foreach (var connection in goapGraph.Connections)
            {
                DrawConnection(connection);
            }
        }

        private void DrawConnection(GOAPGraphConnection connection)
        {
            GOAPGraphEditorNode inputNode = GetNode(connection.inputPort.nodeId);
            GOAPGraphEditorNode outputNode = GetNode(connection.outputPort.nodeId);
            
            if (inputNode == null || outputNode == null) return;

            Port inputPort = inputNode.ports[connection.inputPort.portIndex];
            Port outputPort = outputNode.ports[connection.outputPort.portIndex];
            Edge edge = inputPort.ConnectTo(outputPort);
            AddElement(edge);

            connectionDictionary.Add(edge, connection);
        }

        private GOAPGraphEditorNode GetNode(string nodeId)
        {
            GOAPGraphEditorNode node = null;
            nodeDictionary.TryGetValue(nodeId, out node);
            return node;
        }

        private void ShowSearchWindow(NodeCreationContext context)
        {
            searchProvider.target = (VisualElement)focusController.focusedElement;
            SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchProvider);
        }

        public void Add(GOAPGraphNode node)
        {
            Undo.RecordObject(serializedObject.targetObject, "Added node");

            goapGraph.Nodes.Add(node);
            serializedObject.Update();

            AddNodeToGraph(node);
            Bind();
        }

        private void AddNodeToGraph(GOAPGraphNode node)
        {
            node.typeName = node.GetType().AssemblyQualifiedName;

            GOAPGraphEditorNode editorNode = new GOAPGraphEditorNode(node, serializedObject);
            editorNode.SetPosition(node.position);
            graphNodes.Add(editorNode);
            nodeDictionary.Add(node.id, editorNode);

            AddElement(editorNode);
        }

       
        public void Repaint()
        {
            foreach (var test in contentViewContainer.Children())
            {
                test.MarkDirtyRepaint();
            }
        }

        private void Bind()
        {
            serializedObject.Update();
            this.Bind(serializedObject);
        }
    }
}