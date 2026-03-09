using UnityEngine;

public class RotateBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] private float detectionRadius = 1f;

    public void Behave(Vector3 targetPos)
    {
        this.transform.Rotate(Vector3.up, 10);
    }

    public float Evaluate(Vector3 targetPos)
    {
        float sqrtDistance = Vector3.SqrMagnitude(transform.position - targetPos);
        float sqrtRadius = detectionRadius * detectionRadius;
        
        //нормализуем и инвертируем, чтобы при макс значении был 0, а при минимальном 1
        float score = 1.0f - sqrtDistance/ (sqrtRadius + 0.001f);
        return Mathf.Clamp01(score);
    }

    public void Init()
    {
        
    }
}
