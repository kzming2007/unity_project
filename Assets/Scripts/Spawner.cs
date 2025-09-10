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
        // 다른 친구들을 필요로 하는 코드는 모두 Start에서!
        levelTime = GameManager.instance.maxGameTime / spawnDatas.Length;
    }
    private void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
        //levelTime = GameManager.instance.maxGameTime / spawnDatas.Length;
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
    public RuntimeAnimatorController bossAnimator; // 보스의 애니메이터 컨트롤러
    public int health;          // 보스 체력
    public float speed;         // 보스 이동 속도
    public float attackDamage;  // 보스 공격력 (예시)
    public float attackCooldown; // 보스 공격 쿨타임 (예시)
}