using UnityEngine;
using UnityEngine.Tilemaps;

public class MapChanger : MonoBehaviour
{
    private Tilemap tilemap;

    void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void SwapAllTiles(TileBase originalTile, TileBase newTile)
    {
        if (tilemap != null)
        {
            tilemap.SwapTile(originalTile, newTile);
        }
    }
}