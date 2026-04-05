using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



//aquí afegir més tipus com: punta cano
public enum TypeGround
{
    Null,
    Occupied,
    Buildable,


    Muzzle, //boquilla de arma
    Magazine, //cargador
    OverHeat,
} 



public class GridData
{
    //information ground with the aprts
    PlacementData2 placementData= new PlacementData2();

    private Vector3Int _gridPosition= new Vector3Int();
    private  PlacementDataSO _originalPart= new PlacementDataSO();
    private int _rotationAdded;

    private bool _lastCheckCanPlace;

    private  PlacementDataSO _updatedPart= new PlacementDataSO();

  

    #region Checker and adder
    public bool CanPlaceObejctAt(Vector3Int gridPosition, PlacementDataSO part, int rotationToAdd) //Dictionary<TypeGround,Requiriments> partRequires)
    {
        Debug.LogWarning("entres a CanPlaceObejctAt???");

        if(AlreadyChecked( gridPosition, part, rotationToAdd))return _lastCheckCanPlace;
        
        RecalculatePosition(gridPosition, part, rotationToAdd);  //recalcular tot objectPart position segons gridPosition

        _gridPosition=gridPosition;
        _originalPart=part;
        _rotationAdded=rotationToAdd;


        
        _lastCheckCanPlace= placementData.checkRequirement(_updatedPart.ConfigRequires);
        // _canPlace=true;

        Debug.Log("show positiont will add"+_updatedPart.debugConfigGround());
        return _lastCheckCanPlace;
      
    }

    public void HasPlacedPart(Vector3Int gridPosition)
    {
        /* 

         */

        //  placementData.
    }
    public void AddObjectAt(Vector3Int gridPosition, PlacementDataSO part, int rotationToAdd)//wa
    // wa, int placedObjectIndex)
    // public void AddObjectAt(Vector3Int gridPosition, HashSet<Vector2Int> objectSize, HashSet<Vector2Int> buildableSize,  int ID, int placedObjectIndex)
    {
        Debug.LogWarning("entres a addObjectAt???");
        if(AlreadyChecked( gridPosition, part, rotationToAdd))
        {
            
            placementData.AddNewPartData(_updatedPart);
        }
        else
        {
            if(part==null) Debug.LogError("error estrany de part null");
            if(part.ConfigGround==null) Debug.LogError("error estrany de configGround null");

            RecalculatePosition(gridPosition,part, rotationToAdd); 
            _gridPosition=gridPosition;
            _originalPart=part;
            _rotationAdded=rotationToAdd;
  

            placementData.AddNewPartData(_updatedPart);
        }

        Debug.Log("add to position"+ gridPosition.ToString());
        returnDebugStrings();
       
    }
    private bool AlreadyChecked(Vector3Int gridPosition, PlacementDataSO part, int rotationToAdd)
    {
        if(gridPosition==_gridPosition && part==_originalPart && rotationToAdd== _rotationAdded) return true;
        else return false;
    }
    #endregion

    #region Calculations
    private void RecalculatePosition(Vector3Int gridPosition, PlacementDataSO part, int rotationToAdd)
    {
        _updatedPart.ConfigGround = new Dictionary<TypeGround, HashSet<Vector2Int>>();
        _updatedPart.ConfigRequires = new Dictionary<TypeGround, Requiriments>();

        foreach (var cGround in part.ConfigGround)
        {
            
            if(cGround.Value==null) Debug.LogError("NO FOTIS ENSERIO ETS NULL?????");

            HashSet<Vector2Int> returnVal = new HashSet<Vector2Int>();
            foreach (var pos in cGround.Value)
            {
                Vector2Int rotated = ApplyRotation(pos, rotationToAdd);    
                returnVal.Add(new Vector2Int (gridPosition.x+rotated.x,gridPosition.z+rotated.y));
                // newConfigGround.Add(item.Key, RecalculatePositionsGridSpace(gridPosition, item.Value));
               
            }
             _updatedPart.ConfigGround.Add(cGround.Key,  returnVal);

        }

        foreach (var cRequires in _originalPart.ConfigRequires)
        {
            if(cRequires.Value.positionsToConnect==null) Debug.LogError("NO FOTIS ENSERIO ETS NULL?????");

            HashSet<Vector2Int> returnVal = new HashSet<Vector2Int>();
            foreach (var pos in cRequires.Value.positionsToConnect)
            {
                Vector2Int rotated = ApplyRotation(pos, rotationToAdd);
                returnVal.Add(new Vector2Int (gridPosition.x+rotated.x,gridPosition.z+rotated.y));
            }
            // Requiriments req=new Requiriments();
            Requiriments req=new Requiriments(returnVal,  cRequires.Value.nRequires);

            _updatedPart.ConfigRequires.Add(cRequires.Key, req);
        }
        // _updatedPart.ConfigRequires=newConfigRequires;
        Debug.Log("debugMethod final2 recalculate position: "+ _updatedPart.debugConfigGround()+ _updatedPart.debugConfigRequires());

       

    }
    #endregion

    #region Deleters

    //we could change part with idp and should be hard to do
    public void DeletePart(PlacementDataSO part)
    {
        //delete an especific part
        // placementData.DeleteNewPartData(part);

    }

    public void ResetPlacementData()
    {
        PlacementData2 newPD2=new PlacementData2();
        placementData=newPD2;
    }

    #endregion

    #region Rotation
    private Vector2Int ApplyRotation(Vector2Int pos, int rotationSteps)
    {
        // rotationSteps: 0=0°, 1=90°, 2=180°, 3=270°
        return rotationSteps switch
        {
            1 => new Vector2Int(pos.y, -pos.x),  // 90° Dreta
            2 => new Vector2Int(-pos.x, -pos.y), // 180°
            3 => new Vector2Int(-pos.y, pos.x),  // 270° (o 90° Esquerra)
            _ => pos                             // 0° o defecte
        };
    }
    #endregion

    #region debug
    public void returnDebugStrings()
    {
        Debug.Log("here it is, the final debug:"+ placementData.debugMethod());
    }

    #endregion

}

