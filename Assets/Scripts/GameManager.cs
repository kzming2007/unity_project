using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    [Header("Game object")]
    public PoolManager poolmanager;
    public Player player;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public GameObject enemyCleaner;
    [Header("Game control")]
    public float gameTime;
    public float maxGameTime;
    public bool isLive;
    [Header("player info")]
    public float health;
    public float maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 30, 55, 75, 120, 230, 450, 600 };

    public Spawner spawner;
    [Header("Stage Control")]
    
    public MapChanger[] mapChangers;
    public TileBase[] stageTiles;

    private float stageChangeTimer = 0f;
    private int currentStageIndex = 0;
    [Header("Boss Control")] // 이 부분을 새로 추가하자!
    public GameObject bossPrefab; // 보스 프리팹 원본
    public BossData[] bossDatas;  // 스테이지별 보스 데이터 목록
    private bool isBossSpawnedForStage = false;


    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }


    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());

    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;
        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        stop();
        AudioManager.instance.Playbgm(false);
        AudioManager.instance.Playsfx(AudioManager.SFX.Lose);
    }


    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());

    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        stop();
        AudioManager.instance.Playbgm(false);
        AudioManager.instance.Playsfx(AudioManager.SFX.Win);
    }
    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    public void GameStart()
    {
        health = maxHealth;
        uiLevelUp.Select(0);
        isLive = true;
        Resume();
        AudioManager.instance.Playbgm(true);
        AudioManager.instance.Playsfx(AudioManager.SFX.Select);
    }
    void Update()
    {
        if(!isLive)
            return;

        gameTime += Time.deltaTime;
        stageChangeTimer += Time.deltaTime;


        if (spawner.levelTime > 0 && stageChangeTimer >= spawner.levelTime)
        {
            // --- 보스 소환 로직 ---
            // 아직 이번 스테이지의 보스가 소환되지 않았고, 소환할 보스 데이터가 남아있다면
            if (!isBossSpawnedForStage && currentStageIndex < bossDatas.Length)
            {
                SpawnBoss();
                isBossSpawnedForStage = true; // 소환했다고 표시
            }
            // --------------------

            // 기존 맵 변경 로직 (보스가 나타나고 조금 뒤에 맵이 바뀌는 느낌으로)
            stageChangeTimer = 0f; // 타이머 초기화를 보스가 죽었을 때로 옮길 수도 있어
            ChangeNextStage();     
        }

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory();
        }
    }

    void SpawnBoss()
    {
        // 보스 프리팹을 생성
        GameObject bossObj = Instantiate(bossPrefab, player.transform.position + new Vector3(0, 5, 0), Quaternion.identity);
        // Boss.cs 컴포넌트를 가져옴
        Boss boss = bossObj.GetComponent<Boss>();
        // 현재 스테이지에 맞는 보스 데이터로 초기화
        boss.Init(bossDatas[currentStageIndex]);

        Debug.Log((currentStageIndex + 1) + "번째 보스 등장!");
    }

    void ChangeNextStage()
    {
        // 다음 스테이지 인덱스가 타일 배열의 길이를 넘어서면 더 이상 진행하지 않음
        if (currentStageIndex + 1 >= stageTiles.Length)
        {
            Debug.Log("마지막 스테이지입니다.");
            return;
        }

        // 바꿀 대상이 되는 이전 타일과, 새로 적용할 타일을 지정
        TileBase previousTile = stageTiles[currentStageIndex];
        TileBase nextTile = stageTiles[currentStageIndex + 1];

        // 4개의 맵 조각 모두에게 타일을 바꾸라고 명령!
        foreach (MapChanger changer in mapChangers)
        {
            changer.SwapAllTiles(previousTile, nextTile);
        }

        currentStageIndex++; // 현재 스테이지 인덱스를 1 증가시켜 다음을 준비
        isBossSpawnedForStage = false;
        Debug.Log("스테이지 변경! 현재 스테이지: " + (currentStageIndex + 1));
    }

    public void getExp()
    {
        if (!isLive)
            return;
        exp++;
        if (exp == nextExp[Mathf.Min(level,nextExp.Length-1)])
        {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }
    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }
}