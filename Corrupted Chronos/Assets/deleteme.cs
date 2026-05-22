using UnityEngine;
using TMPro;

public class deleteme : MonoBehaviour
{
    TMP_Text text;

    public Stat.StatTypeGeneral statType = Stat.StatTypeGeneral.MaxHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = this.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = statsManager.instance.GetShipStat(statType).ToString();
    }
}
