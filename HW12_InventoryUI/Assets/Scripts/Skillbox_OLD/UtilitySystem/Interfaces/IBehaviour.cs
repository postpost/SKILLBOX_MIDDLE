using UnityEngine;

public interface IBehaviour 
{
    void Init();
    float Evaluate(Vector3 targetPos);
    void Behave(Vector3 targetPos);
}
