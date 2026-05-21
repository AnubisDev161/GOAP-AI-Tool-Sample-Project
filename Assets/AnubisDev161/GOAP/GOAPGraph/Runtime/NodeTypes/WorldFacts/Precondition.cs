using GOAP.Core;

namespace GOAP.GOAPGraph
{
    [NodeInfo("Precondition", "World Facts/Precondition", hasInputParams: false, hasOutputParams: true, outputPortName: "World Fact")]
    public class Precondition : WorldFactNode
    {
        [ExposedProperty]
        public AcceptedValue acceptedValue;
    }
}
