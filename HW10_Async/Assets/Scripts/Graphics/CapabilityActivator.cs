using System.Collections;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class CapabilityActivator : MonoBehaviour
{
    EntityManager em;
    EntityQuery query;

    private Button button;
    private float elapsedTime;
    
    private void Start()
    {
        button = GetComponent<Button>();

        em = World.DefaultGameObjectInjectionWorld.EntityManager;
        query = em.CreateEntityQuery(typeof(FreezeCapabilityData));
    }

    public void ActivateFreezeCapability(float freezeDuration) //или лучше передать сюда значения из FreezeCapabilityManager?
    {
        button.interactable = false;
        var entities = query.ToEntityArray(Allocator.Temp);
        foreach (Entity entity in entities)
        {
            if (!em.HasComponent<FreezeTag>(entity))
            {
                em.AddComponent<FreezeTag>(entity);
            }
            else if(!em.IsComponentEnabled<FreezeTag>(entity))
            {
                Debug.Log("FreezeTag activated");
                em.SetComponentEnabled<FreezeTag>(entity, true);
            }
        }
        StartCoroutine(ResetButton(freezeDuration));
        entities.Dispose();
    }

    private IEnumerator ResetButton(float duration)
    {
        yield return new WaitForSeconds(duration);
        button.interactable = true;  
    }
}
