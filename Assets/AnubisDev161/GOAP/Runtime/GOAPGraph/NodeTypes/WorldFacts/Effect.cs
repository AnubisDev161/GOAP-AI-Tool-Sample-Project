using GOAP.Core;

namespace GOAP.GOAPGraph
{
    [NodeInfo("Effect", "World Facts/Effect", hasInputParams: true, inputPortName: "World Fact")]
    public class Effect : WorldFactNode
    {
        [ExposedProperty]
        public OperationType operationType;
    }
}
