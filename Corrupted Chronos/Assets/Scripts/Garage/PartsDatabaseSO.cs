using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ConfigurationNau
{
    Parts,
    Naus,
    Pilots
} 




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

    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;
    //aquest només el tindan Naus i parts, inclús haurien d'haver més, per les múltples variacions
    [field: SerializeField]
    public Vector2Int BuildSize { get; private set; } = Vector2Int.one;
    
 

    //2 prefabs probablment un de garatge i un amb la funcionalitat en si 
    [field: SerializeField]
    public GameObject Prefab { get; private set; }
}