using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/*
    TIENE TODOS LOS CODIGOS ARMAS 

*/


//Maybe shootablLeft i shootable Right, no es lo millor per el joc final, 
//pero la demo servirà 
public enum TypePart
{
    ShootableLeft,
    ShootableRight,
    Mele,
    Moveable
    
}


//classe general, del que venen les diferents parts, revisar en un futur
public class PartBase : MonoBehaviour
{


/*     protected string nameItem;
    protected ItemData itemData;
 */

    
    //Ha d'haver una millor manera de guardar l'informació de cada part 
    
    //private InputManager inputManager;
    protected TypePart typePart;
    
    //per shoot:
     //protected InformationBullet bulletInfo;//per agafar prefab
     protected GameObject bulletInstance;//per crear bales
    // protected VisualEffect shoot_vfx;

    // protected string nameTypeBullet = "BulletBasicMB";
     protected Transform firepoint;
     protected GameObject bulletPrefab;

     protected bool canShot = true;
     protected float t2s = 1f;
     protected float t2s2 = 1f;
     protected int cargador = 5;
     protected int contCargador = 5;
     protected int malBala = 1;

    /* void Awake()
    {
        nameItem=this.GetType().Name;
        itemData= GameManager.Instance.itemDataBase.ReturnItemDataByName(nameItem);

    }
    public ItemData GetScipteableObject()
    {
        return itemData;
    } */

    public TypePart GetTypePart()
    {
        return typePart;
    }
    
    
    //si serialitzem els camps això ja no és necesari
    //protected virtual void  saveVariablesShot(){}    
    void Start() { }
    void Update() { }

    public virtual void DoShot( InputAction.CallbackContext ctx)
    {
        
        throw new System.NotImplementedException();
    }

    public void PassVariables(Transform firepoint,GameObject bulletPrefab)
    {
        this.firepoint = firepoint;
        this.bulletPrefab = bulletPrefab;
    }

    
    
}

public class Metralleta : PartBase
{
    void Awake()
    {
        t2s = 0.25f;
        typePart= TypePart.ShootableLeft;
    }

    /*protected override void saveVariablesShot()
    {
        
    }*/
    
    
    public override void DoShot(InputAction.CallbackContext ctx)
    {
        if (canShot && ctx.performed)
        {
            Debug.Log("Transfrom.position: " + transform.position);
            StartCoroutine(ShotIE());
        }
        
        
        Debug.Log("AQUÍ INSTANCIES BALA");
    }

    private IEnumerator ShotIE()
    {
        Quaternion rotationWithOffset = firepoint.GetComponentInParent<Transform>().rotation * Quaternion.Euler(0, 90, 90);

        bulletInstance = Instantiate(bulletPrefab, firepoint.position, rotationWithOffset);
        BaseBullets bulletInfo = bulletInstance.GetComponent<BaseBullets>();
        bulletInfo.DefinirBala(1, +3);
        
        canShot = false;
        //Debug.Log($"t2s: {t2s-t2s %PlayerStats.CooldownBalaJugador}");
        yield return new WaitForSeconds(t2s);
        canShot = true;
    }
    
    
}

public class Escopeta : PartBase
{
    void Awake()
    {
        t2s = 1f;
        typePart= TypePart.ShootableRight;
    }

    public override void DoShot(InputAction.CallbackContext ctx)
    {
        if (canShot && ctx.performed)
        {
            Debug.Log("Transfrom.position: " + transform.position);
            StartCoroutine(ShotIE());
        }
        
        
        Debug.Log("AQUÍ INSTANCIES BALA");
    }

    private IEnumerator ShotIE()
    {
        int valor = 10;
        int nbullets = 5;
        for (int i = 0; i < nbullets; i++)
        {
            valor *= i;
            Quaternion rotationWithOffset = firepoint.GetComponentInParent<Transform>().rotation * Quaternion.Euler((-20+valor), 90, 90);

            bulletInstance = Instantiate(bulletPrefab, firepoint.position, rotationWithOffset);
            BaseBullets bulletInfo = bulletInstance.GetComponent<BaseBullets>();
            bulletInfo.DefinirBala(1, -8 );
            valor =10;

        }
        
        canShot = false;
        //Debug.Log($"t2s: {t2s-t2s %PlayerStats.CooldownBalaJugador}");
        yield return new WaitForSeconds(t2s);
        canShot = true;
    }

    
    
}