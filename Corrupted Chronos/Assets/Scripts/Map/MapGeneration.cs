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
    private int heightVariety=4;
    [SerializeField]
    private int occuSpace=5;
    [SerializeField]
    public bool startRandomlyEachIteration = true;

        
    [SerializeField]
    private MapVisualizer mapVisualizer;
    
    
    
    public void RunProceduralGeneration()
    {
        WalkedBy floorPositions= RunMultipleRandomWalks();
        //HashSet<Vector3Int> floorPositions= RunRandomWalk();
        /*foreach (var position in floorPositions.Path)
        {
            Debug.Log(position.ToString() +"AAAAA");
        }*/
        mapVisualizer.PaintFloorTile(floorPositions);
    }

    //public HashSet<Vector3Int> RunRandomWalk()
    public WalkedBy RunMultipleRandomWalks()
    {
        Debug.Log("Go in IN");

        //varaibles
        WalkedBy floorPositions = new WalkedBy(1);
        
        
        // for (int j = 1; j < 3; j++)
        //{
            //iterations += j;
            //walkSteps += j;
            Debug.Log("Go in");

            //var currentPos = startPos;
            var currentPos0 = GenerationAlgorithms.startPosToRW();
            var currentPos1 = GenerationAlgorithms.startPosToRW();
            //crido x random walks de llargada walksteps y iteracions 
            for (int i = 0; i < iterations; i++)
            {
                WalkedBy rw= new WalkedBy(1);


                for (int j = 0; j <iterations; j++)
                {
                    RunRandomWalk(rw, currentPos0, walkSteps, heightVariety-i-j, occuSpace, floorPositions);
                    //RunRandomWalk(rw, currentPos1, walkSteps, heightVariety-i-j, occuSpace, floorPositions);
                    
                }
                //RunRandomWalk(rw, currentPos, walkSteps, heightVariety-i, occuSpace, floorPositions);
                //RunRandomWalk(rw, currentPos, walkSteps, heightVariety-i, occuSpace, floorPositions);
                
                
                
                Debug.Log("Go between1" + i);
                if (startRandomlyEachIteration)
                {
                    currentPos0 = floorPositions.Corners.ElementAt(Random.Range(0, floorPositions.Corners.Count));
                    currentPos1 = floorPositions.Corners.ElementAt(Random.Range(0, floorPositions.Corners.Count));
                }
                Debug.Log("Go between2" + i);
            }
        //}
        Debug.Log("Go out");
        return floorPositions;
    }

    private void RunRandomWalk(WalkedBy rw, Vector3Int cpos, int ws, int h, int oc, WalkedBy fp)
    {
        
        //crido random walk de walksteps passos
        rw = GenerationAlgorithms.SimpleRandomWalk(cpos, ws, h,oc, fp);
                
        //unió amb la resta de rw
        fp.Path.UnionWith(rw.Path);
        fp.Corners.UnionWith(rw.Corners);
        fp.Occupied.UnionWith(rw.Occupied);
        GenerationAlgorithms.UnionDictionaries(fp.InfoBlock, rw.InfoBlock);
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
    /// <EXPLICACIÓ>
    /// Path es el terreny creat per el random walk,inclou els corners
    /// Corners son els bordes del random walk
    /// Occupied son els blocs que es "reserven", per a que altres random walks no hi puguin entrar
    /// InfoBlock, actualment serveix per fer el calcul i segons el int saber DINAMICAMENT, que es border i que no, en un futur per podria substituïr amb informació general sobre cada block
    /// <EXPLICACIÓ>
    public HashSet<Vector3Int> Path;
    public HashSet<Vector3Int> Corners;
    public HashSet<Vector3Int> Occupied;
    public Dictionary<Vector3, int> InfoBlock;
    
     public WalkedBy(int a)
     {
         Path = new HashSet<Vector3Int>();
         Corners = new HashSet<Vector3Int>();
         InfoBlock = new Dictionary<Vector3, int>();
         Occupied=new HashSet<Vector3Int>();
     }
    public WalkedBy(HashSet<Vector3Int> floor, HashSet<Vector3Int> corner, Dictionary<Vector3, int> infoBlock, HashSet<Vector3Int> occupied)
    {
        Path=floor;
        Corners=corner;
        InfoBlock=infoBlock;
        Occupied=occupied;
    }
    
}

