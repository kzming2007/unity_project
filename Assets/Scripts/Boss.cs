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

    // GameManager�� BossData�� �Ѱ��ָ鼭 ȣ���� �ʱ�ȭ �Լ�
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
        else if (isLive) // isLive 조건을 추가해서 딱 한 번만 실행되도록!
        {
            // 사망 처리를 코루틴으로 시작
            StartCoroutine(DieRoutine());
        }

        IEnumerator DieRoutine()
        {
            // 1. 사망 상태로 전환
            isLive = false;
            coll.enabled = false;
            rb.simulated = false;
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.getExp();
            GameManager.instance.uiLevelUp.Show();

            if (GameManager.instance.isLive)
                AudioManager.instance.Playsfx(AudioManager.SFX.Dead);

            // 2. deathAnimationTime 만큼 기다리기
            yield return new WaitForSeconds(deathAnimationTime);

            // 3. 기다림이 끝나면 시체(오브젝트) 비활성화
            gameObject.SetActive(false);
        }
    }
}