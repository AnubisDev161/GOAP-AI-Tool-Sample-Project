using GOAP.Core;

namespace GOAP.GOAPGraph
{
    [NodeInfo("Goal State", "Goal/Goal", hasFlowInput: true, hasFlowOutput: false, hasInputParams: true, hasOutputParams: false, paramPortsHaveSingleCapacity: false)]
    public class GoalWorldStateNode : GOAPGraphNode
    {
        [ExposedProperty]
        public string name;

        [ExposedProperty]
        public float priority = 1.0f;

        [ExposedProperty]
        public bool preconditionsFromWorldStateAfterAchieving;
    }
}
