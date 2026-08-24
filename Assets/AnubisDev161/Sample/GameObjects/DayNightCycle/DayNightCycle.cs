using System;
using UnityEngine;

namespace ExampleProject
{
    [RequireComponent(typeof(Light))]
    public class DayNightCycle : MonoBehaviour
    {
        [SerializeField]
        private Color defaultColor;

        [SerializeField]
        private Color nightColor;

        public static DayNightCycle instance;
        public Action OnNightEnded;

        public void StartNight()
        {
            GetComponent<Light>().color = nightColor;
        }

        public void EndNight()
        {
            GetComponent<Light>().color = defaultColor;
            OnNightEnded?.Invoke();
        }
    }
}