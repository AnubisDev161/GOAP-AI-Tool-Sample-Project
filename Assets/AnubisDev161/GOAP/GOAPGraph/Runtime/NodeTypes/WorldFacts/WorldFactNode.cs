using GOAP.Core;

namespace GOAP.GOAPGraph
{
    public class WorldFactNode : GOAPGraphNode
    {
        [ExposedWorldFactProperty]
        public WorldFact worldFact;

        public virtual WorldFact GetData()
        {
            return worldFact;
        }
    }
}
