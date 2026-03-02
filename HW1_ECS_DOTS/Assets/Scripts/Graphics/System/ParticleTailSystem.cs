using Unity.Entities;
using UnityEngine;

public partial class ParticleTailSystem : SystemBase
{
    protected override void OnUpdate()
    {
        foreach(var particleSystemReq in SystemAPI.Query<ParticleSystemReq>())
        {
           if(!particleSystemReq.ParticleSystem.isPlaying)
           {
               particleSystemReq.ParticleSystem.Play();
           }
        }
    }

}
