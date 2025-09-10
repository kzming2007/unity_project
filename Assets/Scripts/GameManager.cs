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
    [Header("Boss Control")]
    public GameObject bossPrefab;
    public BossData[] bossDatas;
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
            if (!isBossSpawnedForStage && currentStageIndex < bossDatas.Length)
            {
                SpawnBoss();
                isBossSpawnedForStage = true;
            }

            stageChangeTimer = 0f;
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
        GameObject bossObj = Instantiate(bossPrefab, player.transform.position + new Vector3(0, 5, 0), Quaternion.identity);
        Boss boss = bossObj.GetComponent<Boss>();
        boss.Init(bossDatas[currentStageIndex]);
    }

    void ChangeNextStage()
    {
        if (currentStageIndex + 1 >= stageTiles.Length)
        {
            return;
        }

        TileBase previousTile = stageTiles[currentStageIndex];
        TileBase nextTile = stageTiles[currentStageIndex + 1];

        foreach (MapChanger changer in mapChangers)
        {
            changer.SwapAllTiles(previousTile, nextTile);
        }

        currentStageIndex++;
        isBossSpawnedForStage = false;
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