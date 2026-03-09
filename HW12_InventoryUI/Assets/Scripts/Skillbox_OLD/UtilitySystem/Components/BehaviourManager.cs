using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BehaviourManager : MonoBehaviour
{
    public List<MonoBehaviour> behaviours;
    public IBehaviour currentBehaviour;
    public AIAgent GetAIAgent { get; set; }
    public NavMeshAgent Agent { get; set; }
    private bool isInitialized = false;

    public void StartInitializing()
    {
        Agent = GetComponent<NavMeshAgent>();
        if (isInitialized) return;

        foreach (var behaviour in behaviours)
        {
            if (behaviour is IBehaviour ai)
            {
                ai.Init();
            }
        }
        isInitialized = true;
    }


    public class BehaviourManagerBaker : Baker<BehaviourManager>
    {
        public override void Bake(BehaviourManager authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            var aiAgent = new AIAgent
            {
                Manager = authoring,
                ActiveBehaviour = null,
                IsBusy = false
            };
            authoring.GetAIAgent = aiAgent;
            AddComponentObject(entity, aiAgent);
        }
    }
}

public class AIAgent: IComponentData //to get managed type для ссылочных типов
{
    public BehaviourManager Manager;
    public IBehaviour ActiveBehaviour;
    public bool IsBusy;
}
