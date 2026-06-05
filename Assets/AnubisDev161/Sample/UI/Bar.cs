using GOAP.Core;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private float maxValue;
    private void Start()
    {
        image.fillAmount = 0.0f;
    }

    public void UpdateValue(WorldFact worldFact)
    {
        image.fillAmount = (float)worldFact.GetValue() / maxValue;
    }
}
