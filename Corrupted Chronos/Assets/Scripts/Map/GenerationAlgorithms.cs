using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public static class GenerationAlgorithms
{ 
   
    public static HashSet<Vector3Int> SimpleRandomWalk(Vector3Int startPos, int WalkSteps, int Height)
    {
        HashSet<Vector3Int> path = new HashSet<Vector3Int>();

        path.Add(startPos);
        var prevPos = startPos;

        for (int i = 0; i < WalkSteps; i++)
        {
            var randDir = Direction2D.GetRandomDirection();
            var newPos = prevPos + randDir;
            /*if (path.Contains(newPos))
            {
                WalkSteps += 1;
            }
            else
            {*/
                //Debug.Log(randDir.y +"::"+ Height + "WHAAT1");
                //randDir.y = Height;
                //Debug.Log(randDir.y + "WHAAT2");
                //var newPos = prevPos + randDir;
                //newPos.z = Height;
                newPos.y = 0;
                newPos.y = Height;
                path.Add(newPos);
                prevPos = newPos;
            //}
        }
    

        return path;
    }

}


public static class Direction2D
{
    public static List<Vector3Int> cardinalDirectionList = new List<Vector3Int>
    {
        new Vector3Int(0,0,1), //up
        new Vector3Int(1,0,0), // right
        new Vector3Int(0,0,-1), //down
        new Vector3Int(-1,0,0) //left
    };

   /* public static List<List<Vector3Int>> ComplexDirectionList = new List<List<Vector3Int>>()
    {
        new List<Vector3Int(0,1)>, //up
        new Vector3Int(1,0), // right
        new Vector3Int(0,-1), //down
        new Vector3Int(-1,0) //left
    };*/
    
    
    
    
    public static Vector3Int GetRandomDirection()
    {
        return cardinalDirectionList[Random.Range(0, cardinalDirectionList.Count)];
    }
    
}