using System;
using UnityEngine.UIElements;

namespace GOAPGraph
{
    public class NodeInfoAttribute : Attribute
    {
        public string title { get; private set; }
        public string menuItem {  get; private set; }
        public bool hasFlowInput { get; private set; }
        public bool hasFlowOutput { get; private set; }

        public bool hasInputParams { get; private set; }
        public bool hasOutputParams { get; private set; }

        public bool paramPortsHaveSingleCapacity { get; private set; }

        public NodeInfoAttribute(string title, string menuItem = "", bool hasFlowInput = true, bool hasFlowOutput = true, bool hasInputParams = false, bool hasOutputParams = false, bool paramPortsHaveSingleCapacity = true)
        {
            this.title = title;
            this.menuItem = menuItem;
            this.hasFlowInput = hasFlowInput;
            this.hasFlowOutput = hasFlowOutput;
            this.hasInputParams = hasInputParams;
            this.hasOutputParams = hasOutputParams;
            this.paramPortsHaveSingleCapacity = paramPortsHaveSingleCapacity;
        }
    }
}