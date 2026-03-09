using System.Collections.Generic;
using Unity.Entities;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class CraftHandler : MonoBehaviour
{
    public CraftSettings craftSettings;
    private List<GameObject> selected = new();

    
    public void Select(GameObject go)
    {
        if(selected.Contains(go))
        {
            selected.Remove(go);
            go.GetComponent<Image>().color = Color.white;
        }
        else
        {
            selected.Add(go);
            go.GetComponent<Image>().color = new Color(1, 0.5f, 0.5f, 0.7f);
        }
    }

    public void OnCraftButtonClicked()
    {
        if (selected.Count == 0) return;
        var em = World.DefaultGameObjectInjectionWorld.EntityManager;
        var playerEntity = em.CreateEntityQuery(typeof(PlayerTag)).GetSingletonEntity();

        //add component
        em.AddComponentData(playerEntity, new CraftRequest
        {
            ResultID = craftSettings.ResultItemID,
        });
          
        //вешаем на игрока динамический буфер и "открываем его буфер-рюкзак"
        var buffer = em.AddBuffer<CraftIngredienstBuffer>(playerEntity);
        //берем созданные ингредиенты из SO и помещаем его в буффер для системы CraftingSystem
        foreach (var item in craftSettings.ingredients)
        {
            buffer.Add(new CraftIngredienstBuffer
            {
                ItemID = item.ItemID,
                Count = item.ItemCount
            });
        }

        //Selected
        var selectedBuffer = em.AddBuffer<SelectedItemElement>(playerEntity);
        foreach(var go in selected)
        {
            var slot = go.GetComponent<InventorySlotUI>();
            if (slot != null) selectedBuffer.Add(new SelectedItemElement { ItemID = slot.ItemID });
        }

        
        ClearSelection();
    }

    private void ClearSelection()
    {
        foreach(var go in selected)
        {
            if (go != null)
                go.GetComponent<Image>().color = Color.white;
        }
        selected.Clear();
    }
}
