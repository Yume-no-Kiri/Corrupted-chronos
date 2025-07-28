using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



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
    

}

//cada objecte tindrà això
public class PlacementData
{
    //les posicions que ocupen
    public List<Vector3Int> occupiedPositions;

    //el id de la peça
    public int ID { get; private set; }

    //això serà útil per treure peçes
    public int PlacedObjectIndex { get; private set; }

    //nau, pilot o part, tindran caracteristiques diferents i diferentes variables
    public ConfigurationNau type { get; private set; }

    //si és una superficie on es pot construir
    //ja que una posició pot ser buildable per múltiple occupied, una buildable pot tindre informació donada de múltplies occupied objects
    //list pot tindre duplicats, per no trencar-nos el cap, i tenint en compte que això probablement es refactoritzi, ho deixo en una simple llista
    //public List<Vector3Int> buildablePositions;
    //edit després de trencarme el cap:
    //AQUESTA NO ES BONA MANERA, demoment ho guardo com un segon valor del diccionari, quan vulgui borrar coses veure que faig, pero vull acabar això per la
    //demo o acabaré en un hospital psiquiatric 
    
   //public TypeGround typeGround;
    
    //constructor
    public PlacementData()
    {
        occupiedPositions = new();
        ID = -1;
        PlacedObjectIndex = -1;
    }
    public PlacementData(List<Vector3Int> occupiedPositions, int iD,
        int placedObjectIndex)
    {
        //Data Occupied
        //si té un element a sobre, ja no es buildable, és occupied
        //this.buildablePositions = new List<Vector3Int>();
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
        //typeGround = TypeGround.Occupied;
    }

    public PlacementData( List<Vector3Int> buildablePositions){
        //Data Buildable
        //si elements son inicialitzats, no s'han assignat
        //this.buildablePositions = buildablePositions;
        occupiedPositions = new List<Vector3Int>();
        ID = -1;
        PlacedObjectIndex = -1;
        //typeGround = TypeGround.Buildable;
    }

    /*Oh boy, gran explicació aquí
     *
     * IRRELEVANT
     * si estiguessim fent un sistema més habitual, voldriem que cada part o és buildable o occupied, pero no els dos, PERÒ ja que volem afegir peçes per sobre
     * en algun moment, com cables o x, podem fer que cada quadrat pugui ser dels 2. Un benefici d'aquest sistema, quan una la canviem no hem d'actualitza totes les posicions que continguin
     * informació sobre aquella casella. 
     * Llavors SEMPRE comprovem primer, que estigui occupied, ja que podem entendre que occupied té més importancia a sobre de
     *buildable.
     *IRRELEVANT
     *
     * Hauria d'haverme fet cas ;-;
     * Potser això es podria representar en un altre tipo de data dintre del diccionari, estil Dictionary<Vector3Int, pair<PlacementData,PlacementData>> placedObjects 
     * on a un PlacementData col·loquem objectes col·locables, cables, parts, i el segon PlacementData, seria informació al respecte de grups de caselles,
     * grups buildables o no, si hi ha una casella especial, que otorga atributs aespecials als objectes en ella, i informació que determinem que podem col·locar al primer PlacementData.
     */
    
    /*occupied + occupied =:(
     * occupied + buildable= :)
     * buildable + occupied = :)
     * buildable + buildable= :)
     */
    
    //SI ACABO FENT UN PLACEMENTDATA Per el ground això es útil
    /*public void UpdateData( PlacementData newData)
    {
         
        //occupied + buildable= :) occupied
        if(typeGround==TypeGround.Occupied && newData.typeGround == TypeGround.Buildable)
        {
            buildablePositions = newData.buildablePositions;
            //occupied positions igual
            //id igual
            //placedObjectIndex igual
            //typeGround = TypeGround.Occupied ;
        }
        
        //buildable + occupied = :) occupied
        else if (typeGround==TypeGround.Buildable && newData.typeGround==TypeGround.Occupied)
        {
            //buildablePositions es queda igual
            occupiedPositions = newData.occupiedPositions;
            ID = newData.ID;
            PlacedObjectIndex = newData.PlacedObjectIndex;
            typeGround = TypeGround.Occupied ;
        }
        
        // buildable + buildable= :)
        else if (typeGround==TypeGround.Buildable && newData.typeGround==TypeGround.Buildable)
        {
            buildablePositions.AddRange(newData.buildablePositions);
            //occupied positions igual
            //id igual
            //placedObjectIndex igual
            
        } //occupied + occupied =:(
        else if (typeGround==TypeGround.Occupied && newData.typeGround==TypeGround.Occupied)
        {
            if (occupiedPositions == newData.occupiedPositions)
            {
                Debug.LogWarning("YES, WE ARE APPROACHING SOMETHING");
            }
            else
            {
                Debug.LogError("Intent de sumar 2 occupied positions, GridData.cs");
            }
        }
        
        */
        
    
    
}