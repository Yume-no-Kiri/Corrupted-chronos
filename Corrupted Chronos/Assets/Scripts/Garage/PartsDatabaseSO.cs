using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;


public enum ConfigurationNau
{
    Parts,
    Naus,
    Travelers
} 




[CreateAssetMenu(fileName = "PartsDatabaseSO", menuName = "Scriptable Objects/PartsDatabaseSO")]
public class PartsDatabaseSO : ScriptableObject
{
    public List<ObjectData> AllParts;
    public List<ObjectData> AllNaus;
    
    //els pilots son més simples, no necesitem quan ocupen
    public List<ObjectData> AllTravelers;
}

[Serializable]
public class ObjectData
{
    [field: SerializeField]
    public string Name { get; private set; }
    
    [field: SerializeField]
    public int ID { get; private set; }
    
    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;
     
    //aquest només el tindan Naus i parts, inclús haurien d'haver més, per les múltples variacions
    [field: SerializeField]
    public Vector2Int BuildSize { get; private set; } = Vector2Int.one;
    
    [field: SerializeField]
    public GameObject Prefab { get; private set; }
}