using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;


public class IceCube : MonoBehaviour
{

    public Tile WaterTile;

    float SPEED = 10.0f;
    Vector3 target_pos;
   

    public static List<Vector3Int> wet_tiles = new List<Vector3Int>();
    static List<Vector3Int> removeList = new List<Vector3Int>();
    static float DRY_SPEED = 0.3f;


    public void InitIceCube(Vector3 target)
    {
        target_pos = target;
    }

    void Update()
    {
        Vector3 direction = target_pos - transform.position;
        direction.z = 0;
        transform.position += direction.normalized * SPEED * Time.deltaTime;

        float offset = 0.1f;
        if (Vector3.Distance(target_pos, transform.position) < offset)
        {
            Burst();
        }
    }

    void Burst()
    {

        GameHandler.instance.sfxScript.PlayWaterSplash();
        // MAKE FLOOR WET
        Tilemap tilemap = GameHandler.instance.tilemapWet;
        Vector3Int cellPosition = tilemap.WorldToCell(transform.position);

        bool existing = false;
        Vector3Int temp_remove = Vector3Int.zero;
        foreach (Vector3Int tilePos in wet_tiles)
        {
            if (tilePos == cellPosition)
            {
                existing = true;
                temp_remove = tilePos;
                break;
            }
        }
        if (existing)
            wet_tiles.Remove(temp_remove);

        tilemap.SetTile(cellPosition, WaterTile);
        tilemap.SetTileFlags(cellPosition, TileFlags.None);
        Color tile_color = GameHandler.instance.tilemapWet.GetColor(cellPosition);
        tile_color.a = 1.0f;
        GameHandler.instance.tilemapWet.SetColor(cellPosition, tile_color);
        wet_tiles.Add(cellPosition);

        Destroy(gameObject);
    }

    public static void DryWetTiles()
    {
        

        foreach (Vector3Int tile_pos in wet_tiles)
        {
            Color tile_color = GameHandler.instance.tilemapWet.GetColor(tile_pos);
            tile_color.a -= DRY_SPEED * Time.deltaTime;
            GameHandler.instance.tilemapWet.SetColor(tile_pos, tile_color);

            if (tile_color.a <= 0.0f)
            {
                removeList.Add(tile_pos);
            }
        }

        foreach (Vector3Int tile_pos in removeList)
        {
            wet_tiles.Remove(tile_pos);
            GameHandler.instance.tilemapWet.SetTile(tile_pos, null);

        }
        removeList.Clear();


    }
}
