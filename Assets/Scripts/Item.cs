using UnityEngine;
using UnityEngine.UI;


public class Item : MonoBehaviour
{
    public ItemData data;
    public int Level;
    public Weapon weapon;
    public Gear gear;

    Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;

    private void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        textName.text = data.itemName;
    }

    private void OnEnable()
    {
        textLevel.text = "Lv." + (Level + 1);

        switch (data.itemType)
        {
            case ItemData.ItemType.melee:
            case ItemData.ItemType.range:
                textDesc.text = string.Format(data.itemDesc, data.damage[Level] * 100, data.count[Level]);
                break;
            case ItemData.ItemType.glove:
            case ItemData.ItemType.shoe:
                textDesc.text = string.Format(data.itemDesc, data.damage[Level] * 100);
                break;
            default:
                textDesc.text = string.Format(data.itemDesc);
                break;
        }
    }

    public void OnClick()
    {
        switch (data.itemType)
        {
            case ItemData.ItemType.melee:
            case ItemData.ItemType.range:
                if (Level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }
                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    nextDamage += data.baseDamage * data.damage[Level];
                    nextCount += data.count[Level];

                    weapon.LevelUp(nextDamage, nextCount);
                }
                Level++;
                break;
            case ItemData.ItemType.glove:
            case ItemData.ItemType.shoe:
                if (Level == 0)
                {
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(data);
                }
                else
                {
                    float nextRate = data.damage[Level];
                    gear.LevelUp(nextRate);
                }
                Level++;
                break;
            case ItemData.ItemType.heal:
                GameManager.instance.health = GameManager.instance.maxHealth;
                break;
        }

        

        if (Level == data.damage.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
