using System;

namespace GOAP.GOAPGraph
{
    public class NodeInfoAttribute : Attribute
    {
        public string title { get; private set; }
        public string menuItem {  get; private set; }
        public bool hasFlowInput { get; private set; }
        public bool hasFlowOutput { get; private set; }
        public bool hasInputParams { get; private set; }
        public string inputPortName { get; private set; }
        public bool hasOutputParams { get; private set; }
        public string outputPortName { get; private set; } 
        public bool paramPortsHaveSingleCapacity { get; private set; }

        public NodeInfoAttribute(
            string title, string menuItem = "", bool hasFlowInput = true, bool hasFlowOutput = true, bool hasInputParams = false, bool hasOutputParams = false, 
            bool paramPortsHaveSingleCapacity = false, string inputPortName = "Preconditions", string outputPortName = "Effects")
        {
            this.title = title;
            this.menuItem = menuItem;
            this.hasFlowInput = hasFlowInput;
            this.hasFlowOutput = hasFlowOutput;
            this.hasInputParams = hasInputParams;
            this.hasOutputParams = hasOutputParams;
            this.paramPortsHaveSingleCapacity = paramPortsHaveSingleCapacity;
            this.inputPortName = inputPortName;
            this.outputPortName = outputPortName;
        }
    }
}