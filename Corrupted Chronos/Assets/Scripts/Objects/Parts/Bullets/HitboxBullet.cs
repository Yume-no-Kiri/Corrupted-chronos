using System;
using UnityEngine;

public class HitboxBullet : MonoBehaviour
{

    //name of the collider
    // public string name;

    public Collider BoxCollision;
    public Collider BoxTrigger;

    public event Action<Collision,GameObject> OnCollisionEnterHitbox;
    public event Action<Collider,GameObject> OnTriggerEnterHitbox;

    
    public event Action<Collision,GameObject> OnCollisionExitHitbox;
    public event Action<Collider,GameObject> OnTriggerExitHitbox;

    void Awake()
    {
        // Physics.IgnoreCollision(GetComponent<Collider>(), GetComponent<Collider>(), true);
    }
    void OnCollisionEnter(Collision collision)
    {
        OnCollisionEnterHitbox?.Invoke(collision,this.gameObject);
    }

    void OnTriggerEnter(Collider trigger)
    {
        OnTriggerEnterHitbox?.Invoke(trigger,this.gameObject);
    }

    void OnCollisionExit(Collision collision)
    {
        OnCollisionExitHitbox?.Invoke(collision,this.gameObject);
    }

    void OnTriggerExit(Collider trigger) {
        OnTriggerExitHitbox?.Invoke(trigger,this.gameObject);
        
    }

    public void DefineTriggerSize(Vector3 size)
    {
        BoxTrigger.transform.localScale=size;
        
    }

    public void DeactivateCollider()
    {
        BoxCollision.enabled=false;
    }

    //probably methods to change the boxCollision and boxCollider parameters
}
