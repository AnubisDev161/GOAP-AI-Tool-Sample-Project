using GOAP;
using GOAP.Data;
using System.ComponentModel;
using UnityEngine;

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
    private WorldFactType expectedWorldFactType;

    /// <summary>
    /// Retrieves the specified blackboard key from the graph's blackboard
    /// </summary>
    protected virtual GOAPBlackbaord.BlackboardKey GetSpecifiedlBackboardKey()
    {
        var agent = GetComponent<GOAPAgent>();
        var blackboardKey = agent.goapBrain.graphInstance.Blackboard.GetKey(keyName);

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
    /// Retrieves the specified world fact from the graph's blackboard
    /// </summary>
    protected virtual GOAPBlackbaord.BlackboardKey GetSpecifiedWorldFact()
    {
        var agent = GetComponent<GOAPAgent>();
        var worldFact = agent.goapBrain.graphInstance.Blackboard.GetKey(keyName);

        if (worldFact == null)
        {
            Debug.LogError($"Could not find specified key with name [{keyName}]");
            return null;

        }
        else if (worldFact.worldFactType != expectedWorldFactType)
        {
            Debug.LogError($"Specified Key with name [{keyName}] has a different world fact type [{expectedWorldFactType}] than the world fact in the blackboard [{worldFact.worldFactType}]");
            return null;
        }

        return worldFact;
    }
}