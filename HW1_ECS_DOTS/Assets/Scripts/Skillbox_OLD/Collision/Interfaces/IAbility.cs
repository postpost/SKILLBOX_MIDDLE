
using UnityEngine;

public interface IAbility 
{
    public void ExecuteAll(Collider[] hits, int count);
    public void Execute();
}
