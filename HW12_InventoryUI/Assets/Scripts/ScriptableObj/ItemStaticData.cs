using UnityEngine;

[CreateAssetMenu(fileName = "NewItemStaticData", menuName = "Items/Create New ItemStaticData")]
public class ItemStaticData : ScriptableObject, IUsable
{
    public string ItemName;
    public Sprite icon;
   // public GameObject itemPrefab; //если нужно заспавнить обратно в мир
    public ItemTypes type;
    public bool isConsumable;

    public virtual bool CanBeUsed => isConsumable;
    public int ItemID => (int)type;

    public virtual void Use() 
    {
        Debug.Log("Item can be used");
    }
}
