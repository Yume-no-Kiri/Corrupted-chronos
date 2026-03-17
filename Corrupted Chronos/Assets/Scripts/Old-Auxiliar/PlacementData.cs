

//cada objecte tindrà això
using System.Collections.Generic;
using UnityEngine;

//potser separar això en un script sol
#region PlacementData
public class PlacementData
{
    
    //les posicions que ocupen
    public List<Vector3Int> occupiedPositions;

    //el id de la peça
    public int ID { get; private set; }

    //això serà útil per treure peçes
    public int PlacedObjectIndex { get; private set; }

    //nau, pilot o part, tindran caracteristiques diferents i diferentes variables //what
    public ConfigurationNau type { get; private set; }

  
    
   //public TypeGround typeGround;
    
    //constructor
    public PlacementData()
    {
        occupiedPositions = new();
        ID = -1;
        PlacedObjectIndex = -1;
    }
    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
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


    public List<Vector3Int> returnSTUFF()
    {
        return occupiedPositions;
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

#endregion