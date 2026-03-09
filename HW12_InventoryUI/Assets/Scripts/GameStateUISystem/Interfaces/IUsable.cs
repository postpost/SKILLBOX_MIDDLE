using UnityEngine;

public interface IUsable
{
    public bool CanBeUsed { get; }
    public void Use();
}
