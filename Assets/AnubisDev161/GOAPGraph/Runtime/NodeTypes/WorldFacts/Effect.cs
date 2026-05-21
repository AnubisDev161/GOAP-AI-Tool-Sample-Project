using GOAP.Data;
using UnityEngine;

namespace GOAPGraph
{
    [NodeInfo("Effect", "World Facts/Effect", hasInputParams: true, inputPortName: "World Fact")]
    public class Effect : WorldFactNode
    {
        [ExposedProperty]
        public OperationType operationType;
    }
}
