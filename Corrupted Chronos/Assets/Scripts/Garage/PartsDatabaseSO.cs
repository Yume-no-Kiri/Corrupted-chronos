using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;


public enum ConfigurationNau
{
    Parts,
    Naus,
    Pilots
} 
[Serializable]
public struct SizeGround
{
    public Vector2Int Size ;
    public List<Vector2Int> ExtraSize;

}
[Serializable]
public struct DictionaryAuxSize
{
    public TypeGround typeGround;
    public SizeGround sizeGround;
}

/* Potser canviar això una mica, perque objectData es bastant basica.
Revisar com es fa ara, pero per les parts això és massa general */

[CreateAssetMenu(fileName = "PartsDatabaseSO", menuName = "Scriptable Objects/PartsDatabaseSO")]
public class PartsDatabaseSO : ScriptableObject
{
    public List<ObjectData> AllParts;
    public List<ObjectData> AllNaus;
    
    //els pilots son més simples, no necesitem quan ocupen
    public List<ObjectData> AllPilots;
}

[Serializable]
public class ObjectData
{
    [field: SerializeField]
    public string Name { get; private set; }
    
    //Deberiamos estructurar como hacemos el id
    [field: SerializeField]
    public int ID { get; private set; }
    

    //probablemente informacion de posicionamiento como un script solo
    /* [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one; */

    

    [field: SerializeField]
    public List<DictionaryAuxSize> dictionaryAuxSize { get; private set; }= new List<DictionaryAuxSize>();


    //Dictionarys no es poden configurar al inspector, així que fem una mica de parafermalia
    public Dictionary<TypeGround,HashSet<Vector2Int>> ConfigGround;



    // [field: SerializeField]

    // public List<Vector2Int> Size { get; private set; } = new List<Vector2Int>();

    //aquest només el tindan Naus i parts, inclús haurien d'haver més, per les múltples variacions
    // [field: SerializeField]
    
    // public Vector2Int BuildSize { get; private set; } = Vector2Int.one;
    // [field: SerializeField]

    // public List<Vector2Int> BuildSize { get; private set; } = new List<Vector2Int>();
    
 

    //2 prefabs probablment un de garatge i un amb la funcionalitat en si 
    [field: SerializeField]
    public GameObject PrefabGaratge { get; private set; }

    // [field: SerializeField]
    // public GameObject PrefabJugable { get; private set; }


    void Awake()
    {
        foreach (var item in dictionaryAuxSize)
        {
            if (!ConfigGround.ContainsKey(item.typeGround)){
                ConfigGround.Add(item.typeGround, CalculteAllGround(item));
            }
        }
    }

    HashSet<Vector2Int> CalculteAllGround(DictionaryAuxSize dicAux)
    {
        /* Fer el calcul, passar de sizeGround a hashset de les posicions
         */
        switch(dicAux.typeGround){
            case TypeGround.Occupied:
            
            return null;
            case TypeGround.Buildable:

            return null;
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

    }
}