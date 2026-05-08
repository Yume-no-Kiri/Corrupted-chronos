using UnityEngine;

public class tentacleActions : MonoBehaviour
{
    TentacleAttack tentacle;

    void Awake()
    {
        tentacle = GetComponentInParent<TentacleAttack>();
    }

    public void slamAttack()
    {
        tentacle.SpawnSlamBullets();
    }

    public void splashAttack()
    {
        tentacle.SpawnSplashBullets();
    }
}
