using GOAP.Core;
using System.Collections.Generic;

namespace GOAP.Tree
{
    public class GOAPNode
    {
        public GOAPAction action {  get; private set; }
        public GOAPNode parent;
        public float fCost => gCost + hCost;
        public float hCost{ get;  set; }
        public float gCost { get;  set; }

        public WorldState requiredWorldState {  get; private set; }
        public GOAPNode(GOAPAction action, GOAPNode parent, WorldState requiredWorldState, float gCost = 1, float hCost = 1)
        {
            this.action = action;
            this.parent = parent;
            this.requiredWorldState = requiredWorldState;
            this.hCost = hCost;
            this.gCost = gCost;
        }
        public GOAPNode(GOAPAction action, GOAPNode parent, Dictionary<string, WorldFact> requiredWorldState, float gCost = 1, float hCost = 1)
        {
            this.action = action;
            this.parent = parent;
            this.requiredWorldState = new WorldState(requiredWorldState);
            this.hCost = hCost;
            this.gCost = gCost;
        }

        public override int GetHashCode()
        {
            return requiredWorldState.worldFacts.Count;
        }

        public override bool Equals(object obj) 
        {
            var other = (GOAPNode)obj;

            return other.requiredWorldState == requiredWorldState;
        }
    }
}