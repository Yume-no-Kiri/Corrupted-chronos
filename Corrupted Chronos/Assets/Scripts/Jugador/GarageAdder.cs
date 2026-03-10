using System.Collections.Generic;
using UnityEngine;

public class GarageAdder : MonoBehaviour
{
       
    #region called from placement system
    public List<GameObject> AddedParts { get; private set; }

    private GameObject _nauGO;

    public GameObject adder{get; private set;}

    public void Awake()
    {
        adder=transform.GetChild(0).gameObject;
    }


    public void AddPart(GameObject gb, Vector3 origin)
    {
        //Aquesta marabunda de codi funciona :D
        GameObject part = Instantiate(gb);
        Vector3 newOrigin = transform.position;
        Vector3 relative =gb.transform.position;
        Vector3 finalWorldPosition = relative -origin ;
        finalWorldPosition+=newOrigin;
        finalWorldPosition.y=_nauGO.transform.position.y;
        part.transform.position = finalWorldPosition;
        Transform ToAdd = _nauGO.transform.Find("Added");
        if (ToAdd != null)
        {
            part.transform.SetParent(ToAdd.transform, true);
        }
        Transform coll = part.transform.Find("Collisions");
        if (coll != null) coll.gameObject.SetActive(true);
        
        AddedParts.Add(part);
    }
    
    public void RemovePart()
    {
        PartActions pa;

        // Transform ToRemove = _nauGO.transform.Find("Added");
        foreach (Transform child in adder.transform)
        {
            
            pa = child.gameObject.GetComponent<PartActions>();
            switch (pa.GetTypePart())
            {
                case TypePart.Mele:
                    Debug.LogError("part mele no acabat");
                    break;
                case TypePart.Moveable:
                    Debug.LogError("part movable no acabat");
                    break;
                case TypePart.ShootableLeft:
                    GameManager.Instance.inputManager.OnShotLeft -= pa.DoShot;
                    break;
                case TypePart.ShootableRight:
                    GameManager.Instance.inputManager.OnShotRight -= pa.DoShot;
                    break;
            }
            
            GameObject col=  child.transform.Find("Collisions").gameObject;

            //revsiar detector de capa
            if (col != null)
            {
                //List<GameObject> dettors = new List<GameObject>();
                DetectorCapa[] script= col.transform.GetComponentsInChildren<DetectorCapa>();
                /* foreach (var s in script)
                {
                    if (DetectorsCapa.Contains(s.gameObject))
                    {
                        DetectorsCapa.Remove(s.gameObject);
                    }
                } */
                
            }
            
            
            Destroy(child.gameObject);
        }
        AddedParts.Clear();

    }

    public void ActivateParts()
    {
        PartActions pa;
        foreach (GameObject part in AddedParts)
        {
            //no estic segur de que part actions segui lo millor per invocar aquests mètodes,
            //revisar explicació escrita en EachPartScript per futur REFACTORITZACIÓ
            pa = part.GetComponent<PartActions>();
            switch (pa.GetTypePart())
            {
                case TypePart.Mele:
                    Debug.LogWarning("part mele no acabat");
                    break;
                case TypePart.Moveable:
                    Debug.LogWarning("part movable no acabat");
                    break;
                case TypePart.ShootableLeft:
                    GameManager.Instance.inputManager.OnShotLeft += pa.DoShot;
                    break;
                case TypePart.ShootableRight:
                    GameManager.Instance.inputManager.OnShotRight += pa.DoShot;
                    break;
            }
        }
    }

    public void ResetPlacement()
    {
        foreach (var placedObject in AddedParts) Destroy(placedObject);
        AddedParts.Clear();

        /* if (nauAdded != null)
        {     */
            RemovePart();
        // }
    }

    public void SavePlacement()
    {
        RemovePart();
        Vector3 origin= AddedParts[0].transform.position;
        if (AddedParts.Count > 0)
        {
            for (int i = 1; i < AddedParts.Count; i++)
            {
                GameObject part=AddedParts[i];
                
                AddPart(part,  origin);
            }
        }
        ActivateParts();

    }



    #endregion
}
