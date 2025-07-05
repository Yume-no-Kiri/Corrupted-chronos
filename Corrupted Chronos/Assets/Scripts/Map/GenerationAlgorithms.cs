using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public static class GenerationAlgorithms
{ 
   
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPos, int WalkSteps)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();

        path.Add(startPos);
        var prevPos = startPos;

        for (int i = 0; i < WalkSteps; i++)
        {
            var newPos = prevPos + Direction2D.GetRandomDirection();
            path.Add(newPos);
            prevPos=newPos;
        }

        return path;
    }

}


public static class Direction2D
{
    public static List<Vector2Int> cardinalDirectionList = new List<Vector2Int>
    {
        new Vector2Int(0,1), //up
        new Vector2Int(1,0), // right
        new Vector2Int(0,-1), //down
        new Vector2Int(-1,0) //left
    };

    public static Vector2Int GetRandomDirection()
    {
        return cardinalDirectionList[Random.Range(0, cardinalDirectionList.Count)];
    }
    
}