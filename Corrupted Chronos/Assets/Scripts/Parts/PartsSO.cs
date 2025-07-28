using UnityEngine;

/*Això ho tenia SS pero amb els canvis que portem i tenin en compte que volem fer algo millor
 * dubto de l'utilitat d¡incloure un scripteable object. 
 * 
 */


public abstract class PartsSO : ScriptableObject
{
    public abstract string nomUI { get; }
    public Sprite Sprite;

   // public abstract Type MonoBehaviourType { get; }
}

[CreateAssetMenu(fileName = "Metralleta", menuName = "Scriptable Objects/Metralleta")]
public class Metralleta1SO : PartsSO
{
    //assigna
    public override string nomUI => "machine gun";
    //public override Sprite Sprite;
    //[SerializeField] private Transform customFirepoint;


    public string ID = "Metralleta1MB.cs"; //el nom que t� a PartsMB

    //public override Type MonoBehaviourType => typeof(Metralleta1MB);
//relaciona
//public override Transform Firepoint => customFirepoint;
}