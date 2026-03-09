using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemDataBase", menuName = "Items/Create ItemDataBase")]
public class ItemDataBase : ScriptableObject
{
    public List<ItemStaticData> Items;

    public ItemStaticData GetItem(int ID) 
    {
        return Items.Find(x => x.ItemID == ID);
    }
}
