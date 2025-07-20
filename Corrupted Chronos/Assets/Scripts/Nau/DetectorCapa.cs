using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum DetectCanviCapaType
{
    Up,
    Low,
} 


public class DetectorCapa : MonoBehaviour
{
    //private Collider[] hits;
    [SerializeField]
    DetectCanviCapaType whatIDetect;

    //si hi han valors repetits(problemes canviar a un list, encara que sigui menys eficient
    //no hauria d'haver problemes perque els clons son tractats com objectes diferents
    private HashSet<GameObject> _objectsInside;

    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _objectsInside = new HashSet<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        bool isLock = true;
        _objectsInside.Add(other.gameObject);
        GetComponentInParent<Player>().DetectorCapaResponse(whatIDetect, isLock);
    }

    void OnTriggerExit(Collider other)
    {
        _objectsInside.Remove(other.gameObject);

        if (_objectsInside.Count == 0)
        {
            bool isLock = false;
            GetComponentInParent<Player>().DetectorCapaResponse(whatIDetect, isLock);
        }
    }
    
    
}
