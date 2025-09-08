using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] anicon;
    public Rigidbody2D target;

    bool isLive;
    Rigidbody2D rb;
    Collider2D coll;
    Animator anim;
    SpriteRenderer sr;
    WaitForFixedUpdate wf;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        wf = new WaitForFixedUpdate();
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;
        Vector2 dirVec = target.position - rb.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + nextVec);
        rb.linearVelocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;
        if (!isLive)
            return;
        sr.flipX = target.position.x < rb.position.x;
    }
    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rb.simulated = true;
        sr.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = anicon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = maxHealth;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;
        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(knockback());

        if (health > 0)
        {
            anim.SetTrigger("Hit");
            AudioManager.instance.Playsfx(AudioManager.SFX.Hit);
        }
        else
        {
            isLive = false;
            coll.enabled = false;
            rb.simulated = false;
            sr.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.getExp();

            if(GameManager.instance.isLive) 
                AudioManager.instance.Playsfx(AudioManager.SFX.Dead);
        }
    }

    IEnumerator knockback()
    {
        yield return wf;
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rb.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }
    void Dead()
    {

        gameObject.SetActive(false);
    }
}
