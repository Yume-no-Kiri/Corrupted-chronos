using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    [SerializeField]
    protected Vector2Int startPos=Vector2Int.zero;
    [SerializeField]
    private int iterations=10;
    [SerializeField]
    private int walkSteps=10;
    [SerializeField]
    public bool startRandomlyEachIteration = true;

        
    [SerializeField]
    private MapVisualizer mapVisualizer;
    
    public void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions= RunRandomWalk();
        foreach (var position in floorPositions)
        {
            Debug.Log(position.ToString());
        }
        mapVisualizer.PaintFloorTile(floorPositions);
    }

    public HashSet<Vector2Int> RunRandomWalk()
    {
        var currentPos=startPos;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        //crido x random walks de llargada walksteps y iteracions 
        for (int i = 0; i < iterations; i++)
        {
            //crido random walk de walksteps passos
            var path= GenerationAlgorithms.SimpleRandomWalk(currentPos,walkSteps);
            floorPositions.UnionWith(path);
            if (startRandomlyEachIteration)
            {
                currentPos=floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }
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
