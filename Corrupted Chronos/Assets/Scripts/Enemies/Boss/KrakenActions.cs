using UnityEngine;

public class KrakenActions : MonoBehaviour
{

    BossAttacks attacks;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        attacks = GetComponentInParent<BossAttacks>();
    }

    public void teleport()
    {
        attacks.teleportKraken();
    }

    public void generateWaves()
    {
        attacks.generateWaves();
    }
}
