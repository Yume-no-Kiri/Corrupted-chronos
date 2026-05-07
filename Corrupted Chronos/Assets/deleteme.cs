using UnityEngine;
using TMPro;

public class deleteme : MonoBehaviour
{
    TMP_Text text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = this.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = statsManager.instance.GetShipStat(Stat.StatTypeGeneral.Health).ToString();
    }
}
