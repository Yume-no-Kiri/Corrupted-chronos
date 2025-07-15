using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockDataSO", menuName = "Scriptable Objects/BlockDataSO")]
public class BlockDataSO : ScriptableObject
{
    public float textureSizeX, textureSizeY;
    public List<TextureData> textureDataList;
}

[Serializable]
public class TextureData
{
    public BlockType blockType;
    //textures diferents pèr, amunt, avall i costats
    // si vull tindre més textures pel costats, afegir més aquí.
    public Vector2Int up, down, side;
    //solid, per saber si algo ha de mostrar lo que te adjacent o no, exemple: vidre o aigua del minecraft, pots veure a traves
    public bool isSolid = true;
    public bool generatesCollider = true;
}