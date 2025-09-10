using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;

    private Rigidbody2D target;
    private Rigidbody2D rb;
    private Collider2D coll;
    private Animator anim;
    private SpriteRenderer sr;
    private bool isLive;
    [Header("보스 설정")]
    public float deathAnimationTime = 2f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void Init(BossData data)
    {
        // ũ�⸦ 3��� Ű���!
        transform.localScale = Vector3.one * 3f;

        anim.runtimeAnimatorController = data.bossAnimator;
        speed = data.speed;
        maxHealth = data.health;
        health = maxHealth;

        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rb.simulated = true;
        gameObject.SetActive(true);
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive || !isLive) return;

        Vector2 dirVec = target.position - rb.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + nextVec);
        rb.linearVelocity = Vector2.zero;
    }

    void LateUpdate()
    {
        if (!GameManager.instance.isLive || !isLive) return;
        sr.flipX = target.position.x < rb.position.x;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive) return;

        health -= collision.GetComponent<Bullet>().damage;

        if (health > 0)
        {
            anim.SetTrigger("Hit");
            AudioManager.instance.Playsfx(AudioManager.SFX.Hit);
        }
        else if (isLive)
        {
            StartCoroutine(DieRoutine());
        }

        IEnumerator DieRoutine()
        {
            isLive = false;
            coll.enabled = false;
            rb.simulated = false;
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.getExp();
            GameManager.instance.uiLevelUp.Show();

            if (GameManager.instance.isLive)
                AudioManager.instance.Playsfx(AudioManager.SFX.Dead);

            yield return new WaitForSeconds(deathAnimationTime);

            gameObject.SetActive(false);
        }
    }
}