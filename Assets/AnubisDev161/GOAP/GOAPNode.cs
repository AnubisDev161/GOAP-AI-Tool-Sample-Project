using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace GOAP
{
    public class GOAPNode
    {
        public GOAPAction action {  get; private set; }
        public GOAPNode parent;
        public float fCost { get;  set; }
        public float hCost{ get;  set; }
        public float gCost { get;  set; }

        public WorldState requiredWorldState {  get; private set; }
        public GOAPNode(GOAPAction action, GOAPNode parent, WorldState requiredWorldState, float fCost, float gCost = 1, float hCost = 1)
        {
            this.action = action;
            this.parent = parent;
            this.requiredWorldState = requiredWorldState;
            this.fCost = fCost;
            this.hCost = hCost;
            this.gCost = gCost;
        }
        public GOAPNode(GOAPAction action, GOAPNode parent, Dictionary<string, bool> requiredWorldState, float fCost, float gCost = 1, float hCost = 1)
        {
            this.action = action;
            this.parent = parent;
            this.requiredWorldState = new WorldState(requiredWorldState);
            this.fCost = fCost;
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