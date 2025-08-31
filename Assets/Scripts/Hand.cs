using Unity.Mathematics;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool isLeft;
    public SpriteRenderer sr;

    SpriteRenderer player;

    Vector3 rightPos = new Vector3(0.34f, -0.15f, 0);
    Vector3 rightPosRV = new Vector3(-0.15f, -0.15f, 0);
    quaternion leftRot = quaternion.Euler(0, 0, -35);
    quaternion leftRotRV = quaternion.Euler(0, 0, -135);

    private void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    private void LateUpdate()
    {
        bool isRv = player.flipX;

        if (isLeft)
        {
            transform.localRotation = isRv ? leftRotRV : leftRot;
            sr.flipY = isRv;
            sr.sortingOrder = isRv ? 4 : 6;
        }
        else
        {
            transform.localPosition = isRv ? rightPosRV : rightPos;
            sr.flipX = isRv;
            sr.sortingOrder = isRv ? 6 : 4;
        }
    }
}
