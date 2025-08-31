using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    [Header("Game object")]
    public PoolManager poolmanager;
    public Player player;
    public LevelUp uiLevelUp;
    [Header("Game control")]
    public float gameTime;
    public float maxGameTime = 2 * 10f;
    [Header("player info")]
    public int health;
    public int maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 30, 55, 75, 120, 230, 450 , 600};


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        health = maxHealth;
    }
    void Update()
    {
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }

    public void getExp()
    {
        exp++;
        if (exp == nextExp[level])
        {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }
}