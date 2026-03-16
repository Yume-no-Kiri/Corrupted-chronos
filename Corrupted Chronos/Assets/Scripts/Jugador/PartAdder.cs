using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;




public class PartAdder : MonoBehaviour
{
    //TODO: Acabar de revisar la relació de placementSystem i aquest script, pues per agregar 
    // parts necesito saber quines parts agregades tinc i només es troba a placement system
    #region called from placement system
    private List<PartPosition> ToAddParts ;

    private GameObject _nauGO;

    public GameObject adder{get; private set;}


    private struct PartPosition
    {
        public GameObject gameobject;
        public Vector3 position;
        public quaternion rotation;
       

        public PartPosition(GameObject gb, Vector3 pos, quaternion rot) : this()
        {
            this.gameobject = gb;
            this.position = pos;
            this.rotation= rot;
        }
    }
    public void Awake()
    {
        ToAddParts= new List<PartPosition>();
    }

    public void AssignAdder(GameObject adderAux, GameObject nauAux)
    {
        adder=adderAux;
        _nauGO=nauAux;
    }
    void Update()
    {

    }
    void FixedUpdate()
    {
        
    }

    public void AddPart(GameObject gb, Vector3 pos, quaternion rot)
    {
        ToAddParts.Add(new PartPosition(gb,pos,rot));
    }

    private void CreatePart(PartPosition cpart , Vector3 originNau)
    {
        Vector3 offset= new Vector3(0.25f,0,0.25f);
        GameObject gb= cpart.gameobject;
        Vector3 position=cpart.position;
        quaternion rot= cpart.rotation;
        //Aquesta marabunda de codi funciona :D
        Vector3 newPos=position-ToAddParts[0].position;
        Debug.Log("position will spawn1:"+  newPos.ToString());

        GameObject instPart = Instantiate(gb,adder.transform);
        instPart.transform.localRotation=rot;
        instPart.transform.localPosition=newPos+offset;
      
        Transform coll = instPart.transform.Find("Collisions");
        if (coll != null) coll.gameObject.SetActive(true);
        
        
    }
    
  
    public void ActivateParts()
    {
        //acabar
        PartActions pa;
        foreach (Transform part in adder.transform)
        {
            //no estic segur de que part actions segui lo millor per invocar aquests mètodes,
            //revisar explicació escrita en EachPartScript per futur REFACTORITZACIÓ
           
            // pa = part.GetComponent<PartActions>();
            pa= part.GetComponent<EachPartScript>().Activate();
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
            /* if (col != null)
            {
                //List<GameObject> dettors = new List<GameObject>();
                DetectorCapa[] script= col.transform.GetComponentsInChildren<DetectorCapa>();
                foreach (var s in script)
                {
                    if (DetectorsCapa.Contains(s.gameObject))
                    {
                        DetectorsCapa.Remove(s.gameObject);
                    }
                }
                
            } */
            
            
            Destroy(child.gameObject);
        }
        // ToAddParts.Clear();
        // AddedParts=new List<GameObject>();


    }

    public void ResetPlacement()
    {
        ToAddParts.Clear();
   
        RemovePart();
    }

    public void SavePlacement()
    {
        RemovePart();

        Vector3 origin= GameManager.Instance.playerInstance.transform.position;
        if (ToAddParts.Count > 0)
        {
            for (int i = 1; i < ToAddParts.Count; i++)
            {                
                CreatePart(ToAddParts[i],  origin);
            }
        }
        ActivateParts();

    }




    #endregion
}
