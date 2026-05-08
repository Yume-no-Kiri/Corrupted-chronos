using UnityEngine;

public class KrakenActions : MonoBehaviour
{
    BossAttacks attacks;
    
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

    public void shootLaser()
    {
        attacks.ShootLaser();
    }
}
