using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, ICraftable
{
    public UnityEngine.UI.Image iconImage;
    public Button useBtn;

    [HideInInspector] public int ItemID;
}
