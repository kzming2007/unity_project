using UnityEngine;
using UnityEngine.Tilemaps;

public class MapChanger : MonoBehaviour
{
    private Tilemap tilemap;

    void Awake()
    {
        // �� ��ũ��Ʈ�� �پ��ִ� ���� ������Ʈ�� Tilemap ������Ʈ�� �����´�.
        tilemap = GetComponent<Tilemap>();
    }

    // GameManager�� ȣ���� �Լ�.
    // Ư�� Ÿ��(original)�� ���ο� Ÿ��(newTile)���� ���� ��ü�Ѵ�.
    public void SwapAllTiles(TileBase originalTile, TileBase newTile)
    {
        if (tilemap != null)
        {
            // SwapTile �Լ��� Ư�� Ÿ���� �ٸ� Ÿ�Ϸ� �� ���� ��ü���ִ� ���� ����̾�.
            tilemap.SwapTile(originalTile, newTile);
        }
    }
}