using UnityEngine;

public enum TypePart
{
    Shootable,
    Mele,
    Moveable
    
}


//classe general, del que venen les diferents parts, revisar en un futur
public class PartActions : MonoBehaviour
{
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

     protected float t2s = 1f;
     protected float t2s2 = 1f;
     protected int cargador = 5;
     protected int contCargador = 5;
     protected int malBala = 1;
    
    
    
    public TypePart GetTypePart()
    {
        return typePart;
    }
    
    
    //si serialitzem els camps això ja no és necesari
    //protected virtual void  saveVariablesShot(){}    
    void Start() { }
    void Update() { }

    public virtual void DoShot()
    {
        throw new System.NotImplementedException();
    }
}

public class Metralleta : PartActions
{
    void Start()
    {
        typePart= TypePart.Shootable;
    }

    /*protected override void saveVariablesShot()
    {
        
    }*/
    
    
    public override void DoShot()
    {
        
    }
}