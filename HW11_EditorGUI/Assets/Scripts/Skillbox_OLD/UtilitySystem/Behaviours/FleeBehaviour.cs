using Unity.Mathematics;
using UnityEngine;

public class FleeBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] private float detectRadius = 10f;
    [SerializeField] private float exponent = 3f; //the higher the sharper the reaction (slope of the reaction)
    [SerializeField] private float moveSpeed = 2f;

    public void Behave(Vector3 targetPos)
    {
        Vector3 dir =(transform.position- targetPos).normalized;
        Vector3 target = transform.position + dir;
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
        
    }

    //math.exp(x) Ч это возведение числа (e = 2.718f) в степень (x).
    public float Evaluate(Vector3 targetPos)
    {
        float distance = Vector3.Distance(transform.position, targetPos);
        //the closest = 1, the farest = 0
        float distanceNorm = math.clamp(1 - distance / detectRadius, 0, 1);

        float score = (math.exp(exponent * distanceNorm) - 1f) / (math.exp(exponent) - 1f);
        return score;
    }

    public void Init()
    {
        
    }
}
