using UnityEngine;

public class WaitBehaviour : MonoBehaviour, IBehaviour
{
    public void Behave(Vector3 targetPos)
    {
        Debug.Log("Wait!");
    }

    public float Evaluate(Vector3 targetPos)
    {
        return 0.1f;
    }

    public void Init()
    {
       
    }
}
