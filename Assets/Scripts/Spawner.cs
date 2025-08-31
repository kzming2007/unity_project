using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public SpawnData[] spawnDatas;

    float timer;
    int level;

    private void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f),spawnDatas.Length - 1);
        if (timer > spawnDatas[level].spawnTime)
        {
            Spawn();
            timer = 0;
        }

        
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.poolmanager.Get(0);
        enemy.transform.position = spawnPoints[Random.Range(1, spawnPoints.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnDatas[level]);
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}