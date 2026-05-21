using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using GOAP;

namespace GOAPGraph
{
    public class GOAPGraphObject : MonoBehaviour // TODO Replace with GOAP Brain
    {
        public GOAPNavigation navigation { get; private set; }
        private void OnEnable()
        {
            var navigation = GetComponent<GOAPNavigation>();

            if (navigation == null)
            {
                Debug.LogError("No GOAPNaviagtion component found!");
                return;
            }

            this.navigation = navigation;
        }
    }
}

