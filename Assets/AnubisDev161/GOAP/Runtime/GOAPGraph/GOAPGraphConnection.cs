using System;
using UnityEngine;

namespace GOAP.GOAPGraph
{
    [Serializable]
    public struct GOAPGraphConnection
    {
        public GOAPGraphConnectionPort inputPort;
        public GOAPGraphConnectionPort outputPort;

        public GOAPGraphConnection(GOAPGraphConnectionPort input, GOAPGraphConnectionPort outPut)
        {
            inputPort = input;
            outputPort = outPut;
        }

        public GOAPGraphConnection(string inputPortId, int inputIndex, string outputPortId, int outputPortIndex)
        {
            inputPort = new GOAPGraphConnectionPort(inputPortId, inputIndex);
            outputPort = new GOAPGraphConnectionPort(outputPortId, outputPortIndex);
        }
    }

    [Serializable]
    public struct GOAPGraphConnectionPort
    {
        public string nodeId;
        public int portIndex;

        public GOAPGraphConnectionPort(string nodeId, int portIndex)
        {
            this.nodeId = nodeId;
            this.portIndex = portIndex;
        }        
    }
}
