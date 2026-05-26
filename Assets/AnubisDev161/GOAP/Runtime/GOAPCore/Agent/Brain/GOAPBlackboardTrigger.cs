using UnityEngine;

namespace GOAP.Core.Agent
{
    /// <summary>
    /// Base class for all blackbaord triggers
    /// </summary>
    [RequireComponent(typeof(GOAPAgent))]
    public class GOAPBlackboardTrigger : MonoBehaviour
    {
        /// <value>
        /// Defines the name of the blackboard key to change, this can either be a normal blacbkoard key or a world fact
        /// </value>
        [SerializeField]
        private string keyName;

        /// <value>
        /// Defines the expected type of the blackboard key to change, this makes sure you are writing the right data type in the blackboard key
        /// </value>
        [Tooltip("Defines the expected type of the blackboard key to change, this makes sure you are writing the right data type in the blackboard key")]
        [SerializeField]
        private GOAPBlackbaord.BlackboardKeyType expectedType;

        /// <value>
        /// Defines the expected world fact type of the world fact to change, this makes sure you are writing the right data type in the world fact
        /// </value>
        [SerializeField]
        private WorldFactType expectedWorldFactValueType;

        /// <summary>
        /// Retrieves the specified blackboard key from the graph's blackboard
        /// </summary>
        protected virtual GOAPBlackbaord.BlackboardKey GetSpecifiedlBackboardKey()
        {
            var agent = GetComponent<GOAPAgent>();
            var blackboardKey = agent.goapBrain.graphInstance.Blackboard.GetKeyWithExpectedType(keyName, expectedType);

            if (blackboardKey == null)
            {
                Debug.LogError($"Could not find specified key with name [{keyName}]");

            }
            else if (blackboardKey.keyType != expectedType)
            {
                Debug.LogError($"Specified Key with name [{keyName}] has a different key type [{expectedType}] than the key in the blackboard [{blackboardKey.keyType}]");
            }

            return blackboardKey;
        }

        /// <summary>
        /// Retrieves the specified world fact from the GOAP brain's current world state
        /// </summary>
        protected virtual WorldFact? GetSpecifiedWorldFact()
        {
            var agent = GetComponent<GOAPAgent>();

            agent.goapBrain.currentWorldState.worldFacts.TryGetValue(keyName, out WorldFact worldFact);

            if (worldFact.name != keyName)
            {
                Debug.LogError($"Could not find specified key with name [{keyName}]");
                return null;

            }
            else if (worldFact.valueType != expectedWorldFactValueType)
            {
                Debug.LogError($"Specified Key with name [{keyName}] has a different value type [{expectedWorldFactValueType}] than the world fact in the blackboard [{worldFact.valueType}]");
                return null;
            }

            return worldFact;
        }

        /// <summary>
        /// Uses the TryAddFact method of the current world state to add a new world fact or set it to a new value if the world fact is already defined in the current world state
        /// </summary>
        protected virtual bool AddSpecifiedWorldFactWithValue(string newValue, bool callWorldStateChangedByTrigger)
        {
            var agent = GetComponent<GOAPAgent>();
            if (agent.goapBrain.currentWorldState.worldFacts.TryGetValue(keyName, out WorldFact specifiedWorldFact) && (specifiedWorldFact.valueType != expectedWorldFactValueType))
            {
                Debug.LogError($"Specified Key with name [{keyName}] has a different value type [{expectedWorldFactValueType}] than the world fact that is already defined in the world state [{specifiedWorldFact.valueType}]");
                return false;
            }

            specifiedWorldFact = new WorldFact();
            specifiedWorldFact.value = newValue;
            specifiedWorldFact.valueType = expectedWorldFactValueType;
            specifiedWorldFact.name = keyName;
            return agent.goapBrain.currentWorldState.TryAddFact(specifiedWorldFact, callWorldStateChangedByTrigger);
        }
    }
}