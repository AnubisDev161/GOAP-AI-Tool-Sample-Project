using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace GOAP.Core.Agent
{
    [RequireComponent(typeof(TMP_Text))]
    public class StateDisplay : MonoBehaviour
    {
        [SerializeField]
        private GOAPAgent agent;
        private TMP_Text textDisplay;

        private void OnEnable()
        {
            textDisplay = GetComponent<TMP_Text>();
            agent.newActionStarted += UpdateText;
        }

        public void UpdateText(GOAPAction currentAction)
        {
            textDisplay.text = currentAction.name;
        }
    }
}
