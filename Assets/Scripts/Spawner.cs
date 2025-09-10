using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public SpawnData[] spawnDatas;
    public float levelTime;
    float timer;
    int level;

    public void Start()
    {
        levelTime = GameManager.instance.maxGameTime / spawnDatas.Length;
    }
    private void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime),spawnDatas.Length - 1);
        if (timer > spawnDatas[level].spawnTime)
        {
            timer = 0;
            Spawn();
            
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
[System.Serializable]
public class BossData
{
    public RuntimeAnimatorController bossAnimator;
    public int health;
    public float speed;
}