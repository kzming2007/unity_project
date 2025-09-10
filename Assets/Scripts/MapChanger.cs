using UnityEngine;
using UnityEngine.Tilemaps;

public class MapChanger : MonoBehaviour
{
    private Tilemap tilemap;

    void Awake()
    {
        // 이 스크립트가 붙어있는 게임 오브젝트의 Tilemap 컴포넌트를 가져온다.
        tilemap = GetComponent<Tilemap>();
    }

    // GameManager가 호출할 함수.
    // 특정 타일(original)을 새로운 타일(newTile)으로 전부 교체한다.
    public void SwapAllTiles(TileBase originalTile, TileBase newTile)
    {
        if (tilemap != null)
        {
            // SwapTile 함수는 특정 타일을 다른 타일로 한 번에 교체해주는 편리한 기능이야.
            tilemap.SwapTile(originalTile, newTile);
        }
    }
}