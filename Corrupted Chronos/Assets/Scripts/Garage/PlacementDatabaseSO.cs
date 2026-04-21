// Parusing System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

#region auxiliar enums and structs
public enum ConfigurationNau
{
    Parts,
    Naus,
    Pilots
} 

//informació del terra
[Serializable]
public struct SizeGround
{
    public Vector2Int Origen;
    public Vector2Int Size ;
    public List<Vector2Int> ExtraSize;

    public SizeGround(Vector2Int ori, Vector2Int size, List<Vector2Int> esize)
    {
        this.Origen=ori;
        this.Size=size;
        this.ExtraSize=esize;
    }


}
//auxiliar, usable al inspector
[Serializable]
public struct DictionaryAuxSize
{
    public TypeGround typeGround;
    public SizeGround sizeGround;

    public DictionaryAuxSize(TypeGround tground, SizeGround sground )
    {
        this.typeGround=tground;
        this.sizeGround=sground;
    }
}
[Serializable]
public struct DictionaryAuxRequires
{
    public TypeGround typeGround;
    public SizeGround sizeGround;
    public int nRequires;

     public DictionaryAuxRequires(TypeGround tground, SizeGround sground, int nrequires )
    {
        this.typeGround=tground;
        this.sizeGround=sground;
        this.nRequires=nrequires;
    }
}

public struct Requiriments
{   //i guesss I should do something more to say how we negate things, only for occupied is simple

    public HashSet<Vector2Int> positionsToConnect;
    public int nRequires;

    public Requiriments(HashSet<Vector2Int> setV2, int nR) : this()
    {
        this.positionsToConnect = setV2;
        this.nRequires = nR;
    }

}
#endregion



//probalement hauria de modificar una mica aixo, un objecte del inventari, pot ser item o pot ser part, inclús podrien ser els 2 a la vegada 
[Serializable]
public class PlacementDataSO
{
    //aquí hauriem d'afegir estats i valors de cada arma a modificar
    [field: SerializeField]
    public string Name { get; private set; }
    
    //Deberiamos estructurar como hacemos el id
    [field: SerializeField]
    public int ID { get; private set; }

    //this idp is giving when the part is added to a ship
    [HideInInspector]
    public int IDP { get; private set; }

    //diccionary auxiliar per que es configura al inspector
    [field: SerializeField]
    public List<DictionaryAuxSize> dictionaryAuxSize { get; private set; }= new List<DictionaryAuxSize>();

   //change dictionaryAuxSize if requisits become more complex
    [field: SerializeField]
    public List<DictionaryAuxRequires> dictionaryAuxRequires{ get; private set; }= new List<DictionaryAuxRequires>();

    //Dictionarys correcte que es configura amb dictionaryAuxSize, no es poden fusionar en 2, perque diccionary no es pot configurar en l'inpector
    public Dictionary<TypeGround,HashSet<Vector2Int>> ConfigGround= new Dictionary<TypeGround, HashSet<Vector2Int>>();

    public Dictionary<TypeGround,Requiriments> ConfigRequires= new Dictionary<TypeGround, Requiriments>();


    //THIS SHOULD PROBABLY BE DELETED AND USE THE ONE FROM THE ALLOBJECT INSTED
    //possible fix: 2 prefabs, un de garatge i un amb la funcionalitat en si 
    [field: SerializeField]
    public GameObject PrefabGaratge { get; private set; }

    // [field: SerializeField]
    // public GameObject PrefabJugable { get; private set; }



    #region Constructors
    //I should return to this constructor, but I see it like a very hard work, i will do it later 

    /* public  PlacementDataSO()
    {
        // "this" serveix per diferenciar la variable de la classe del paràmetre
        this.Name = "Escopeta";
        this.ID = 0;


        this.dictionaryAuxSize.Add(new DictionaryAuxSize(TypeGround.Occupied, new SizeGround(new Vector2Int(0,0), new Vector2Int(1,2), null)));
        this.dictionaryAuxSize.Add(new DictionaryAuxSize(TypeGround.Buildable, new SizeGround(new Vector2Int(0,0), new Vector2Int(3,4), null)));
        this.dictionaryAuxRequires.Add(new DictionaryAuxRequires(TypeGround.Buildable, new SizeGround(new Vector2Int(0,0), new Vector2Int(1,2), null),1));
        // Debug.Log($"S'ha creat una nova dada per: {nom}");
    } */

    #endregion

    #region Change to usable values
    /* Canvi de valors del inspector a valors hashset de sizeGround de les posicions
         */
    public void ConfigPositions()
    {
        IDP=-1;
        Debug.LogWarning("execute config positions");
        foreach (var item in dictionaryAuxSize)
        {
            if (!ConfigGround.ContainsKey(item.typeGround)){
                ConfigGround.Add(item.typeGround, CalculteAllGround(item.typeGround,item.sizeGround,true));
            }
        }
        if(dictionaryAuxRequires==default) Debug.LogWarning("EXCUSE MOI WTF");
        foreach (var item in dictionaryAuxRequires)
        {
            Debug.Log("debugMethod final2: first foreach");
            if (!ConfigRequires.ContainsKey(item.typeGround)){

                HashSet<Vector2Int> setV2= new HashSet<Vector2Int>();
                setV2 =CalculteAllGround(item.typeGround,item.sizeGround);
                foreach (var v2 in setV2)
                {
                    Debug.Log("debugMethod final2 set values"+ v2);
                }
                Requiriments req=new Requiriments(setV2, item.nRequires);
                ConfigRequires.Add(item.typeGround, req);
            }
            
        }
        // console debugConfigRequires();
    }

    HashSet<Vector2Int> CalculteAllGround(TypeGround tGround, SizeGround sGround, bool filtrateBorders=false)//DictionaryAuxSize dicAux)
    {
       
        HashSet<Vector2Int> res= new HashSet<Vector2Int>();
        int offsetX = sGround.Size.x / 2;
        int offsetY = sGround.Size.y / 2;
        
        switch(tGround){
            case TypeGround.Occupied:
                
                for (int x = 0; x < sGround.Size.x; x++)
                {
                    for (int y = 0; y < sGround.Size.y; y++)
                    {
                        res.Add(new Vector2Int(x-offsetX, y-offsetY));
                    }
                } 
                foreach (var item in sGround.ExtraSize)
                {
                    res.Add(item);
                }
            return res;
            case TypeGround.Buildable:
                Vector2Int origen= sGround.Origen;
                for (int x = 0; x < sGround.Size.x; x++)
                {
                    for (int y = 0; y < sGround.Size.y; y++)
                    {
                        //extrems
                       if(filtrateBorders)  if((x == 0 || x == sGround.Size.x - 1) && (y == 0 || y == sGround.Size.y - 1)) continue;

                        res.Add(new Vector2Int(x-offsetX+origen.x, y-offsetY+origen.y));
                    }
                } 
                foreach (var item in sGround.ExtraSize)
                {
                    res.Add(item);
                }
               
                //no tindre en compte els cantonades
            return res;
            case TypeGround.Null:
                Debug.LogError("Error terreny es null????");
            return null;
            case TypeGround.Magazine:
                Debug.LogError("No implementat Magazine");
            return null;
            case TypeGround.Muzzle:
                Debug.LogError("No implementat Muzzle");
            return null;
            case TypeGround.OverHeat:
                Debug.LogError("No implementat OverHeat");
            return null;
            default:
                Debug.LogError("WTF error default de switch typeGround");
            return null;
        }
        /* FINISH WITH
        
        dictionaryAuxRequires->ConfigRequires
          */


    }

    public void AssignIDP(int idp)
    {
        IDP=idp;
    }

    


    #endregion

    #region  debugs
    public string debugConfigGround()
    {
        if (ConfigGround == null || ConfigGround.Count == 0) return "Configuració buida";

        // 1. Primer necessitem saber els límits (Min/Max) de totes les posicions combinades
        int minX = int.MaxValue, maxX = int.MinValue;
        int minY = int.MaxValue, maxY = int.MinValue;
        
        // Diccionari auxiliar per saber què hi ha a cada coordenada ràpidament
        Dictionary<Vector2Int, string> gridMap = new Dictionary<Vector2Int, string>();

        foreach (var entry in ConfigGround)
        {
            string lletra = entry.Key == TypeGround.Occupied ? "O" : (entry.Key == TypeGround.Buildable ? "B" : "E");
            
            foreach (Vector2Int pos in entry.Value)
            {
                // Actualitzem límits
                if (pos.x < minX) minX = pos.x;
                if (pos.x > maxX) maxX = pos.x;
                if (pos.y < minY) minY = pos.y;
                if (pos.y > maxY) maxY = pos.y;

                // Omplim el mapa visual (si una cel·la té O i B, sortirà "OB")
                if (gridMap.ContainsKey(pos)) gridMap[pos] += lletra;
                else gridMap[pos] = lletra;
            }
        }

        // 2. Generem el string
        string res = "--- Preview de la Peça --- DebugMethod \n";
        
        for (int y = maxY; y >= minY; y--)
        {
            string fila = "";
            for (int x = minX; x <= maxX; x++)
            {
                Vector2Int current = new Vector2Int(x, y);
                if (gridMap.ContainsKey(current))
                {
                    // Formatem la cel·la: [X,Y: Tipus]
                    string cell = $"[{x},{y}:{gridMap[current]}]";
                    fila += cell.PadRight(12);
                }
                else
                {
                    // Cel·la buida dins dels límits
                    fila += " . ".PadRight(12);
                }
            }
            res += fila + "\n";
        }

        return res;
    }

    public string debugConfigRequires()
    {
        if (ConfigRequires == null || ConfigRequires.Count == 0) return "No hi ha requeriments definits.";

        // 1. Cercar límits (Min/Max) de totes les posicions de connexió
        int minX = int.MaxValue, maxX = int.MinValue;
        int minY = int.MaxValue, maxY = int.MinValue;
        
        // Diccionari auxiliar per mapejar coordenades -> tipus de requeriment + (mínim necessari)
        Dictionary<Vector2Int, string> reqMap = new Dictionary<Vector2Int, string>();
        // Guardarem també els nRequires per mostrar-ho a la llegenda o capçalera
        Dictionary<TypeGround, int> countMap = new Dictionary<TypeGround, int>();

        foreach (var entry in ConfigRequires)
        {
            TypeGround tipus = entry.Key;
            Requiriments req = entry.Value;
            
            string lletra = tipus == TypeGround.Occupied ? "RO" : (tipus == TypeGround.Buildable ? "RB" : "RE");
            countMap[tipus] = req.nRequires;

            if (req.positionsToConnect == null) continue;

            foreach (Vector2Int pos in req.positionsToConnect)
            {
                // Actualitzem límits
                if (pos.x < minX) minX = pos.x;
                if (pos.x > maxX) maxX = pos.x;
                if (pos.y < minY) minY = pos.y;
                if (pos.y > maxY) maxY = pos.y;

                // Omplim el mapa (si una cel·la requereix múltiples coses, les concatenem)
                if (reqMap.ContainsKey(pos)) reqMap[pos] += "/" + lletra;
                else reqMap[pos] = lletra;
            }
        }

        if (reqMap.Count == 0) return "Els requeriments no tenen posicions assignades.";

        string res = "--- DEBUG REQUERIMENTS DE LA PEÇA ---\n";
        res += "Mínims exigits: ";
        foreach(var kvp in countMap) res += $"[{kvp.Key}: {kvp.Value}] ";
        res += "\n\n";

        for (int y = maxY; y >= minY; y--)
        {
            string fila = "";
            for (int x = minX; x <= maxX; x++)
            {
                Vector2Int current = new Vector2Int(x, y);
                if (reqMap.ContainsKey(current))
                {
                    // Formatem la cel·la: [X,Y: RO] o [X,Y: RB]
                    string cell = $"[{x},{y}:{reqMap[current]}]";
                    fila += cell.PadRight(15);
                }
                else
                {
                    fila += " . ".PadRight(15);
                }
            }
            res += fila + "\n";
        }

        return res;
    }
    #endregion


}

//això estaria millor (?) si gran part de les variables les escrivís en codi i no al inspector
[CreateAssetMenu(fileName = "PlacementDatabaseSO", menuName = "Scriptable Objects/PlacementDatabaseSO")]
public class PlacementDatabaseSO : ScriptableObject
{
    public List<PlacementDataSO> AllParts;
    public List<PlacementDataSO> AllNaus;
    
    //els pilots son més simples, no necesitem quan ocupen
    public List<PlacementDataSO> AllPilots;

    //no es un monobehavior, no hi ha start, toca cridar-ho
    public void StartConfigPositions()
    {
        Debug.LogWarning("ENTREM A CONFIG POSITONS" + AllParts.Count);
        foreach (var item in AllParts)
        {
            Debug.LogWarning("name");
            item.ConfigPositions();
            Debug.Log("DebugMethod final2 creació del configRequires: "+ item.debugConfigRequires());
        }
        foreach (var item in AllNaus)
        {
            item.ConfigPositions();
        }
        Debug.LogWarning("ENDED CONFIG POSITIONS");
    }

    public PlacementDataSO ReturnPartDataById(int ID)
    {
        return AllParts[ID];
    }



}
