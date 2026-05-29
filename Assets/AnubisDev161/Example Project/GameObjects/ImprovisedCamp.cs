using GOAP.GOAPGraph;
using UnityEngine;

namespace ExampleProject
{
    public class ImprovisedCamp : SmartObject
    {
        public override void Interact(GOAPGraphAsset currentGraph)
        {
            Destroy(gameObject);
        }
    }
}