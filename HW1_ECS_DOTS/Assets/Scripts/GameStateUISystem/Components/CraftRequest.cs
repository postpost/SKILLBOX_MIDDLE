
using Unity.Entities;

public struct CraftRequest:IComponentData
{
    public int ResultID;
}

//это простой массив данных (рюкзак)
//DynamicBuffer
public struct CraftIngredienstBuffer: IBufferElementData
{
    public int ItemID;
    public int Count;
}
