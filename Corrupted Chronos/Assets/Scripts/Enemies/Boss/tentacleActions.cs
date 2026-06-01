using UnityEngine;

public class tentacleActions : MonoBehaviour
{
    TentacleAttack tentacle;

    void Awake()
    {
        tentacle = GetComponentInParent<TentacleAttack>();
    }

    public void stabAttack()
    {
        tentacle.SpawnStabBullet();
    }

    public void spinAttack()
    {
        tentacle.SpawnSpinBullet();
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
