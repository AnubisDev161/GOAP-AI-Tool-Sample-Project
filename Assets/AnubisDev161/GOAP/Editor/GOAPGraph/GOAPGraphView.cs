using GOAP.Core;
using GOAP.Core.Agent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GOAP.GOAPGraph.Editor
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

            StyleSheet style = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/AnubisDev161/GOAP/Editor/GOAPGraph/USS/GOAPGraphEditor.uss");
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

            // Create blackboard
            AddBlackboard();
            AddWorldFactBlackboard();
        }

        private void AddWorldFactBlackboard()
        {
            Blackboard worldFactsBlackboard = new Blackboard();
            worldFactsBlackboard.graphView = this;
            worldFactsBlackboard.title = "World State";
            worldFactsBlackboard.addItemRequested += OnAddWorldFactRequested;
            this.AddElement(worldFactsBlackboard);

            AddExistingWorldFacts(worldFactsBlackboard);
        }

        private void AddExistingWorldFacts(Blackboard worldFactsBlackboard)
        {
            foreach (var keyValuePair in goapGraph.Blackboard.GetWorldFacts())
            {
                AddBlackboardKey(worldFactsBlackboard, keyValuePair.Key, keyValuePair.Value.keyType, keyValuePair.Value.worldFactType, keyValuePair.Value.isWorldFact);
            }
        }

        private void AddBlackboard()
        {
            Blackboard blackboard = new Blackboard();
            blackboard.graphView = this;
            blackboard.title = "Blackboard";
            blackboard.addItemRequested += OnAddBlackboardKeyRequested;

            var defaultPos = new Rect(0, 400, 200, 400);
            this.AddElement(blackboard);
            blackboard.SetPosition(defaultPos);

            AddExistingBlackboardKeys(blackboard);
        }

        private void AddExistingBlackboardKeys(Blackboard blackboard)
        {
            foreach(var keyValuePair in goapGraph.Blackboard.GetKeys())
            {
                AddBlackboardKey(blackboard, keyValuePair.Key, keyValuePair.Value.keyType, keyValuePair.Value.worldFactType, keyValuePair.Value.isWorldFact);
            }
        }

        private void OnAddWorldFactRequested(Blackboard blackboard)
        {
            AddBlackboardKey(blackboard, isWorldFact: true);
        }

        /// <summary>
        /// If I had had the time I would have improved the perfomance here because it is called for every new letter you insert in the name, not just when pressing enter.
        /// </summary>
        /// <param name="blackboard"></param>
        /// <param name="keyName"></param>
        /// <param name="keyType"></param>
        /// <param name="worldFactType"></param>
        /// <param name="isWorldFact"></param>
        private void AddBlackboardKey(
            Blackboard blackboard, string keyName = "Enter name to save key", GOAPBlackbaord.BlackboardKeyType keyType = GOAPBlackbaord.BlackboardKeyType.Bool,
            WorldFactType worldFactType = WorldFactType.Bool, bool isWorldFact = false)
        {
            EnumField valueTypeField;
            string titleText;
            if (isWorldFact)
            {
                valueTypeField = new EnumField(worldFactType);
                titleText = "World fact";
            }
            else
            {
                valueTypeField = new EnumField(keyType);
                titleText = "Blackbaord Key";
            }

            var blackboardField = new BlackboardField(null, keyName, titleText);

            blackboardField.RegisterCallback<ChangeEvent<string>>(evt =>
            {
                if (evt.previousValue == evt.newValue) return;
                HandleChange(blackboardField, evt, isWorldFact);
            }
            );

            blackboardField.Add(valueTypeField);
            blackboard.Add(blackboardField);
        }

        private void HandleChange(BlackboardField blackboardField, ChangeEvent<string> evt, bool isWorldfact = false)
        {
            // Determine whether it is a name change or a value change
            bool isNameChange;
            if (evt.newValue == blackboardField.text)
            {
                goapGraph.Blackboard.RemoveKey(evt.previousValue);
                isNameChange = true;
            }
            else
            {
                goapGraph.Blackboard.RemoveKey(blackboardField.text);
                isNameChange = false;
            }

           var newKeyName = isNameChange ? evt.newValue : blackboardField.text;

            foreach (var child in blackboardField.Children())
            {
                if (child is EnumField)
                {
                    var enumField = (EnumField)child;

                    if (isWorldfact)
                    {
                        goapGraph.Blackboard.AddKey(newKeyName, (WorldFactType)enumField.value);
                    }
                    else
                    {
                        goapGraph.Blackboard.AddKey(newKeyName, (GOAPBlackbaord.BlackboardKeyType)enumField.value);
                    }
                }
            }

            EditorUtility.SetDirty(goapGraph);
        }

        private void RemoveBlackboardKey(BlackboardField field)
        {
            goapGraph.Blackboard.RemoveKey(field.text);
            EditorUtility.SetDirty(goapGraph);
        }

        private void OnAddBlackboardKeyRequested(Blackboard blackboard)
        {
            AddBlackboardKey(blackboard, isWorldFact: false);
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
                UnityEngine.Debug.Log("Nodes removed: " +  nodesToRemove.Count);

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
                }

                List<BlackboardField> blackboardFieldsToRemove = graphViewChange.elementsToRemove.OfType<BlackboardField>().ToList();

                foreach (var field in blackboardFieldsToRemove)
                {
                    RemoveBlackboardKey(field);
                }
            }

            if (graphViewChange.edgesToCreate != null)
            {
                Undo.RecordObject(serializedObject.targetObject, "Added connections");
                
               foreach (var edge in graphViewChange.edgesToCreate)
               {
                    CreateEdge(edge);
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

        private void Bind()
        {
            serializedObject.Update();
            this.Bind(serializedObject);
        }
    }
}