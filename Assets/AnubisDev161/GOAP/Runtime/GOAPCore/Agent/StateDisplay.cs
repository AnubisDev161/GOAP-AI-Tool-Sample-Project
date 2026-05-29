using System.Collections;
using TMPro;
using UnityEngine;

namespace GOAP.Core.Agent
{
    [RequireComponent(typeof(TMP_Text))]
    public class StateDisplay : MonoBehaviour
    {
        [SerializeField]
        private GOAPAgent agent;
        private TMP_Text textDisplay;
        [SerializeField]
        private Canvas canvas;
        [SerializeField]
        private float updateInterval = 0.3f;

        private void OnEnable()
        {
            textDisplay = GetComponent<TMP_Text>();
            agent.newActionStarted += UpdateText;
            StartCoroutine(UpdateRotation());
        }

        public void UpdateText(GOAPAction currentAction)
        {
            textDisplay.text = currentAction.name;
        }

        private IEnumerator UpdateRotation()
        {
            yield return new WaitForSeconds(updateInterval);
            canvas.transform.LookAt(Camera.main.transform);
            StartCoroutine(UpdateRotation());
        }
    }
}
