using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/* visualitzador del mapa amb prefab i tilemap
 * Actualment no funicona la tile part perque no utilitzem tilemap
 * 
 */



public class MapVisualizer : MonoBehaviour
{
    //us de tilemap
    //s'ha de [serialiaze]r 
    private Tilemap floorTilemap;
    private TileBase floorTile;

    //prefabs
    public GameObject prefabTerreny;
    
    public void PaintFloorTile(WalkedBy floorPositions)
    {
        //PaintTiles(floorPositions, floorTilemap, floorTile);
        PaintPrefab(floorPositions);
        foreach (var VARIABLE in floorPositions.Occupied)
        {
            Debug.Log($"Key: {VARIABLE.Key}, Value: {VARIABLE.Value}");
        }
    }

    private void PaintPrefab(WalkedBy positions)
    {
        foreach (var position in positions.Path)
        {
            if (positions.Corners.Contains(position))
            {
                GameObject obj=Instantiate(prefabTerreny, position,Quaternion.identity);
                SpriteRenderer sr = obj.GetComponentInChildren<SpriteRenderer>();
                sr.color = Color.blue; // Tint it blue
            }
            else
            {
                Instantiate(prefabTerreny, position,Quaternion.identity);
            }
        }
    }


    //use de tiles
    private void PaintTiles(IEnumerable<Vector3Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in positions)
        {
            PaintSingleTile( tilemap, tile,position);  
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile,Vector3Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
