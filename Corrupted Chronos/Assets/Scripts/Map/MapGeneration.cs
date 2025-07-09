using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    [SerializeField]
    protected Vector3Int startPos=Vector3Int.zero;
    [SerializeField]
    private int iterations=20;
    [SerializeField]
    private int walkSteps=20;
    [SerializeField]
    public bool startRandomlyEachIteration = true;

        
    [SerializeField]
    private MapVisualizer mapVisualizer;
    
    
    
    public void RunProceduralGeneration()
    {
        WalkedBy floorPositions= RunRandomWalk();
        //HashSet<Vector3Int> floorPositions= RunRandomWalk();
        /*foreach (var position in floorPositions.Path)
        {
            Debug.Log(position.ToString() +"AAAAA");
        }*/
        mapVisualizer.PaintFloorTile(floorPositions);
    }

    //public HashSet<Vector3Int> RunRandomWalk()
    public WalkedBy RunRandomWalk()
    {
        Debug.Log("Go in IN");

        //HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();
        WalkedBy floorPositions = new WalkedBy(1);
        
        // for (int j = 1; j < 3; j++)
        //{
            //iterations += j;
            //walkSteps += j;
            Debug.Log("Go in");

            var currentPos = startPos;
            //crido x random walks de llargada walksteps y iteracions 
            for (int i = 0; i < iterations; i++)
            {
                //crido random walk de walksteps passos
                WalkedBy path = GenerationAlgorithms.SimpleRandomWalk(currentPos, walkSteps, Random.Range(0, 4));
                floorPositions.Path.UnionWith(path.Path);
                floorPositions.Corners.UnionWith(path.Corners);
                GenerationAlgorithms.UnionDictionaries(floorPositions.Occupied, path.Occupied);
                Debug.Log("Go between1" + i);
                if (startRandomlyEachIteration)
                {
                    currentPos = floorPositions.Corners.ElementAt(Random.Range(0, floorPositions.Corners.Count));
                }
                Debug.Log("Go between2" + i);
            }
        //}
        Debug.Log("Go out");
        return floorPositions;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RunProceduralGeneration();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


public struct WalkedBy
{
    public HashSet<Vector3Int> Path;
    public HashSet<Vector3Int> Corners;
    public Dictionary<Vector3, int> Occupied;

     public WalkedBy(int a)
     {
         Path = new HashSet<Vector3Int>();
         Corners = new HashSet<Vector3Int>();
         Occupied = new Dictionary<Vector3, int>();
     }
    public WalkedBy(HashSet<Vector3Int> floor, HashSet<Vector3Int> corner, Dictionary<Vector3, int> occupied)
    {
        Path=floor;
        Corners=corner;
        Occupied=occupied;
    }
}

