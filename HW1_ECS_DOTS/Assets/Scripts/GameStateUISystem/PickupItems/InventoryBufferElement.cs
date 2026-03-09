using Unity.Entities;

public struct InventoryBufferElement:IBufferElementData 
{
    public int ItemID;
}

public struct SelectedItemElement:IBufferElementData
{
    public int ItemID;
}
