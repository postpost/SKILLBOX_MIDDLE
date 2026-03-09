using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemRecipe", menuName = "Items/Create New Recipe")]
public class CraftSettings : ScriptableObject
{
    public List<Ingredient> ingredients;

    [SerializeField] private ItemTypes _result;
    public int ResultItemID => (int)_result;
}

[System.Serializable]
public struct Ingredient
{

    [SerializeField] private ItemTypes _itemType;
    public int ItemID => (int)_itemType;
    public int ItemCount;
}