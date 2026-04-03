using System;
using UnityEngine;

public class HitboxBullet : MonoBehaviour
{

    public Collider BoxCollision;
    public Collider BoxTrigger;

    public event Action<Collision,GameObject> OnCollisionEnterHitbox;
    public event Action<Collider,GameObject> OnTriggerEnterHitbox;

    void OnCollisionEnter(Collision collision)
    {
        OnCollisionEnterHitbox?.Invoke(collision,this.gameObject);
    }

    void OnTriggerEnter(Collider trigger)
    {
        OnTriggerEnterHitbox?.Invoke(trigger,this.gameObject);
    }

    public void DefineTriggerSize(Vector3 size)
    {
        BoxTrigger.transform.localScale=size;
    }

    //probably methods to change the boxCollision and boxCollider parameters
}
