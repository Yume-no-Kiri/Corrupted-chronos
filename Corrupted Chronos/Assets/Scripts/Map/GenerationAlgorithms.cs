using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public static class GenerationAlgorithms
{
    //static HashSet<Vector3Int> Path = new HashSet<Vector3Int>();
    //static HashSet<Vector3Int> Path;
    //static HashSet<Vector3Int> Corners = new HashSet<Vector3Int>();
    //static HashSet<Vector3Int> Corners;
    
    public static WalkedBy SimpleRandomWalk(Vector3Int startPos, int WalkSteps, int Height)
    {
        WalkedBy thisRW = new WalkedBy(1);
        //startPos.y = 0;
        startPos.y = Height;
        thisRW.Path.Add(startPos);
        thisRW.Corners.Add(startPos);
        thisRW.Occupied.Add(startPos,0);
        Debug.Log("added corner:"+startPos);

        var prevPos = startPos;

        for (int i = 0; i < WalkSteps; i++)
        {
            Debug.Log("coming for"+i);
            var randDir = Direction2D.GetRandomDirection();
            var newPos = prevPos + randDir;
            if (thisRW.Path.Contains(newPos))
            {
                Debug.Log("coming IF1");
                //si saltat les barreres i per algun motiu està per sota d'un rw
                //no se l'efectivitat d'això
                if (IsSurrounded(newPos, thisRW))
                {
                    Debug.Log("coming is surrounded 1");
                    prevPos= thisRW.Corners.ElementAt(Random.Range(0, thisRW.Corners.Count));
                }
                //i -= 1; //WHY THIS PETA TOT????
            }
            else
            {
                newPos.y = Height;
                
                Debug.Log("coming ELSE1::added corner:"+newPos);
                thisRW.Corners.Add(newPos);
                thisRW.Occupied.Add(newPos,0);

                List<Vector3Int> checkDirections = Direction2D.GetAllPossibleDirections();
                foreach (var dir in checkDirections)
                {
                    Debug.Log("coming foreach"+dir.ToString());
                    Vector3Int possibleRemove = newPos + dir;
                    Debug.Log("coming foreach"+possibleRemove.ToString());
                    if (thisRW.Corners.Contains(possibleRemove))
                    {
                        Debug.Log("coming IF2");
                        thisRW.Occupied[possibleRemove] += 1;
                        thisRW.Occupied[newPos] += 1;
                        //int p = IsProababySurrounded( possibleRemove, thisRW);
                        if(thisRW.Occupied[possibleRemove]==4){
                            Debug.Log("coming is surrounded 2");
                            thisRW.Corners.Remove(possibleRemove);
                        }
                    }
                }
                Debug.Log("ending else");
                thisRW.Path.Add(newPos);
                prevPos = newPos;
            }
        }
    

        return thisRW;
    }

   

    
    public static bool IsSurrounded(Vector3Int pos, WalkedBy thisRW)
    {
        //podria millorar l'eficiencia, guardant un valor per el número de costats tapats, i així només hauria de sumar als quadrats que afectene el que acabem d'afegir
        List<Vector3Int> checkDirections = Direction2D.GetAllPossibleDirections();
        foreach (var dir in checkDirections)
        {
            if (!thisRW.Path.Contains(pos+dir))
            {
                Debug.Log("RETURN FALSE"+pos.ToString() + dir.ToString());
                return false;
            }
            
            Debug.Log("Continuing is surrounded"+pos.ToString() + dir.ToString());
            
            
        }
        Debug.Log("RETURN TRUE"+pos.ToString());
        return true;
    }
    public static void UnionDictionaries<TKey, TValue>(
        Dictionary<TKey, TValue> target,
        Dictionary<TKey, TValue> source)
    {
        foreach (var pair in source)
        {
            if (!target.ContainsKey(pair.Key))
            {
                target.Add(pair.Key, pair.Value);
            }
        }
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

    public static List<Vector3Int> GetAllPossibleDirections()
    {
        return cardinalDirectionList;
    }
}