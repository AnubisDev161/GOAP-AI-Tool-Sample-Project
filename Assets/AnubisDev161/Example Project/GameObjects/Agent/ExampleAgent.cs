using GOAP.Core.Agent;
using UnityEngine;

namespace ExampleProject
{
    public class ExampleAgent : GOAPAgent
    {
        [SerializeField]
        private GameObject beaconPrefab;
        public bool PlaceBeacon()
        {
            var beacon = Instantiate(beaconPrefab, transform.position, Quaternion.identity);

            if (beacon == null) return false;
            Debug.Log("<color=green>Beacon placed!");

            return true;
        }
    }
}