using GOAP.Data;

namespace GOAPGraph
{

    public class WorldFactNode : GOAPGraphNode
    {
        [ExposedWorldFactProperty]
        public WorldFact worldFact;

        public WorldFact GetData()
        {
            return worldFact;
        }
    }
}
