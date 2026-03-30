using UnityEngine;

[CreateAssetMenu(menuName = "Combat/BulletData")]
public class bulletStatsSO : ScriptableObject
{
    [Header("Bullet Properties")]
    [Tooltip("The name of the bullet")]
    public Sprite sprite;
    public int damage;
    public float penetration;
    public float distEffective;
    public float distMax;
    public float speed;

    [Header("Gameobjects")]
    public GameObject[] hitEffects;
    public GameObject[] trailEffects;
}