using System;
using System.Collections.Generic;
using System.Linq;
using Ink.Parsed;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;





public struct PriorityTGround
{
    public Dictionary<TypeGround, int> PriorityByTGround;

    public PriorityTGround(Dictionary<TypeGround, int> priTGround)
    {
        PriorityByTGround= priTGround;
    }
    public static PriorityTGround CreateDefault()
    {
        PriorityTGround res= new PriorityTGround();
        res.PriorityByTGround = new Dictionary<TypeGround, int>();
        // PriorityByTGround =new();
        // if(PriorityByTGround.Count!=0) return;
        foreach (TypeGround tGround in Enum.GetValues(typeof(TypeGround)))
        {
            switch (tGround)
            {
                case TypeGround.Occupied:
                res.PriorityByTGround.Add(TypeGround.Occupied, 2);  
                continue;
                case TypeGround.Buildable:
                res.PriorityByTGround.Add(TypeGround.Buildable, 1);  
                continue;
                case TypeGround.Magazine:
                Debug.LogWarning("No definit");
                continue;
                default:
                Debug.LogWarning("No definit");
                continue;
            }   
        }
        return res;
    }

    public int returnPriority(TypeGround tGround)
    {
        return PriorityByTGround[tGround];
    }
}

struct Node
{
   public int myIDP;
   public int profundity;
   public List<int> connections;


    public Node FirstStructure(int idp)
    {
        Node node;
        node.myIDP=idp;
        node.profundity=0;
        node.connections=new List<int>();
        return node;
    }
}

public class PlacementData2
{

    Dictionary<int, PlacementDataSO> AllAddedParts= new();


    //I should make something to relate parts with parts, nodes with nodes, so deleting a node should only be on the extremes, and it could also be used to detect influence, probably a struct
    // ?? PositionInfluence/ NodeTreeParts
    // Dictionary<int, Node> TreeParts= new();

    private Dictionary<Vector2Int, List<TypeGround>> AllPositionsTGround = new();

    //position and what it is as a result, be carefull about this one, change it/delete it if needed
    public Dictionary<Vector2Int, TypeGround> FinalPositionTGround = new();

    
    //there should be a better idp creator, but this could work atm

   
   private PriorityTGround _priorityTGround=new PriorityTGround();
    // private Dictionary<TypeGround, int> PriorityByTGround =new();
    

    public TypeGround  returnFinalTypeGround(Vector2Int pos)
    {
        if (FinalPositionTGround.ContainsKey(pos))
        {
            return FinalPositionTGround[pos];
        }else return TypeGround.Null;
        
    }

    #region Requirements
    public bool checkRequirement(Dictionary<TypeGround,Requiriments> ConfigRequires)
    {
        if (_priorityTGround.PriorityByTGround == null || _priorityTGround.PriorityByTGround.Count()==0) {
            _priorityTGround = PriorityTGround.CreateDefault();
        }
        if (ConfigRequires == null || ConfigRequires.Count == 0) {
            Debug.LogWarning("possible error configRequires null");
            return true;
        }

        

        // bool res=false;
        Debug.Log("debugMethod final2 requeriments de part a afegir"+ debugMethodRequirements(ConfigRequires));
        foreach (var ground in ConfigRequires)
        {
            bool currentReq=false;
            switch (ground.Key)
            {
                case TypeGround.Occupied:
                //some positions should be occupied
                currentReq=PermitiveRequirement( ground.Value,TypeGround.Occupied );
                // return res;
                break;
                //some positions should be buildable
                case TypeGround.Buildable:
                // Debug.LogError("entrem a requirment de buildable");
                Debug.Log("debugMethod final2 how is the scene"+debugMethod());

                currentReq=RestrictiveRequirement( ground.Value,TypeGround.Buildable );
                // return res;
                break;

                default:
                Debug.LogError("No implementat o algo estrany");
                // return res;
                return false;

            }
            if(!currentReq)
            {
                return false;
            }

        }
     
        return true;
    }

    //el standard requirement: tgroundToCompare is in the positions of positionsToConnect at least nRequires
    //very freedom requirement, not usefull for a lot of parts
    private bool PermitiveRequirement(  Requiriments ground, TypeGround tgroundToCompare)
    {
        bool res=false;
        int nCompleted=0;
        int minimCompleted= ground.nRequires;
        HashSet<Vector2Int> positionsToConnect= ground.positionsToConnect;

        foreach (var pos in positionsToConnect)
        {
            // if(nCompleted>=minimCompleted) break;
            if (FinalPositionTGround.ContainsKey( pos))
            {
                if (FinalPositionTGround[pos] == tgroundToCompare)
                {
                    nCompleted++;
                }
               
                
            }
        }
        if(nCompleted>=minimCompleted) res=true;
        else res=false;
        return res;
    }

    //el restrictive requirement: tgroundToCompare is in the positions of positionsToConnect al least nRequires, but if the someposition of positionsToConnect is in a position with finalground more requirment, it is dennied
    //
    private bool RestrictiveRequirement(  Requiriments ground, TypeGround tgroundToCompare)
    {
        // bool res=false;
        // bool exclude=false;
        int nCompleted=0;
        int minimCompleted= ground.nRequires;
        HashSet<Vector2Int> positionsToConnect= ground.positionsToConnect;

        foreach (var pos in positionsToConnect)
        {
            // if(nCompleted>=minimCompleted || exclude) break;
            if (FinalPositionTGround.ContainsKey( pos))
            {
                if (FinalPositionTGround[pos] == tgroundToCompare)
                {
                    nCompleted++;
                }else if (_priorityTGround.returnPriority( FinalPositionTGround[pos]) > _priorityTGround.returnPriority(tgroundToCompare))
                {
                   return false;
                }
                
                
            }
        }
        return nCompleted>=minimCompleted;
    }

    #endregion

    public void AddNewPartData(PlacementDataSO part,int newIDP)
    {
        part.AssignIDP(newIDP);
        if (_priorityTGround.PriorityByTGround == null || _priorityTGround.PriorityByTGround.Count()==0) {
            _priorityTGround = PriorityTGround.CreateDefault();
        }

        Debug.LogWarning("entrem a addnewpartdata");
        //create id and relationate objectdata
        /*  int thisIdp=GameManager.Instance.GiveNextIdp(); //_givenIDP++;
         AllAddedParts.Add(thisIdp,part);
         part.AssignIDP(thisIdp);
         //also creates inventory


         //should add thisIdp to information of the part
  */
        if (AllAddedParts.ContainsKey(part.IDP))
        {
            if (AllAddedParts[part.IDP] != part)
            {
                Debug.LogError(" DIFERENT PART AMB MATEIXA IDP");
            }
            else
            {
                Debug.LogError("mateix idp diferent part??????");
            }
        }else{
            AllAddedParts.Add(part.IDP,part);
        }//complete allPositionground with configGround
        Debug.LogWarning("before configGround");

        var configGround= part.ConfigGround;
        Debug.LogWarning("after configGround, before foreach");

        foreach (var ground in configGround)
        {
            foreach (var pos in ground.Value)
            {
                if (AllPositionsTGround.ContainsKey(pos))
                {
                    AllPositionsTGround[pos].Add(ground.Key);
                }
                else
                {
                    List<TypeGround> tground= new List<TypeGround>();
                    tground.Add(ground.Key);
                    AllPositionsTGround.Add(pos,tground );
                }
            }
        }
        Debug.LogWarning("sortim a addnewpartdata");

        //Recalculate positionGround
         // ConstructorPriorityTGround();
        RecalculateFinalPositionTGround();
    }

    //not actually implemented, it could be part o idp
    public void DeleteNewPartData(PlacementDataSO part)
    {
        //obtain id with objectdata
        int thisIDP;
        if (AllAddedParts.Keys.Contains(part.IDP))
        {
            thisIDP=part.IDP;
            if(thisIDP==-1){
                Debug.LogError("IMPOSSIBLE, you shouldn't be here"); 
                return;
            }

            AllAddedParts.Remove(thisIDP);
        }

        ///////////erase positionInfluence with ConfigGround/id ;
        

        //erase allPositionground with configGround/id
        var configGround= part.ConfigGround;
        foreach (var ground in configGround)
        {
            foreach (var pos in ground.Value)
            {
                if (AllPositionsTGround.ContainsKey(pos))
                {
                    AllPositionsTGround[pos].Remove(ground.Key);
                }
                else
                {
                    //this means we don't have a seved the ground we should delete
                    Debug.LogError("this is probably an error but IDK how but it is");
                    // HashSet<TypeGround> tground= new HashSet<TypeGround>();
                    /* tground.Add(ground.Key);
                    AllPositionsTGround.Add(pos,tground ); */
                }
            }
        }


        

        //Recalculate positionGround
        RecalculateFinalPositionTGround();
    }

    private void RecalculateFinalPositionTGround()
    {
        Dictionary<Vector2Int, TypeGround> newFinalPositionTGround=new();
        foreach (var pos in AllPositionsTGround)
        {
            TypeGround tGroundAct= TypeGround.Null;
            int gPointsAct=0;
            foreach (var ground in pos.Value)
            {
                if(_priorityTGround.returnPriority(ground) >= gPointsAct){
                    tGroundAct=ground;
                    gPointsAct=_priorityTGround.returnPriority(ground);
                }
            }

            newFinalPositionTGround.Add(pos.Key, tGroundAct);
            // FinalPositionTGround[pos]=maxPriority;
        }
        FinalPositionTGround=newFinalPositionTGround;

    }


    #region debugMethod
    public string debugMethod()
    {
     

        if (AllPositionsTGround == null || AllPositionsTGround.Count == 0) 
            return "Grid buit";

        // 1. Busquem els límits del mapa per saber què dibuixar
        int minX = int.MaxValue, maxX = int.MinValue;
        int minY = int.MaxValue, maxY = int.MinValue;

        foreach (var pos in AllPositionsTGround.Keys)
        {
            if (pos.x < minX) minX = pos.x;
            if (pos.x > maxX) maxX = pos.x;
            if (pos.y < minY) minY = pos.y;
            if (pos.y > maxY) maxY = pos.y;
        }

        string res = "Visualització del Grid, debugMethod:\n";

        // 2. Recorrem des del Y màxim al Y mínim (de dalt a baix)
        for (int y = maxY; y >= minY; y--)
        {
            string fila = "";
            // 3. Recorrem de X mínim a X màxim (d'esquerra a dreta)
            for (int x = minX; x <= maxX; x++)
            {
                Vector2Int posActual = new Vector2Int(x, y);

                if (AllPositionsTGround.ContainsKey(posActual))
                {
                    string cell = "[" + x + "," + y + ": ";
                    foreach (var item in AllPositionsTGround[posActual])
                    {
                        switch (item)
                        {
                            case TypeGround.Occupied: cell += "O"; break;
                            case TypeGround.Buildable: cell += "B"; break;
                            default: cell += "E"; break;
                        }
                    }
                    cell += "]";
                    // Ajustem l'espaiat perquè totes les columnes ocupin el mateix (p.ex. 12 caràcters)
                    fila += cell.PadRight(12); 
                }
                else
                {
                    // Si la posició no existeix, posem espais buits per mantenir l'alineació
                    fila += " . ".PadRight(12);
                }
            }
            res += fila + "\n";
        }

        return res;
    } 



    public string debugMethodRequirements(Dictionary<TypeGround, Requiriments> configRequires)
    {
        if (configRequires == null || configRequires.Count == 0) return "No hi ha requeriments per mostrar.";

        // 1. Recollim totes les posicions de la nau actual i del requeriment
        HashSet<Vector2Int> allKeys = new HashSet<Vector2Int>(AllPositionsTGround.Keys);
        foreach (var req in configRequires.Values)
        {
            allKeys.UnionWith(req.positionsToConnect);
        }

        if (allKeys.Count == 0) return "Grid i Requeriments buits.";

        // 2. Trobem els límits per al dibuix
        int minX = int.MaxValue, maxX = int.MinValue;
        int minY = int.MaxValue, maxY = int.MinValue;

        foreach (var pos in allKeys)
        {
            if (pos.x < minX) minX = pos.x;
            if (pos.x > maxX) maxX = pos.x;
            if (pos.y < minY) minY = pos.y;
            if (pos.y > maxY) maxY = pos.y;
        }

        string res = "--- DEBUG DE REQUERIMENTS ---\n";
        res += "Llegenda: [O]=Nau Ocupada, [B]=Nau Buildable, [?]=On la peça BUSCA, [!]=MATCH!\n\n";

        for (int y = maxY; y >= minY; y--)
        {
            string fila = "";
            for (int x = minX; x <= maxX; x++)
            {
                Vector2Int current = new Vector2Int(x, y);
                bool isInGrid = AllPositionsTGround.ContainsKey(current);
                bool isInReq = false;
                
                // Mirem si aquesta posició és buscada per algun requeriment
                foreach (var req in configRequires.Values)
                {
                    if (req.positionsToConnect.Contains(current)) isInReq = true;
                }

                string cell = "";
                if (isInGrid && isInReq) cell = "[!]"; // Requeriment sobreposat correctament a la nau
                else if (isInReq) cell = "[?]";        // La peça busca aquí, però la nau no hi és
                else if (isInGrid)                     // Aquí hi ha nau, però la peça no mira res
                {
                    var types = AllPositionsTGround[current];
                    cell = types.Contains(TypeGround.Occupied) ? "[O]" : "[B]";
                }
                else cell = " . ";

                fila += cell.PadRight(6);
            }
            res += fila + "\n";
        }
        return res;
    }
    #endregion
}
