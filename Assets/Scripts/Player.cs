using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scaner scan;
    public Hand[] hands;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scan = GetComponent<Scaner>();
        hands = GetComponentsInChildren<Hand>(true);
    }


    void FixedUpdate()
    {
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + nextVec);
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>().normalized;
    }

    private void LateUpdate()
    {
        anim.SetFloat("Speed", inputVec.magnitude);
        if (inputVec.x != 0)
        {
            sr.flipX = inputVec.x < 0;
        }
    }
}
