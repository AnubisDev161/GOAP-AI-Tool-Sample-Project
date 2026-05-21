using GOAP.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;

namespace GOAP.GOAPGraph.Editor
{
    public class GOAPGraphEditorNode : Node
    {
        public GOAPGraphNode graphNode {  get; private set; }

        private Port outputPort;
        
        private SerializedObject serializedObject;
        private SerializedProperty serializedProperty;

        public List<Port> ports {  get; private set; }
        public List<int> portsIndices { get; private set; } = new List<int>();

        public GOAPGraphEditorNode(GOAPGraphNode node, SerializedObject goapGraphObject)
        {
            this.AddToClassList("GOAP-graph-node");

            serializedObject = goapGraphObject;
            graphNode = node;

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

            if (info.hasInputParams)
            {
                CreateParamInputPort(info.paramPortsHaveSingleCapacity, info.inputPortName);

            }

            if (info.hasOutputParams)
            {
                CreateParamOutputPort(info.paramPortsHaveSingleCapacity, info.outputPortName);
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
            
            return field;
        }

        private void CreateParamInputPort(bool paramPortsHaveSingleCapacity, string inputPortName)
        {
            Port.Capacity capacity = paramPortsHaveSingleCapacity ? Port.Capacity.Single : Port.Capacity.Multi;

            Port paramPort = InstantiatePort(Orientation.Horizontal, Direction.Input, capacity, typeof(GOAPGraphPortTypes.ParamPort));
            paramPort.portName = inputPortName;
            paramPort.tooltip = "Param input";
            ports.Add(paramPort);
            inputContainer.Add(paramPort);
            portsIndices.Add(ports.Count - 1);
            SavePorts();
        }
            
        private void CreateParamOutputPort(bool paramPortsHaveSingleCapacity, string outputPortName)
        {
            Port.Capacity capacity = paramPortsHaveSingleCapacity ? Port.Capacity.Single : Port.Capacity.Multi;

            Port paramPort = InstantiatePort(Orientation.Horizontal, Direction.Output, capacity, typeof(GOAPGraphPortTypes.ParamPort));
            paramPort.portName = outputPortName;
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
    }
}