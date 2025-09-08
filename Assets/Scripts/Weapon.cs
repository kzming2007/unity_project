
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabid;
    public float damage;
    public int count;
    public float speed;
    float timer;

    Player player;

    private void Awake()
    {
        player = GameManager.instance.player;
    }

    private void Update()
    {
        if (!GameManager.instance.isLive)
            return;
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;
                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }

        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(20, 5);
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0)
        {
            Batch();
        }

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }
    public void Init(ItemData data)
    {
        name = "weapon_" + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;


        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;
        
        for (int i=0; i < GameManager.instance.poolmanager.prefabs.Length; i++)
        {
            if (data.prpjectile == GameManager.instance.poolmanager.prefabs[i])
            {
                prefabid = i;
                break;
            }
        }
        switch (id)
        {
            case 0:
                speed = 150f;
                Batch();
                break;
            default:
                speed = 0.3f;
                break;
        }

        Hand hand = player.hands[(int)data.itemType];
        hand.sr.sprite = data.hand;
        hand.gameObject.SetActive(true);
        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);
    }

    void Batch()
    {
        for (int i = 0; i < count; i++)
        {
            Transform bullet;
            if (i < transform.childCount)
            {
                bullet = transform.GetChild(i);
            }
            else
            {
                bullet = GameManager.instance.poolmanager.Get(prefabid).transform;
            }

            bullet.parent = transform;

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * i / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero);
        }
    }

    void Fire()
    {
        if (!player.scan.nearestTarget)
            return;

        Vector3 targetPos = player.scan.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.poolmanager.Get(prefabid).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);

        AudioManager.instance.Playsfx(AudioManager.SFX.Range);
    }
}
