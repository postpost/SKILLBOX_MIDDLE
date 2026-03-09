using UnityEngine;

[CreateAssetMenu(fileName = "NewWaterItem", menuName = "Items/Create New WaterItem")]
public class WaterItemData: ItemStaticData
{

    public override void Use()
    {
        CapabilityActivator activator = FindFirstObjectByType<CapabilityActivator>();
        if(activator != null )
        {
            activator.ActivateFreezeCapability();
        }
        else
        {
            Debug.Log("No Freeze Capability in the scene!");
        }
    }
}
