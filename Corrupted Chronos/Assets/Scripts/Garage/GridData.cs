using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



//aquí afegir més tipus com: punta cano
public enum TypeGround
{
    Null,
    Buildable,
    Occupied
} 

public class GroundData
{
    public PlacementData PlacementData;
    public TypeGround TypeGround;

    public GroundData(PlacementData placementData, TypeGround typeGround)
    {
        PlacementData = placementData;
        TypeGround = typeGround;
    }
}

public class GridData
{
    Dictionary<Vector3Int, GroundData> placedObjects = new();

    #region afegir objectes al mapa
    //afegir un objecte
    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, Vector2Int buildableSize,  int ID, int placedObjectIndex)
    {
        Debug.Log("______________________new objct____________________");
        List<Vector3Int> positionWillOccupy = CalculatePositionsWillOccupy(gridPosition, objectSize);
        List<Vector3Int> positionWillBuildable= CalculatePositionsWillBuildable(gridPosition, objectSize, buildableSize);
        PlacementData dataOccupy = new PlacementData(positionWillOccupy, ID, placedObjectIndex);
        
        //PlacementData dataBuild = new PlacementData(positionWillBuildable);
        
        
        bool canBuild = false;
        foreach (var pos in positionWillOccupy)
        {
            Debug.Log($"{pos.ToString()}");
            //fes throw, perque per arribar aquí ja s'ha comprovat que es pot
            if (IsOcupiedAt(pos)){
                throw new Exception($"Intentes col·locar algo on ja hi ha algo {pos}");
            }
            AddOrModifyFirstElement(pos, dataOccupy);

            /*if (!esPrimeraPeca)
            {
                if (isBuildableAt(pos))
                {
                    canBuild = true;
                }
            }*/

        }
        //si cap posició occupied es buildable, estem col·locant algo on no hauries
        //if (!esPrimeraPeca) if (!canBuild) throw new Exception("Intentes col·locar algo on no tens permes construir ");

        
        //això està malament, hauriem de comprovar si alguna de les superficies que s'ocuparan es buildable, aquío només definim 
        //data de les que amplien com a buildable
        //BUGFIX
        var buildableCopy = new List<Vector3Int>(positionWillBuildable);
        foreach (var pos in buildableCopy)
        {
            Debug.Log($"{pos.ToString()} will build 1");
            //AddOrModify(pos,dataBuild);
            AddOrModifySecondElement(pos);
        }

    }

    /*occupied + occupied =:( 
     * occupied + buildable= :) occupied
     * buildable + occupied = :) occupied
     * buildable + buildable= :) buildable
     */
    private void AddOrModifySecondElement(Vector3Int pos)
    {
        if (!placedObjects.ContainsKey(pos))
        {
            GroundData newGD = new GroundData(new PlacementData(), TypeGround.Buildable);
            placedObjects[pos] = newGD;
        }
        
    
    }

    //el primer element a construir es la nau
    //mètode per build i occupy
    public void AddOrModifyFirstElement(Vector3Int pos, PlacementData dataUpdate)
    {
        //if (!placedObjects.ContainsKey(pos)){
            //no tenim informació en aquesta posició
            //(PlacementData, TypeGround) 
            GroundData newGD = new GroundData(dataUpdate, TypeGround.Occupied);
            placedObjects[pos] = newGD;
            //Debug.Log($"{pos.ToString()} assigned 2");

        //}
        /*else
        {
            //actualitzem informació de la posició 
            GroundData newGD = new GroundData(dataUpdate, TypeGround.Occupied);

            placedObjects[pos]= newGD;
            //Debug.Log($"{pos.ToString()} assigned modify 2");

            //PlacementData updatedData = placedObjects[pos].UpdateData(placedObjects[pos], dataOccupy);
            //placedObjects[pos]= updatedData;
        }*/
    }
    
    #endregion
    #region calcul occupied o buildable
    
    //calcular totes posicions del objecte que ocuapran
    private List<Vector3Int> CalculatePositionsWillOccupy(Vector3Int gridPosition, Vector2Int objectSize)
    {
        //si es vol afegir rotació algo a fer aquí 
        List<Vector3Int> returnVal = new();
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        
        return returnVal;
    }

    //calcular totes les posicions que deixarà buildable un objecte
    private List<Vector3Int> CalculatePositionsWillBuildable(Vector3Int gridPosition, Vector2Int objectSize,
        Vector2Int buildableSize)
    {
        Vector2Int overSize= buildableSize- objectSize ;
        overSize= new Vector2Int(Mathf.Abs(overSize.x),Mathf.Abs(overSize.y));
        //podria ser que diferents objectes tinguin diferents mètodes per calcular l'espai que deixen poder construïr 
        List<Vector3Int> returnVal = new();
        for (int x = 0-overSize.x; x < objectSize.x+overSize.x; x++)
        {
            for (int y = 0-overSize.y; y < objectSize.y+overSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;

        /*
        List<Vector3Int> returnVal = new();
        for (int x = objectSize.x; x < buildableSize.x; x++)
        {
            for (int y = objectSize.y; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        for (int x = 0; x > -buildableSize.x; x--)
        {
            for (int y = 0; y > -objectSize.y; y--)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;
        */
    }

    #endregion

    #region CanPlaceObject
    //comprovar si afegir object
    //comprovar occupied i si no es buildable
    public bool CanPlaceObejctAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        //veure si té alguna peça en buildable
        bool r = false, occuped = false;
        bool canBuild = false;
        List<Vector3Int> positionsToOccupy = CalculatePositionsWillOccupy(gridPosition, objectSize);
        foreach (var pos in positionsToOccupy)
        {
            if (IsOcupiedAt(pos))
            {
                occuped = true;
               //r= false;
               break;
            }
            
            if (isBuildableAt(pos))
            {
                canBuild = true;
            }
            
        }
        if (!occuped && canBuild) r=true;
        
        /*List<Vector3Int> positionsToBuild =CalculatePositionsWillBuildable(gridPosition, objectSize, buildableSize);
        foreach (var pos in positionsToBuild)
        {
            
        }*/
        
        return r;
    }
    public int CanPlaceObejctAt2(Vector3Int gridPosition, Vector2Int objectSize)
    {
        //veure si té alguna peça en buildable
        int r = 0;
        bool occuped = false;
        bool canBuild = false;
        List<Vector3Int> positionsToOccupy = CalculatePositionsWillOccupy(gridPosition, objectSize);
        foreach (var pos in positionsToOccupy)
        {
            if (IsOcupiedAt(pos))
            {
                occuped = true;
                //r= false;
                break;
            }
            
            if (isBuildableAt(pos))
            {
                canBuild = true;
            }
            
        }
        if (!occuped && canBuild) r=1; //green
        else if (occuped && canBuild) r = 3; //red
        else if (!occuped && !canBuild) r = 2; //blue
        else if(!occuped && !canBuild) r = 4;
        
        /*List<Vector3Int> positionsToBuild =CalculatePositionsWillBuildable(gridPosition, objectSize, buildableSize);
        foreach (var pos in positionsToBuild)
        {

        }*/
        
        return r;
    }

    #endregion

    #region si es occupied o buildable
    private bool IsOcupiedAt(Vector3Int pos)
    {
        bool r = false;
        if (placedObjects.TryGetValue(pos, out var data))
        {
            if (data.TypeGround==TypeGround.Occupied) r = true;
        }
        return r;
    }

    private bool isBuildableAt(Vector3Int pos)
    {
        bool r = false;
        if (placedObjects.TryGetValue(pos, out var data))
        {
            if (data.TypeGround==TypeGround.Buildable) r = true;
        }
        return r;
    }
    
    #endregion
}

