using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public static class GenerationAlgorithms
{
    //static HashSet<Vector3Int> Path = new HashSet<Vector3Int>();
    //static HashSet<Vector3Int> Path;
    //static HashSet<Vector3Int> Corners = new HashSet<Vector3Int>();
    //static HashSet<Vector3Int> Corners;
    
    public static WalkedBy SimpleRandomWalk(Vector3Int startPos, int WalkSteps, int Height, int OccuSpace, WalkedBy rwBefore)
    {

        
        
        //varaibles:
        WalkedBy thisRW = new WalkedBy(1);
        
        
        //startPos.y = 0;
        startPos.y = Height;
        thisRW.Path.Add(startPos);
        thisRW.Corners.Add(startPos);
        thisRW.InfoBlock.Add(startPos,0);
        Debug.Log("added corner:"+startPos);

        var prevPos = startPos;

        //Creació del randomwalk i passos
        for (int i = 0; i < WalkSteps; i++)
        {
            
            //Debug.Log("coming for"+i);
            var randDir = Direction2D.GetRandomDirection();
            var newPos = prevPos + randDir;
            
            
            
            
            //si on anem és una posició que ja tenim
            if (thisRW.Path.Contains(newPos) || rwBefore.Occupied.Contains(newPos))
            {//Debug.Log("coming IF1");
                
                
                //si saltat les barreres i per algun motiu està per sota d'un rw
                //no se l'efectivitat d'això
                if (IsSurrounded(newPos, thisRW))
                {
                    //Debug.Log("coming is surrounded 1");
                    prevPos= thisRW.Corners.ElementAt(Random.Range(0, thisRW.Corners.Count));
                }
                //i -= 1; //WHY THIS PETA TOT???? //ho hauré de substituïr per un while, encara que em preocupa que peti igualment
            }
            //anem a una posició que no tenim
            
            else
            {
                //Debug.Log("coming ELSE1::added corner:"+newPos);
                
                newPos.y = Height;
                thisRW.Corners.Add(newPos);
                thisRW.InfoBlock.Add(newPos,0);

                List<Vector3Int> checkDirections = Direction2D.GetAllPossibleDirections();
                
                //revisió per els corners
                foreach (var dir in checkDirections)
                {
                    //Debug.Log("coming foreach"+dir.ToString());
                    Vector3Int possibleRemove = newPos + dir;
                    //Debug.Log("coming foreach"+possibleRemove.ToString());
                    if (thisRW.Corners.Contains(possibleRemove))
                    {
                        //Debug.Log("coming IF2");
                        thisRW.InfoBlock[possibleRemove] += 1;
                        thisRW.InfoBlock[newPos] += 1;
                        //int p = IsProababySurrounded( possibleRemove, thisRW);
                        if(thisRW.InfoBlock[possibleRemove]==4){
                            //Debug.Log("coming is surrounded 2");
                            thisRW.Corners.Remove(possibleRemove);
                        }
                        if(thisRW.InfoBlock[newPos]==4){
                            //Debug.Log("coming is surrounded 2");
                            thisRW.Corners.Remove(newPos);
                        }
                    }
                }
                Debug.Log("ending else");
                thisRW.Path.Add(newPos);
                prevPos = newPos;
                
                //creació de occupied
                for (int j = 0; j < OccuSpace; j++)
                {
                    newPos.y-=1;
                    thisRW.Occupied.Add(newPos);
                }
                
                
            }
        }
    

        return thisRW;
    }


    public static Vector3Int startPosToRW()
    {
        return new Vector3Int(Random.Range(-16,16),0,Random.Range(-16,16));
    }
    
    public static bool IsSurrounded(Vector3Int pos, WalkedBy thisRW)
    {
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