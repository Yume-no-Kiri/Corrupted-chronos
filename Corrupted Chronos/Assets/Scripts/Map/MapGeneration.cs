using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    [SerializeField]
    protected Vector3Int startPos=Vector3Int.zero;
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
        HashSet<Vector3Int> floorPositions= RunRandomWalk();
        foreach (var position in floorPositions)
        {
            Debug.Log(position.ToString() +"AAAAA");
        }
        mapVisualizer.PaintFloorTile(floorPositions);
    }

    public HashSet<Vector3Int> RunRandomWalk()
    {
        HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();

        for (int j = 1; j < 3; j++)
        {
            //iterations += j;
            //walkSteps += j;

            var currentPos = startPos;
            //crido x random walks de llargada walksteps y iteracions 
            for (int i = 0; i < 4; i++)
            {
                //crido random walk de walksteps passos
                var path = GenerationAlgorithms.SimpleRandomWalk(currentPos, walkSteps, j);
                floorPositions.UnionWith(path);
                if (startRandomlyEachIteration)
                {
                    currentPos = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
                }
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
