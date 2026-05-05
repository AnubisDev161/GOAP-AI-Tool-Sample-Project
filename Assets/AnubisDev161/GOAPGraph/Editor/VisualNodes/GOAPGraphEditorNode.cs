using Codice.Client.BaseCommands.BranchExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager.UI;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GOAPGraph.Editor
{
    public class GOAPGraphEditorNode : Node
    {
        public GOAPGraphNode graphNode {  get; private set; }

        private Port outputPort;
        
        private SerializedObject serializedObject;
        private SerializedProperty serializedProperty;

        public static string PARAM_PORT_NAME { get; private set; } = "Required World Fact";
        public List<Port> ports {  get; private set; }
        public List<int> portsIndices { get; private set; } = new List<int>();

        public GOAPGraphEditorNode(GOAPGraphNode node, SerializedObject goapGraphObject)
        {
            this.AddToClassList("GOAP-graph-node");

            serializedObject = goapGraphObject;
            graphNode = node;
            graphNode.valueUpdated += OnGraphNodeValueUpdated;

            Type typeInfo = node.GetType();
            NodeInfoAttribute info = typeInfo.GetCustomAttribute<NodeInfoAttribute>();

            title = info.title;

            ports = new List<Port>();

            string[] depths = info.menuItem.Split("/");
            foreach (string depth in depths)
            {
                this.AddToClassList(depth.ToLower().Replace(" ", "-"));
            }

            name = typeInfo.Name;

            // Flow output is created so that output can be at index 0
            if (info.hasFlowOutput)
            {
                CreateFlowOutputPort();
            }

            if (info.hasFlowInput)
            {
                CreateFlowInputPort();
            }


            if (info.hasInputParams)
            {
                CreateParamInputPort(info.paramPortsHaveSingleCapacity);

            }

            if (info.hasOutputParams)
            {
                CreateParamOutputPort(info.paramPortsHaveSingleCapacity);
            }

            DrawProperties(typeInfo);
            

            RefreshExpandedState();
        }

        private void DrawProperties(Type typeInfo)
        {
            foreach (FieldInfo property in typeInfo.GetFields())
            {
                if (property.GetCustomAttribute<Attribute>() is Attribute exposedProperty)
                {
                    if (exposedProperty is ExposedWorldFactPropertyAttribute)
                    {
                        DrawWorldFactProperty(property.Name);
                        continue;
                    }

                    PropertyField propertyField = DrawProperty(property.Name);
                }
            }
        }

        public void OnWorldFactPropertyChanged(SerializedPropertyChangeEvent evt)
        {

            var valueTypeProperty = evt.changedProperty.FindPropertyRelative("valueType");
            var valueProperty = evt.changedProperty.FindPropertyRelative("value");



            var worldFact = graphNode.GetType().GetProperties();

            var nodeInfo = graphNode.GetType().GetCustomAttribute<NodeInfoAttribute>();

            //if (nodeInfo.title == "Blackbaord Key")
            //{
            //    var blackboardNode = (BlackbaordKeyNode)graphNode;
            //    if (blackboardNode.worldFact.valueType == (ValueType)valueTypeProperty.boxedValue)
            //    {
            //        return;
            //    }
            //}

            object newValue = null;

            switch ((ValueType)valueTypeProperty.boxedValue)
            {
                case ValueType.Bool:
                    newValue = true;
                    break;
                case ValueType.Int:
                    newValue = 0;
                    break;
                case ValueType.Float:
                    newValue = 0.0f;
                    break;
                case ValueType.String:
                    newValue = "Default";
                    break;
            }

            valueProperty.boxedValue = newValue;
            valueProperty.serializedObject.ApplyModifiedProperties();
            valueProperty.serializedObject.Update();

            //evt.changedProperty.serializedObject.ApplyModifiedProperties();
            //evt.changedProperty.serializedObject.Update();
        }

        private void OnFieldChangedCallback(SerializedPropertyChangeEvent evt)
        {
            graphNode.OnWorldFactPropertyChanged(evt);
        }

        public void RemoveInputParams()
        {
            //RemoveParamInputPort();

            
            RefreshExpandedState();
        }

        public void ExpandInputParams()
        {
            //CreateParamInputPort();

            RefreshExpandedState();
        }

        private void RemoveParamInputPort()
        {
            if (ports.Count > 1)
            {
                inputContainer.Remove(ports[ports.Count -1]);
                ports.RemoveAt(ports.Count -1);
            }
        }
      
        private void FetchSerializedProperty()
        {
            SerializedProperty nodes = serializedObject.FindProperty("nodes");
            if (nodes.isArray)
            {
                int size = nodes.arraySize;
                for (int i = 0; i < size; i++)
                {
                    var element = nodes.GetArrayElementAtIndex(i);
                    var elementId = element.FindPropertyRelative("guid");
                    if (elementId.stringValue == graphNode.id)
                    {
                        serializedProperty = element;
                    }
                }
            }
        }

        private PropertyField DrawProperty(string propertyName)
        {
            if (serializedProperty == null)
            {
                FetchSerializedProperty();
            }

            SerializedProperty property = serializedProperty.FindPropertyRelative(propertyName);
            PropertyField field = new PropertyField(property);
            field.bindingPath = property.propertyPath;
            extensionContainer.Add(field);

            return field;
        }
        
        private PropertyField DrawWorldFactProperty(string propertyName)
        {
            if (serializedProperty == null)
            {
                FetchSerializedProperty();
            }

            
            SerializedProperty property = serializedProperty.FindPropertyRelative(propertyName);
            PropertyField field = new PropertyField(property);
            field.bindingPath = property.propertyPath;
            extensionContainer.Add(field);

           // field.RegisterValueChangeCallback(OnWorldFactPropertyChanged);
            
            return field;
        }

        private void CreateFlowInputPort()
        {
            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(GOAPGraphPortTypes.FlowPort));
            inputPort.portName = "Required World State";
            inputPort.tooltip = "Flow input";
            ports.Add(inputPort);
            inputContainer.Add(inputPort);
            portsIndices.Add(ports.Count - 1);
            SavePorts();
        }

        private void CreateFlowOutputPort()
        {
            outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(GOAPGraphPortTypes.FlowPort));
            outputPort.portName = "World State";
            outputPort.tooltip = "Flow output";
            ports.Add(outputPort);
            outputContainer.Add(outputPort);
            portsIndices.Add(ports.Count - 1);
            SavePorts();
        }

        private void CreateParamInputPort(bool paramPortsHaveSingleCapacity)
        {
            Port.Capacity capacity = paramPortsHaveSingleCapacity ? Port.Capacity.Single : Port.Capacity.Multi;

            Port paramPort = InstantiatePort(Orientation.Horizontal, Direction.Input, capacity, typeof(GOAPGraphPortTypes.ParamPort));
            paramPort.portName = PARAM_PORT_NAME;
            paramPort.tooltip = "Param input";
            ports.Add(paramPort);
            inputContainer.Add(paramPort);
            portsIndices.Add(ports.Count - 1);
            SavePorts();
        }
            
        private void CreateParamOutputPort(bool paramPortsHaveSingleCapacity)
        {
            Port.Capacity capacity = paramPortsHaveSingleCapacity ? Port.Capacity.Single : Port.Capacity.Multi;

            Port paramPort = InstantiatePort(Orientation.Horizontal, Direction.Output, capacity, typeof(GOAPGraphPortTypes.ParamPort));
            paramPort.portName = "Mutated World Fact";
            paramPort.tooltip = "Param output";
            ports.Add(paramPort);
            outputContainer.Add(paramPort);
            portsIndices.Add(ports.Count - 1);
            SavePorts();
        }

        public void SavePosition()
        {
            graphNode.SetPosition(GetPosition());
        }

        public void SavePorts()
        {
            graphNode.SetPorts(portsIndices);
        }

        public void OnGraphNodeValueUpdated()
        {
            var x = 23;
            //Type typeInfo = graphNode.GetType();
            //NodeInfoAttribute info = typeInfo.GetCustomAttribute<NodeInfoAttribute>();
            //DrawProperties(typeInfo);
           //this = new GOAPGraphEditorNode(node, goapGraphObject);
        }
    }
}