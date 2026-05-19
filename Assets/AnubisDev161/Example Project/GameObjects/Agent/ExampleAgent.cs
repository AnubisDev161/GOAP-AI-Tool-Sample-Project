using GOAP;
using GOAP.Data;
using System.Collections;
using UnityEngine;

public class ExampleAgent : GOAPAgent
{
    [SerializeField]
    private GameObject beaconPrefab;

    [SerializeField]
    private float tiredInterval;

    [SerializeField]
    private string tiredKey;
    public bool PlaceBeacon()
    {
        var beacon = Instantiate(beaconPrefab, transform.position, Quaternion.identity);

        if (beacon == null) return false;
        Debug.Log("<color=green>Beacon placed!");

        return true;
    }

    protected override void Init()
    {
        base.Init();
        StartCoroutine(SetTiredAfterDelay());
    }

    private IEnumerator SetTiredAfterDelay()
    {
        yield return new WaitForSeconds(tiredInterval);


    }

}
