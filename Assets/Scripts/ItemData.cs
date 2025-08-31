using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Item",menuName ="Scriptble objects/ItemData")]
public class ItemData : ScriptableObject
{

    
    public enum ItemType {melee, range, glove, shoe, heal }
    [Header("main info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;
    [Header("level data")]
    public float baseDamage;
    public int baseCount;
    public float[] damage;
    public int[] count;

    [Header("weapon")]

    public GameObject prpjectile;
    public Sprite hand;
}
