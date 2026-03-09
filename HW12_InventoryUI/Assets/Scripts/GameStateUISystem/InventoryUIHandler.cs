using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIHandler : MonoBehaviour
{
    [Header("Button settings")]
    [SerializeField] private GameObject inventoryCanvas;
    [Header("UI Settings")]
    [SerializeField] private ItemDataBase dataBase;
    [SerializeField] private Transform inventoryContainer;
    [SerializeField] private GameObject itemPrefab;

    [Header("Craft Settings")]
    [SerializeField] private CraftHandler craft;

    private EntityManager _em;
    private Entity _playerEntity;
    private int _lastBufferTick = -1;

    private void Start()
    {
        inventoryCanvas.SetActive(false);

        _em = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    private void Update()
    {
        if(_playerEntity == Entity.Null)
        {
            var query = _em.CreateEntityQuery(typeof(CharacterDataComponent));
            if(query.HasSingleton<CharacterDataComponent>())
                _playerEntity = query.GetSingletonEntity();
            else return;
        }
        if(_em.HasBuffer<InventoryBufferElement>(_playerEntity))
        {
            var inventory = _em.GetBuffer<InventoryBufferElement>(_playerEntity);
            int currentHash = GetCurrentHash(inventory);
            if(currentHash != _lastBufferTick)
            {
                RefreshUI(inventory);
                _lastBufferTick = currentHash;
            }
        }
    }

    private void RefreshUI(DynamicBuffer<InventoryBufferElement> inventory)
    {
        //clear olds
        foreach (Transform child in inventoryContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var element in inventory)
        {
            ItemStaticData item = dataBase.GetItem(element.ItemID);
            if(item!=null)
            {
                var newItem = Instantiate(itemPrefab, inventoryContainer);
                var slotUI = newItem.GetComponent<InventorySlotUI>();
                slotUI.iconImage.sprite = item.icon;
                slotUI.ItemID = element.ItemID;

                //CRAFT
                var iconButton = newItem.GetComponent<Button>()?? newItem.AddComponent<Button>();
                iconButton.onClick.RemoveAllListeners();
                iconButton.onClick.AddListener(
                    () =>
                    {
                        craft.Select(newItem);
                    }
                    );  

                //USE
                IUsable usableItem = item as IUsable;
                bool canBeUsed = usableItem != null && usableItem.CanBeUsed;
                if(slotUI.useBtn != null && usableItem.CanBeUsed)
                {
                    //var useButton = 
                    slotUI.useBtn.gameObject.SetActive(canBeUsed);
                    slotUI.useBtn.onClick.RemoveAllListeners();
                    slotUI.useBtn.onClick.AddListener(() =>
                        {
                           // Debug.Log($"<color=cyan> btn Use for {item.name}");
                            usableItem.Use();
                            _em.AddComponentData(_playerEntity, new UseItemRequest
                            {
                                ItemID = element.ItemID,
                            });
                        });
                        
                    
                }
            }
        }
    }

    public void OnOpenInventoryClicked()
    {
        inventoryCanvas.SetActive(true);
    }

    public void OnCloseInventoryClicked()
    {
        inventoryCanvas.SetActive(false);
    }

    private int GetCurrentHash(DynamicBuffer<InventoryBufferElement> inventory)
    {
        int currentHash = inventory.Length;
        for (int i = 0; i < inventory.Length; i++)
        {
            currentHash += inventory[i].ItemID * (i + 1);
        }
        return currentHash;
    }
}
