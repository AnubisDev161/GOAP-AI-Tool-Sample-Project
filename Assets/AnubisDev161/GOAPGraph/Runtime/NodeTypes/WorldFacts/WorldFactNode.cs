using GOAP.Data;

namespace GOAPGraph
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
