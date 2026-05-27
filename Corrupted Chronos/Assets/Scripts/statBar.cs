using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class StatBar : MonoBehaviour
{
    public Stat.StatTypeGeneral statType;
    public Stat.StatTypeGeneral statMax;

    public Image fillImage;
    public Color fillColor;

    private Slider slider;

    private float initialMaxValue;
    private Vector3 initialScale;

    void Start()
    {
        slider = GetComponent<Slider>();

        initialMaxValue = statsManager.instance.GetShipStat(statMax);
        initialScale = transform.localScale;

        if (fillImage)
        {
            fillImage.color = fillColor;
        }
    }

    void Update()
    {
        float currentValue = statsManager.instance.GetShipStat(statType);
        float currentMaxValue = statsManager.instance.GetShipStat(statMax);

        slider.maxValue = currentMaxValue;
        slider.value = currentValue;

        float scaleMultiplier = currentMaxValue / initialMaxValue;

        transform.localScale = new Vector3(
            initialScale.x * scaleMultiplier,
            initialScale.y,
            initialScale.z
        );
    }
}